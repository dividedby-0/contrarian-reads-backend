using AutoMapper;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.Models;
using contrarian_reads_backend.Services.DTOs;
using contrarian_reads_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Services;

public class BookService : IBookService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BookService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ActionResult<BookDTO>> GetBook(string id)
    {
        if (!Guid.TryParse(id, out var guidId)) return new BadRequestObjectResult("Invalid GUID format.");

        var book = await _context.Books
            .Include(b => b.BookTags)
            .ThenInclude(bt => bt.Tag)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == guidId);

        if (book == null) return new NotFoundObjectResult("Book not found.");

        var bookDTO = _mapper.Map<BookDTO>(book);
        return new OkObjectResult(bookDTO);
    }

    public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
    {
        var books = await _context.Books
            .Include(b => b.BookTags)
            .ThenInclude(bt => bt.Tag)
            .Include(b => b.User)
            .ToListAsync();

        var bookDTOs = _mapper.Map<List<BookDTO>>(books);
        return new OkObjectResult(bookDTOs);
    }

    public async Task<ActionResult<BookDTO>> CreateBook(CreateBookDTO createBookDTO)
    {
        var existingBook = await _context.Books
            .FirstOrDefaultAsync(b =>
                b.Title.ToLower() == createBookDTO.Title.ToLower() &&
                b.Author.ToLower() == createBookDTO.Author.ToLower());

        if (existingBook != null)
        {
            var existingBookDTO = _mapper.Map<BookDTO>(existingBook);
            return new OkObjectResult(existingBookDTO);
        }

        var book = _mapper.Map<Book>(createBookDTO);

        book.Id = Guid.NewGuid();
        book.AddedBy = Guid.Parse(createBookDTO.AddedBy);
        book.CreatedAt = DateTime.UtcNow;

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        var bookDTO = _mapper.Map<BookDTO>(book);

        return new CreatedAtActionResult("GetBook", "Books", new { id = book.Id }, bookDTO);
    }

    public async Task<IActionResult> UpdateBook(string id, BookDTO bookDTO)
    {
        if (!Guid.TryParse(id, out var guidId)) return new BadRequestObjectResult("Invalid GUID format.");

        var existingBook = await _context.Books.FindAsync(guidId);

        if (existingBook == null) return new NotFoundObjectResult("Book not found.");

        existingBook.Title = bookDTO.Title;
        existingBook.Author = bookDTO.Author;
        existingBook.Description = bookDTO.Description;
        existingBook.CoverImageUrl = bookDTO.CoverImageUrl;

        await _context.SaveChangesAsync();

        var updatedBookDTO = _mapper.Map<BookDTO>(existingBook);
        return new OkObjectResult(updatedBookDTO);
    }

    public async Task<IActionResult> DeleteBook(string id)
    {
        if (!Guid.TryParse(id, out var guidId)) return new BadRequestObjectResult("Invalid GUID format.");

        var book = await _context.Books.FindAsync(guidId);

        if (book == null) return new NotFoundObjectResult("Book not found.");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return new NoContentResult();
    }

    public async Task<ActionResult<SearchBooksResultDTO>> SearchBooks(
        string? searchTerm,
        string? userId,
        int pageSize,
        DateTime? lastCreatedAt = null)
    {
        try
        {
            var bookIds = await GetFilteredBookIds(searchTerm, pageSize, lastCreatedAt);

            if (!bookIds.Any())
                return new NotFoundObjectResult(
                    string.IsNullOrWhiteSpace(searchTerm)
                        ? "No books found."
                        : "No books found matching the search term.");

            var books = await GetBooksWithDetails(bookIds);
            var suggestions = await GetSuggestionsWithDetails(bookIds);

            var groupedSuggestions = MapSuggestions(suggestions, userId);
            var booksWithSuggestions = MapBooksWithSuggestions(books, groupedSuggestions);

            var pagination = await ComputePaginationInfo(searchTerm, pageSize, booksWithSuggestions, lastCreatedAt);

            return new OkObjectResult(new SearchBooksResultDTO
            {
                BooksWithSuggestions = booksWithSuggestions,
                HasMoreBooks = pagination.HasMoreBooks,
                RemainingBooksCount = pagination.RemainingBooksCount,
                NextCursor = pagination.NextCursor
            });
        }
        catch
        {
            return new StatusCodeResult(500);
        }
    }

    private async Task<List<Guid>> GetFilteredBookIds(string? searchTerm, int pageSize, DateTime? lastCreatedAt)
    {
        var suggestedBookIds = _context.Suggestions
            .Select(s => s.BookId)
            .Distinct();

        var query = _context.Books
            .Where(b => suggestedBookIds.Contains(b.Id));

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));

        if (lastCreatedAt.HasValue)
            query = query.Where(b => b.CreatedAt < lastCreatedAt.Value);

        return await query
            .OrderByDescending(b => b.CreatedAt)
            .Take(pageSize)
            .Select(b => b.Id)
            .ToListAsync();
    }

    private async Task<List<Book>> GetBooksWithDetails(List<Guid> bookIds)
    {
        return await _context.Books
            .Where(b => bookIds.Contains(b.Id))
            .Include(b => b.BookTags).ThenInclude(bt => bt.Tag)
            .Include(b => b.User)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    private async Task<List<Suggestion>> GetSuggestionsWithDetails(List<Guid> bookIds)
    {
        return await _context.Suggestions
            .Where(s => bookIds.Contains(s.BookId))
            .Include(s => s.SuggestedBook)
            .Include(s => s.SuggestedByUser)
            .Include(s => s.Comments).ThenInclude(c => c.User)
            .Include(s => s.Upvotes)
            .ToListAsync();
    }

    private Dictionary<Guid, List<SuggestionWithCommentsDTO>> MapSuggestions(List<Suggestion> suggestions,
        string? userId)
    {
        return suggestions
            .GroupBy(s => s.BookId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(s =>
                {
                    var dto = _mapper.Map<SuggestionWithCommentsDTO>(s);
                    dto.UserHasUpvoted = !string.IsNullOrEmpty(userId) &&
                                         s.Upvotes.Any(u => u.UpvotedBy == Guid.Parse(userId));
                    return dto;
                }).ToList()
            );
    }

    private List<BookWithSuggestionsDTO> MapBooksWithSuggestions(List<Book> books,
        Dictionary<Guid, List<SuggestionWithCommentsDTO>> suggestionsDict)
    {
        return books
            .Select(b => new BookWithSuggestionsDTO(
                _mapper.Map<BookDTO>(b),
                suggestionsDict.TryGetValue(b.Id, out var suggestions)
                    ? suggestions
                    : new List<SuggestionWithCommentsDTO>()
            ))
            .Where(bws => bws.Suggestions.Any())
            .ToList();
    }

    private async Task<(bool HasMoreBooks, int RemainingBooksCount, DateTime? NextCursor)> ComputePaginationInfo(
        string? searchTerm, int pageSize, List<BookWithSuggestionsDTO> booksWithSuggestions, DateTime? lastCreatedAt)
    {
        if (booksWithSuggestions.Count < pageSize)
            return (false, 0, null);

        var nextCursor = booksWithSuggestions.Min(bws => bws.Book.CreatedAt);

        var suggestedBookIds = _context.Suggestions
            .Select(s => s.BookId)
            .Distinct();

        var remainingQuery = _context.Books
            .Where(b => suggestedBookIds.Contains(b.Id) && b.CreatedAt < nextCursor);

        if (!string.IsNullOrWhiteSpace(searchTerm))
            remainingQuery = remainingQuery.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));

        var remainingCount = await remainingQuery.CountAsync();

        return (remainingCount > 0, remainingCount, remainingCount > 0 ? nextCursor : null);
    }
}