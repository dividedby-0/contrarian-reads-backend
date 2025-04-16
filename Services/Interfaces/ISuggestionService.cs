using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace contrarian_reads_backend.Services.Interfaces;

public interface ISuggestionService
{
    Task<ActionResult<SuggestionResponseDTO>> GetSuggestion(string id, string userId);

    Task<ActionResult<List<SuggestionDTO>>> GetSuggestions();

    Task<ActionResult<int>> GetSuggestionCount();

    Task<ActionResult<int>> GetUpvoteCount(string id);

    Task<ActionResult<SuggestionDTO>> CreateSuggestion(CreateSuggestionDTO createSuggestionDTO);

    //Task<ActionResult<SuggestionDTO>> UpdateSuggestion(string id, CreateSuggestionDTO suggestion);

    Task<ActionResult<SuggestionDTO>> DeleteSuggestion(string id);

    Task<ActionResult<UpvoteResponseDTO>> UpvoteSuggestion(string suggestionId, Guid userId);
}