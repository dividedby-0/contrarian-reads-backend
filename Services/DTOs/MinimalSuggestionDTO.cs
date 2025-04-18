namespace contrarian_reads_backend.Services.DTOs;

public record MinimalSuggestionDTO(
    Guid Id,
    string SuggestedbookTitle,
    string SuggestedbookAuthor,
    string MainbookTitle,
    string MainbookAuthor
);