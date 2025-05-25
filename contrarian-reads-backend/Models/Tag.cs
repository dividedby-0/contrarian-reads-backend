namespace contrarian_reads_backend.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<BookTag> BookTags { get; set; }
    }

}
