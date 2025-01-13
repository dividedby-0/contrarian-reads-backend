namespace contrarian_reads_backend.Models
{
    public class BookTag
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid TagId { get; set; }
        public Book Book { get; set; }
        public Tag Tag { get; set; }
    }

}
