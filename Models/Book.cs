namespace contrarian_reads_backend.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public ICollection<BookAlternative>? BookAlternatives { get; set; }
        public ICollection<BookAlternative>? AlternativeToBooks { get; set; }
    }

    public class BookAlternative
    {
        public Guid Id { get; set; }
        public Guid OriginalBookId { get; set; }
        public Guid AlternativeBookId { get; set; }
        public Book? OriginalBook { get; set; }
        public Book? AlternativeBook { get; set; }
        public ICollection<BookAlternativeUpvote>? BookAlternativeUpvotes { get; set; }
    }

    public class BookAlternativeUpvote
    {
        public Guid Id { get; set; }
        public Guid BookAlternativeId { get; set; }
        public BookAlternative? BookAlternative { get; set; }
        public Guid UserId { get; set; }
    }
}