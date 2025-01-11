namespace contrarian_reads_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Bio { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<Book> Books { get; set; }
        public ICollection<Suggestion> Suggestions { get; set; }
        public ICollection<Upvote> Upvotes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

}
