namespace contrarian_reads_backend.DTOs
{
    public class BookWithSuggestionsDTO
    {
        public BookDTO Book { get; set; }
        public List<SuggestionWithCommentsDTO> Suggestions { get; set; }
    }
}