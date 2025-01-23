namespace contrarian_reads_backend.DTOs
{
    public class CreateBookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string AddedBy { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public float? Rating { get; set; }
    }
}
