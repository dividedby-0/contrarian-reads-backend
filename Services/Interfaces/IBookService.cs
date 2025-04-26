using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace contrarian_reads_backend.Services.Interfaces;

public interface IBookService
{
    Task<ActionResult<BookDTO>> GetBook(string id);
    Task<ActionResult<IEnumerable<BookDTO>>> GetBooks();
    Task<ActionResult<BookDTO>> CreateBook(CreateBookDTO createBookDTO);
    Task<IActionResult> UpdateBook(string id, BookDTO bookDTO);
    Task<IActionResult> DeleteBook(string id);

    Task<ActionResult<SearchBooksResultDTO>> SearchBooks(string? searchTerm, string? userId, int pageSize,
        DateTime? lastCreatedAt = null);
}