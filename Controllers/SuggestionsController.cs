using contrarian_reads_backend.Services;
using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace contrarian_reads_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuggestionsController : ControllerBase
{
    private readonly ISuggestionService _suggestionService;

    public SuggestionsController(ISuggestionService suggestionService)
    {
        _suggestionService = suggestionService;
    }

    // GET: api/Suggestions/all
    [HttpGet("all")]
    public async Task<ActionResult<List<SuggestionDTO>>> GetSuggestions()
    {
        return await _suggestionService.GetSuggestions();
    }

    // GET: api/Suggestions/count
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetSuggestionCount()
    {
        return await _suggestionService.GetSuggestionCount();
    }

    // GET: api/Suggestions?suggestionId=5&userId=1
    [HttpGet]
    public async Task<ActionResult<SuggestionResponseDTO>> GetSuggestion([FromQuery] string suggestionId,
        [FromQuery] string? userId)
    {
        return await _suggestionService.GetSuggestion(suggestionId, userId);
    }

    // GET: api/Suggestions/5/upvotes
    [HttpGet("{id}/upvotes")]
    public async Task<ActionResult<int>> GetUpvoteCount(string id)
    {
        return await _suggestionService.GetUpvoteCount(id);
    }

    // POST: api/Suggestions
    [HttpPost]
    public async Task<ActionResult<SuggestionDTO>> CreateSuggestion(CreateSuggestionDTO createSuggestionDTO)
    {
        return await _suggestionService.CreateSuggestion(createSuggestionDTO);
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
        await _suggestionService.DeleteSuggestion(id);
        return NoContent();
    }

    // POST: api/Suggestions/5/upvote
    [HttpPost("{suggestionId}/upvote")]
    public async Task<ActionResult<UpvoteResponseDTO>> UpvoteSuggestion(string suggestionId, Guid userId)
    {
        var result = await _suggestionService.UpvoteSuggestion(suggestionId, userId);

        if (result.Result is ObjectResult objectResult && objectResult.StatusCode == StatusCodes.Status400BadRequest)
            return BadRequest(objectResult.Value);

        return result;
    }
}