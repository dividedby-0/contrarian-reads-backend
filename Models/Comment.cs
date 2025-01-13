namespace contrarian_reads_backend.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid SuggestionId { get; set; }
        public Guid CommentedBy { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? ParentId { get; set; }
        public Suggestion Suggestion { get; set; }
        public User User { get; set; }
        public Comment Parent { get; set; }
        public ICollection<Comment> Replies { get; set; }
    }

}
