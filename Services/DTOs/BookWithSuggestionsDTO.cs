namespace contrarian_reads_backend.Services.DTOs;

public record BookWithSuggestionsDTO(BookDTO Book, List<SuggestionWithCommentsDTO> Suggestions);