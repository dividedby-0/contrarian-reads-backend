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
            var allBooksWithSuggestions = _context.Suggestions
                .Select(s => s.BookId)
                .Distinct();

            var booksQuery = _context.Books
                .Where(b => allBooksWithSuggestions.Contains(b.Id));

            if (!string.IsNullOrWhiteSpace(searchTerm))
                booksQuery = booksQuery.Where(b =>
                    b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));

            if (lastCreatedAt.HasValue) booksQuery = booksQuery.Where(b => b.CreatedAt < lastCreatedAt.Value);

            booksQuery = booksQuery.OrderByDescending(b => b.CreatedAt).Take(pageSize);

            var bookIds = await booksQuery.Select(b => b.Id).ToListAsync();

            if (!bookIds.Any())
                return new NotFoundObjectResult(
                    string.IsNullOrWhiteSpace(searchTerm)
                        ? "No books found."
                        : "No books found matching the search term.");

            var books = await _context.Books
                .Where(b => bookIds.Contains(b.Id))
                .Include(b => b.BookTags).ThenInclude(bt => bt.Tag)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            var suggestions = await _context.Suggestions
                .Where(s => bookIds.Contains(s.BookId))
                .Include(s => s.SuggestedBook)
                .Include(s => s.SuggestedByUser)
                .Include(s => s.Comments).ThenInclude(c => c.User)
                .Include(s => s.Upvotes)
                .ToListAsync();

            var groupedSuggestions = suggestions
                .GroupBy(s => s.BookId)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList().Select(s =>
                    {
                        var suggestionDto = _mapper.Map<SuggestionWithCommentsDTO>(s);
                        suggestionDto.UserHasUpvoted = !string.IsNullOrEmpty(userId) &&
                                                       s.Upvotes.Any(u => u.UpvotedBy == Guid.Parse(userId));
                        return suggestionDto;
                    }).ToList()
                );

            var booksWithSuggestions = books
                .Select(b => new BookWithSuggestionsDTO(
                    _mapper.Map<BookDTO>(b),
                    groupedSuggestions.TryGetValue(b.Id, out var bookSuggestions)
                        ? bookSuggestions
                        : new List<SuggestionWithCommentsDTO>()
                ))
                .Where(bws => bws.Suggestions.Any())
                .ToList();

            DateTime? nextCursor = null;
            var hasMoreBooks = false;
            var remainingBooksCount = 0;

            if (booksWithSuggestions.Count < pageSize)
            {
                hasMoreBooks = false;
                remainingBooksCount = 0;
                nextCursor = null;
            }
            else
            {
                nextCursor = booksWithSuggestions.Min(bws => bws.Book.CreatedAt);

                var remainingBooksQuery = _context.Books
                    .Where(b => allBooksWithSuggestions.Contains(b.Id));
                if (!string.IsNullOrWhiteSpace(searchTerm))
                    remainingBooksQuery = remainingBooksQuery.Where(b =>
                        b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));
                remainingBooksQuery = remainingBooksQuery.Where(b => b.CreatedAt < nextCursor.Value);

                remainingBooksCount = await remainingBooksQuery.CountAsync();
                hasMoreBooks = remainingBooksCount > 0;

                if (!hasMoreBooks) nextCursor = null;
            }

            return new OkObjectResult(new SearchBooksResultDTO
            {
                BooksWithSuggestions = booksWithSuggestions,
                HasMoreBooks = hasMoreBooks,
                RemainingBooksCount = remainingBooksCount,
                NextCursor = nextCursor
            });
        }
        catch (Exception)
        {
            return new StatusCodeResult(500);
        }
    }
}