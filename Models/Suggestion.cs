namespace contrarian_reads_backend.Models
{
    public class Suggestion
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid SuggestedBookId { get; set; }
        public Guid SuggestedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }

        public Book Book { get; set; } // Navigation property
        public Book SuggestedBook { get; set; } // Navigation property
        public User SuggestedByUser { get; set; } // Navigation property
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Upvote> Upvotes { get; set; }
    }

}
