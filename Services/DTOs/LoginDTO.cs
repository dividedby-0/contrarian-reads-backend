using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public record LoginDTO(
    [Required] [EmailAddress] string Email,
    [Required]
    [DataType(DataType.Password)]
    string Password);