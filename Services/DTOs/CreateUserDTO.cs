using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public class CreateUserDTO
{
    [Required] public Guid Id { get; set; }

    [Required] public string Username { get; set; } = null!;

    [Required] [EmailAddress] public string Email { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;

    public string? Bio { get; set; }
}