using contrarian_reads_backend.Services;
using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace contrarian_reads_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    // GET: api/Comments/{suggestionId}
    [HttpGet("{suggestionId}")]
    public async Task<ActionResult<List<CommentDTO>>> GetCommentsBySuggestionId(string suggestionId)
    {
        return await _commentService.GetCommentsBySuggestionId(suggestionId);
    }

    // POST: api/Comments
    [HttpPost]
    public async Task<ActionResult<CommentDTO>> CreateComment(CreateCommentDTO createCommentDTO)
    {
        return await _commentService.CreateComment(createCommentDTO);
    }
}