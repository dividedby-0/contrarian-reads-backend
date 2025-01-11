namespace contrarian_reads_backend.Models
{
    public class BookTag
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int TagId { get; set; }

        public Book Book { get; set; }
        public Tag Tag { get; set; }
    }

}
