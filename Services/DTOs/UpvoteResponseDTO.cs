namespace contrarian_reads_backend.Services.DTOs;

public record UpvoteResponseDTO(Guid SuggestionId, bool UpvoteAlreadyExists);