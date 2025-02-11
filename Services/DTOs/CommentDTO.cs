namespace contrarian_reads_backend.Services.DTOs;

public record CommentDTO(
    Guid Id,
    Guid SuggestionId,
    UserDTO User,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    Guid? ParentId,
    IEnumerable<CommentDTO> Replies
);