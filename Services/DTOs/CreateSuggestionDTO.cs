namespace contrarian_reads_backend.Services.DTOs;

public record CreateSuggestionDTO(
    string BookId,
    BookDTO? Book,
    string SuggestedBookId,
    BookDTO? SuggestedBook,
    string SuggestedByUserId,
    UserDTO? SuggestedByUser,
    DateTime CreatedAt,
    string Reason,
    int UpvoteCount);