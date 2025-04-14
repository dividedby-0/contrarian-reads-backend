using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public record CommentDTO(
    [Required] Guid Id,
    Guid SuggestionId,
    [Required] UserDTO User,
    [Required]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Comment content must be between 1 and 150 characters")]
    string Content,
    DateTime CreatedAt,
    IEnumerable<CommentDTO> Replies,
    DateTime? UpdatedAt = null,
    Guid? ParentId = null
);