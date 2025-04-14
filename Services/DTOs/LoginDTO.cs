using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public record LoginDTO(
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    string Email,
    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password is either too short or too long")]
    [DataType(DataType.Password)]
    string Password);