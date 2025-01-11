namespace contrarian_reads_backend.Models
{
    public class Upvote
    {
        public int Id { get; set; }
        public int SuggestionId { get; set; }
        public int UpvotedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public Suggestion Suggestion { get; set; }
        public User User { get; set; }
    }

}
