namespace contrarian_reads_backend.DTOs
{
    public class BookAlternativeDto
    {
        public Guid Id { get; set; }
        public required BookDto OriginalBook { get; set; }
        public required BookDto AlternativeBook { get; set; }
    }
}
