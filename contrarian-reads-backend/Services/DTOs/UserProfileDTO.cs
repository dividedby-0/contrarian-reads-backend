namespace contrarian_reads_backend.Services.DTOs;

public record UserProfileDTO(
    UserDTO User,
    ICollection<SuggestionDTO> AddedSuggestions,
    ICollection<SuggestionDTO> UpvotedSuggestions,
    ICollection<CommentDTO> Comments
);