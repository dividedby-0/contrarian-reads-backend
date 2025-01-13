namespace contrarian_reads_backend.DTOs
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public float? Rating { get; set; }

        //public UserDTO AddedByUser { get; set; }
        //public IEnumerable<string> Tags { get; set; }
    }
}