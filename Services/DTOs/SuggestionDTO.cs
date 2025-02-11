namespace contrarian_reads_backend.Services.DTOs;

public record SuggestionDTO(
    Guid Id,
    BookDTO? SuggestedBook,
    UserDTO? SuggestedByUser,
    DateTime CreatedAt,
    string Reason,
    int UpvoteCount
);