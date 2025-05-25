namespace contrarian_reads_backend.Services.DTOs;

public class SearchBooksResultDTO
{
    public List<BookWithSuggestionsDTO> BooksWithSuggestions { get; set; }
    public bool HasMoreBooks { get; set; }
    public int RemainingBooksCount { get; set; }
    public DateTime? NextCursor { get; set; }
}