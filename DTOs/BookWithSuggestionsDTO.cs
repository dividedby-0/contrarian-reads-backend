namespace contrarian_reads_backend.DTOs
{
    public class BookWithSuggestionsDTO
    {
        public BookDTO Book { get; set; }
        public List<SuggestionDTO> Suggestions { get; set; }
    }
}
