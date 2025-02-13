using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace contrarian_reads_backend.Services;

public interface ICommentService
{
    Task<ActionResult<List<CommentDTO>>> GetCommentsBySuggestionId(string suggestionId);

    Task<ActionResult<CommentDTO>> CreateComment(CreateCommentDTO createCommentDTO);
}