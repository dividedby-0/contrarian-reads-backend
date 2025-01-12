namespace contrarian_reads_backend.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Guid AddedBy { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public float? Rating { get; set; }
        public ICollection<BookTag> BookTags { get; set; }

        public User User { get; set; } // Navigation property
    }

}
