using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public record CreateBookDTO(
    [Required]
    [StringLength(200, MinimumLength = 1)]
    string Title,
    [Required]
    [StringLength(100, MinimumLength = 1)]
    string Author,
    [Required] string AddedBy,
    [StringLength(2000)] string? Description = null,
    [Url] string? CoverImageUrl = null,
    [Range(0, 5)] float? Rating = null);