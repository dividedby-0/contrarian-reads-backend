using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public record CreateCommentDTO(
    [Required] Guid SuggestionId,
    [Required]
    [StringLength(150, ErrorMessage = "Comment content cannot be longer than 150 characters.")]
    [MinLength(1)]
    string Content,
    [Required] Guid CommentedByUserId,
    Guid? ParentId
);