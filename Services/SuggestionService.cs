using AutoMapper;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.Models;
using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Services;

public class SuggestionService : ISuggestionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SuggestionService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ActionResult<SuggestionDTO>> GetSuggestion(string id)

    {
        if (!Guid.TryParse(id, out var guidId)) return new BadRequestObjectResult("Invalid GUID format.");

        var suggestion = await _context.Suggestions
            .Include(s => s.Book)
            .Include(s => s.SuggestedBook)
            .Include(s => s.SuggestedByUser)
            .FirstOrDefaultAsync(s => s.Id == guidId);

        if (suggestion == null) return new NotFoundObjectResult("Suggestion not found.");

        return new OkObjectResult(_mapper.Map<SuggestionDTO>(suggestion));
    }

    public async Task<ActionResult<int>> GetSuggestionCount()
    {
        var count = await _context.Suggestions.CountAsync();
        return new OkObjectResult(count);
    }

    public async Task<ActionResult<List<SuggestionDTO>>> GetSuggestions()
    {
        var suggestions = await _context.Suggestions
            .Include(s => s.Book)
            .Include(s => s.SuggestedBook)
            .Include(s => s.SuggestedByUser)
            .ToListAsync();

        return new OkObjectResult(_mapper.Map<IEnumerable<SuggestionDTO>>(suggestions));
    }

    public async Task<ActionResult<SuggestionDTO>> CreateSuggestion(CreateSuggestionDTO createSuggestionDTO)
    {
        if (!Guid.TryParse(createSuggestionDTO.BookId, out var bookId) ||
            !Guid.TryParse(createSuggestionDTO.SuggestedBookId, out var suggestedBookId))
            return new BadRequestObjectResult("Invalid BookId or SuggestedBookId format.");

        if (await _context.Suggestions.AnyAsync(s =>
                s.BookId == bookId &&
                s.SuggestedBookId == suggestedBookId))
            return new ConflictObjectResult("A suggestion with this Book and SuggestedBook already exists.");

        if (!Guid.TryParse(createSuggestionDTO.SuggestedByUserId, out var suggestedByUserId))
            return new BadRequestObjectResult("Invalid SuggestedByUserId format.");

        var book = await _context.Books.FindAsync(bookId);
        var suggestedBook = await _context.Books.FindAsync(suggestedBookId);
        var suggestedByUser = await _context.Users.FindAsync(suggestedByUserId);

        if (book == null || suggestedBook == null || suggestedByUser == null)
            return new NotFoundObjectResult("Book, SuggestedBook, or SuggestedByUser not found.");

        var suggestion = _mapper.Map<Suggestion>(createSuggestionDTO);
        suggestion.Id = Guid.NewGuid();
        suggestion.Book = book;
        suggestion.SuggestedBook = suggestedBook;
        suggestion.SuggestedByUser = suggestedByUser;
        suggestion.CreatedAt = DateTime.UtcNow;
        suggestion.Reason = createSuggestionDTO.Reason;
        suggestion.Upvotes = new List<Upvote>();

        _context.Suggestions.Add(suggestion);
        await _context.SaveChangesAsync();

        var createdSuggestionDTO = new SuggestionDTO(
            suggestion.Id,
            _mapper.Map<BookDTO>(suggestedBook),
            _mapper.Map<UserDTO>(suggestedByUser),
            Reason: suggestion.Reason,
            CreatedAt: suggestion.CreatedAt,
            UpvoteCount: suggestion.Upvotes.Count
        );
        return new OkObjectResult(createdSuggestionDTO);
    }

    public async Task<ActionResult<SuggestionDTO>> DeleteSuggestion(string id)
    {
        if (!Guid.TryParse(id, out var guidId)) return new BadRequestObjectResult("Invalid GUID format.");
        var suggestion = await _context.Suggestions.FindAsync(guidId);
        if (suggestion == null) return new NotFoundObjectResult("Suggestion not found.");
        _context.Suggestions.Remove(suggestion);
        await _context.SaveChangesAsync();
        return new OkObjectResult(_mapper.Map<SuggestionDTO>(suggestion));
    }

    public async Task<ActionResult<SuggestionDTO>> UpvoteSuggestion(string suggestionId, Guid userId)
    {
        if (!Guid.TryParse(suggestionId, out var guidSuggestionId))
            return new BadRequestObjectResult("Invalid SuggestionId format.");

        var existingUpvote = await _context.Upvotes
            .FirstOrDefaultAsync(u => u.SuggestionId == guidSuggestionId && u.UpvotedBy == userId);

        if (existingUpvote != null)
        {
            _context.Upvotes.Remove(existingUpvote);
        }
        else
        {
            var upvote = new Upvote
            {
                Id = Guid.NewGuid(),
                SuggestionId = guidSuggestionId,
                UpvotedBy = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Upvotes.Add(upvote);
        }

        await _context.SaveChangesAsync();

        return new OkObjectResult(guidSuggestionId);
    }
}