namespace contrarian_reads_backend.Services.DTOs;

public record SuggestionWithCommentsDTO(
    Guid Id,
    BookDTO? SuggestedBook,
    UserDTO? SuggestedByUser,
    DateTime CreatedAt,
    string Reason,
    int UpvoteCount,
    List<CommentDTO> Comments
)
{
    public SuggestionWithCommentsDTO() : this(Guid.Empty, null, null, default, "", 0, new List<CommentDTO>())
    {
    }

    public bool UserHasUpvoted { get; set; }
}