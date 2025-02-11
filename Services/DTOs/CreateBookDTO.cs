namespace contrarian_reads_backend.Services.DTOs;

public record CreateBookDTO(
    string Title,
    string Author,
    string AddedBy,
    string? Description = null,
    string? CoverImageUrl = null,
    float? Rating = null);