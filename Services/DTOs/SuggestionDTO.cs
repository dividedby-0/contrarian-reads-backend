namespace contrarian_reads_backend.Services.DTOs
{
    public class SuggestionDTO
    {
        public Guid Id { get; set; }
        public BookDTO? SuggestedBook { get; set; }
        public UserDTO? SuggestedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }
        public int UpvoteCount { get; set; }
    }
}