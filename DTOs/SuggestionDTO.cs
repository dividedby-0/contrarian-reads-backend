namespace contrarian_reads_backend.DTOs
{
    public class SuggestionDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public BookDTO Book { get; set; }
        public int SuggestedBookId { get; set; }
        public BookDTO SuggestedBook { get; set; }
        public Guid SuggestedByUserId { get; set; }
        public UserDTO SuggestedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }
        public int UpvoteCount { get; set; }
    }
}
