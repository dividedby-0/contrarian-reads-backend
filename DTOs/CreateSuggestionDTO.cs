namespace contrarian_reads_backend.DTOs
{
    public class CreateSuggestionDTO
    {
        public string BookId { get; set; }
        public BookDTO? Book { get; set; }
        public string SuggestedBookId { get; set; }
        public BookDTO? SuggestedBook { get; set; }
        public string SuggestedByUserId { get; set; }
        public UserDTO? SuggestedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }
        public int UpvoteCount { get; set; }
    }
}
