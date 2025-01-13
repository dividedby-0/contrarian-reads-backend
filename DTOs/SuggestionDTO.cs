namespace contrarian_reads_backend.DTOs
{
    public class SuggestionDTO
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public BookDTO Book { get; set; }
        public Guid SuggestedBookId { get; set; }
        public BookDTO SuggestedBook { get; set; }
        public Guid SuggestedByUserId { get; set; }
        public UserDTO SuggestedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }
        public int UpvoteCount { get; set; }
    }
}
