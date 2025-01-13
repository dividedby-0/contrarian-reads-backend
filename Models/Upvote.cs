namespace contrarian_reads_backend.Models
{
    public class Upvote
    {
        public Guid Id { get; set; }
        public Guid SuggestionId { get; set; }
        public Guid UpvotedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public Suggestion Suggestion { get; set; }
        public User User { get; set; }
    }

}
