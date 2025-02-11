using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public record CreateUserDTO(
    [property: Required] Guid Id,
    [property: Required] string Username,
    [property: Required]
    [property: EmailAddress]
    string Email,
    string? ProfilePictureUrl,
    DateTime CreatedAt,
    [property: Required]
    [property: StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [property: DataType(DataType.Password)]
    string Password,
    [property: DataType(DataType.Password)]
    [property: Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    string ConfirmPassword,
    string? Bio);