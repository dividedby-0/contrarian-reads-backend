using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public record LoginDTO(
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    string Email,
    [Required]
    [DataType(DataType.Password)]
    string Password);