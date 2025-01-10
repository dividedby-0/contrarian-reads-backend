namespace contrarian_reads_backend.DTOs
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        //public ICollection<BookAlternativeDto>? BookAlternatives { get; set; }
        //public ICollection<BookAlternativeDto>? AlternativeToBooks { get; set; }
    }
}