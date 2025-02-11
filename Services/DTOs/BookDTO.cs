namespace contrarian_reads_backend.Services.DTOs;

public record BookDTO(
    Guid Id,
    string Title,
    string Author,
    string Description,
    string CoverImageUrl);