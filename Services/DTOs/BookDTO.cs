using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public record BookDTO(
    [Required] Guid Id,
    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    string Title,
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Author must be between 1 and 100 characters")]
    string Author,
    DateTime CreatedAt,
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    string? Description = null,
    [Url(ErrorMessage = "Invalid URL format for cover image")]
    string? CoverImageUrl = null,
    UserDTO? User = null);