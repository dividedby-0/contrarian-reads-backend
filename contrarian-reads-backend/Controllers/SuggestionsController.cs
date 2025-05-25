using contrarian_reads_backend.Services;
using contrarian_reads_backend.Services.DTOs;
using contrarian_reads_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

    // POST: api/Suggestions/5/upvote
    [Authorize]
    [HttpPost("{suggestionId}/upvote")]
    public async Task<ActionResult<UpvoteResponseDTO>> UpvoteSuggestion(string suggestionId, Guid userId)
    {
        var result = await _suggestionService.UpvoteSuggestion(suggestionId, userId);

        if (result.Result is ObjectResult objectResult && objectResult.StatusCode == StatusCodes.Status400BadRequest)
            return BadRequest(objectResult.Value);

        return result;
    }

    // POST: api/Suggestions
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SuggestionDTO>> CreateSuggestion(CreateSuggestionDTO createSuggestionDTO)
    {
        return await _suggestionService.CreateSuggestion(createSuggestionDTO);
    }

    // DELETE: api/Suggestions/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSuggestion(string id)
    {
        await _suggestionService.DeleteSuggestion(id);
        return NoContent();
    }
}