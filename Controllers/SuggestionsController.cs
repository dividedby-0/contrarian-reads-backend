using AutoMapper;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.DTOs;
using contrarian_reads_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SuggestionsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Suggestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuggestionDTO>>> GetSuggestions()
        {
            var suggestions = await _context.Suggestions
                .Include(s => s.Book)
                .Include(s => s.SuggestedBook)
                .Include(s => s.SuggestedByUser)
                .ToListAsync();

            var suggestionDTOs = _mapper.Map<List<SuggestionDTO>>(suggestions);
            return Ok(suggestionDTOs);
        }

        // GET: api/Suggestions/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetSuggestionCount()
        {
            var count = await _context.Suggestions.CountAsync();
            return Ok(count);
        }

        // GET: api/Suggestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SuggestionDTO>> GetSuggestion(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var suggestion = await _context.Suggestions
                .Include(s => s.Book)
                .Include(s => s.SuggestedBook)
                .Include(s => s.SuggestedByUser)
                .FirstOrDefaultAsync(s => s.Id == guidId);

            if (suggestion == null)
            {
                return NotFound();
            }

            var suggestionDTO = _mapper.Map<SuggestionDTO>(suggestion);
            return Ok(suggestionDTO);
        }

        // POST: api/Suggestions
        [HttpPost]
        public async Task<ActionResult<SuggestionDTO>> CreateSuggestion(CreateSuggestionDTO createSuggestionDTO)
        {
            if (!Guid.TryParse(createSuggestionDTO.BookId, out var bookId) ||
                !Guid.TryParse(createSuggestionDTO.SuggestedBookId, out var suggestedBookId))
            {
                return BadRequest("Invalid BookId or SuggestedBookId format.");
            }

            if (await _context.Suggestions.AnyAsync(s =>
                s.BookId == bookId &&
                s.SuggestedBookId == suggestedBookId))
            {
                return Conflict("A suggestion with this Book and SuggestedBook already exists.");
            }

            if (!Guid.TryParse(createSuggestionDTO.SuggestedByUserId, out var suggestedByUserId))
            {
                return BadRequest("Invalid SuggestedByUserId format.");
            }

            var book = await _context.Books.FindAsync(bookId);
            var suggestedBook = await _context.Books.FindAsync(suggestedBookId);
            var suggestedByUser = await _context.Users.FindAsync(suggestedByUserId);

            if (book == null || suggestedBook == null || suggestedByUser == null)
            {
                return NotFound("Book, SuggestedBook, or SuggestedByUser not found.");
            }

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

            var createdSuggestionDTO = _mapper.Map<SuggestionDTO>(suggestion);
            createdSuggestionDTO.SuggestedBook = _mapper.Map<BookDTO>(suggestedBook);
            createdSuggestionDTO.SuggestedByUser = _mapper.Map<UserDTO>(suggestedByUser);

            return CreatedAtAction("GetSuggestion", new { id = suggestion.Id }, createdSuggestionDTO);
        }

        //TODO: reimplement suggestion put
        // PUT: api/Suggestions/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateSuggestion(string id, SuggestionDTO suggestionDTO)
        //{
        //    if (!Guid.TryParse(id, out var guidId))
        //    {
        //        return BadRequest("Invalid GUID format.");
        //    }

        //    if (await _context.Suggestions.AnyAsync(s =>
        //        s.Id != guidId &&
        //        s.BookId == suggestionDTO.BookId &&
        //        s.SuggestedBookId == suggestionDTO.SuggestedBookId))
        //    {
        //        return Conflict("A suggestion with this Book and SuggestedBook already exists.");
        //    }

        //    var existingSuggestion = await _context.Suggestions
        //        .Include(s => s.Book)
        //        .Include(s => s.SuggestedBook)
        //        .Include(s => s.SuggestedByUser)
        //        .FirstOrDefaultAsync(s => s.Id == guidId);

        //    if (existingSuggestion == null)
        //    {
        //        return NotFound();
        //    }

        //    existingSuggestion.Reason = suggestionDTO.Reason;
        //    existingSuggestion.BookId = suggestionDTO.BookId;
        //    existingSuggestion.SuggestedBookId = suggestionDTO.SuggestedBookId;

        //    await _context.SaveChangesAsync();

        //    var updatedSuggestionDTO = _mapper.Map<SuggestionDTO>(existingSuggestion);
        //    updatedSuggestionDTO.Book = _mapper.Map<BookDTO>(existingSuggestion.Book);
        //    updatedSuggestionDTO.SuggestedBook = _mapper.Map<BookDTO>(existingSuggestion.SuggestedBook);
        //    updatedSuggestionDTO.SuggestedByUser = _mapper.Map<UserDTO>(existingSuggestion.SuggestedByUser);

        //    return Ok(updatedSuggestionDTO);
        //}

        // DELETE: api/Suggestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuggestion(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var suggestion = await _context.Suggestions.FindAsync(guidId);

            if (suggestion == null)
            {
                return NotFound();
            }

            _context.Suggestions.Remove(suggestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

