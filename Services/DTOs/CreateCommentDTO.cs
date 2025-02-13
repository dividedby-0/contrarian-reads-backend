namespace contrarian_reads_backend.Services.DTOs;

public record CreateCommentDTO(
    Guid SuggestionId,
    string Content,
    Guid CommentedByUserId
);