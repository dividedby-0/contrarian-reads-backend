using System.ComponentModel.DataAnnotations;

namespace contrarian_reads_backend.Services.DTOs;

public record CreateSuggestionDTO(
    [Required]
    [RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
        ErrorMessage = "Invalid BookId format")]
    string BookId,
    [Required]
    [RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
        ErrorMessage = "Invalid SuggestedBookId format")]
    string SuggestedBookId,
    [Required]
    [RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
        ErrorMessage = "Invalid UserId format")]
    string SuggestedByUserId,
    [Required]
    [StringLength(150, MinimumLength = 10, ErrorMessage = "Reason must be between 10 and 150 characters")]
    string Reason,
    BookDTO? Book = null,
    BookDTO? SuggestedBook = null,
    UserDTO? SuggestedByUser = null,
    DateTime? CreatedAt = null,
    int UpvoteCount = 0
);