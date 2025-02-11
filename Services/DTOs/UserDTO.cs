namespace contrarian_reads_backend.Services.DTOs;

public record UserDTO(
    Guid Id,
    string Username,
    string Email,
    string ProfilePictureUrl,
    DateTime CreatedAt,
    string Bio);