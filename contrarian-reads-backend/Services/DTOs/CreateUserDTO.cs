using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public class CreateUserDTO
{
    [Required] public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9_-]+$",
        ErrorMessage = "Username can only contain letters, numbers, underscores and hyphens")]
    public string Username { get; set; } = null!;

    [Required] [EmailAddress] public string Email { get; set; } = null!;

    [Url] public string? ProfilePictureUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    [StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage =
            "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;

    [StringLength(500)] public string? Bio { get; set; }
}