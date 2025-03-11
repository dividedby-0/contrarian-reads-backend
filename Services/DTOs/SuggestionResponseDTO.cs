namespace contrarian_reads_backend.Services.DTOs;

public record SuggestionResponseDTO(
    Guid? Id = null,
    BookDTO? SuggestedBook = null,
    UserDTO? SuggestedByUser = null,
    DateTime? CreatedAt = null,
    string? Reason = null,
    int? UpvoteCount = null,
    bool? upvoteAlreadyExists = null
);