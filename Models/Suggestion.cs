namespace contrarian_reads_backend.Models
{
    public class Suggestion
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int SuggestedBookId { get; set; }
        public int SuggestedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }

        public Book Book { get; set; } // Navigation property
        public Book SuggestedBook { get; set; } // Navigation property
        public User SuggestedByUser { get; set; } // Navigation property
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Upvote> Upvotes { get; set; }
    }

}
