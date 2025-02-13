using AutoMapper;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.Models;
using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Services;

public class CommentService : ICommentService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CommentService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ActionResult<List<CommentDTO>>> GetCommentsBySuggestionId(string suggestionId)
    {
        if (!Guid.TryParse(suggestionId, out var guidSuggestionId))
            return new BadRequestObjectResult("Invalid SuggestionId format.");

        var comments = await _context.Comments
            .Where(c => c.SuggestionId == guidSuggestionId)
            .Include(c => c.User)
            .ToListAsync();

        var commentDictionary = comments.ToDictionary(c => c.Id);

        foreach (var comment in comments)
            if (comment.ParentId != null && commentDictionary.ContainsKey(comment.ParentId.Value))
            {
                var parentComment = commentDictionary[comment.ParentId.Value];
                if (parentComment.Replies == null)
                    parentComment.Replies = new List<Comment>();

                parentComment.Replies.Add(comment);
            }

        var topLevelComments = comments.Where(c => c.ParentId == null).ToList();

        var commentDTOs = _mapper.Map<List<CommentDTO>>(topLevelComments);
        return new OkObjectResult(commentDTOs);
    }


    public async Task<ActionResult<CommentDTO>> CreateComment(CreateCommentDTO createCommentDTO)
    {
        if (!Guid.TryParse(createCommentDTO.SuggestionId.ToString(), out var suggestionId))
            return new BadRequestObjectResult("Invalid SuggestionId format.");

        if (string.IsNullOrWhiteSpace(createCommentDTO.Content))
            return new BadRequestObjectResult("Comment content cannot be empty.");

        if (!Guid.TryParse(createCommentDTO.CommentedByUserId.ToString(), out var commentedByUserId))
            return new BadRequestObjectResult("Invalid CommentedByUserId format.");

        var suggestion = await _context.Suggestions.FindAsync(suggestionId);
        if (suggestion == null)
            return new NotFoundObjectResult("Suggestion not found.");

        var user = await _context.Users.FindAsync(commentedByUserId);
        if (user == null)
            return new NotFoundObjectResult("User not found.");

        var comment = _mapper.Map<Comment>(createCommentDTO);
        comment.Id = Guid.NewGuid();
        comment.SuggestionId = suggestionId;
        comment.CommentedBy = commentedByUserId;
        comment.Content = createCommentDTO.Content;
        comment.CreatedAt = DateTime.UtcNow;
        comment.UpdatedAt = DateTime.UtcNow;

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        var commentDTO = _mapper.Map<CommentDTO>(comment);
        return new OkObjectResult(commentDTO);
    }
}