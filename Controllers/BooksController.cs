using AutoMapper;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.Services.DTOs;
using contrarian_reads_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace contrarian_reads_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BooksController(ApplicationDbContext context, IMapper mapper, IBookService bookService)
    {
        _bookService = bookService;
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
    {
        return await _bookService.GetBooks();
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDTO>> GetBook(string id)
    {
        return await _bookService.GetBook(id);
    }

    // POST: api/Books
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BookDTO>> CreateBook(CreateBookDTO createBookDTO)
    {
        return await _bookService.CreateBook(createBookDTO);
    }

    // PUT: api/Books/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(string id, BookDTO bookDTO)
    {
        return await _bookService.UpdateBook(id, bookDTO);
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(string id)
    {
        return await _bookService.DeleteBook(id);
    }

    // GET: api/Books/search?searchTerm=&pageSize=10
    [HttpGet("search")]
    public async Task<ActionResult<SearchBooksResultDTO>> SearchBooks(string? searchTerm, string? userId,
        int pageSize,
        DateTime? lastCreatedAt = null)
    {
        return await _bookService.SearchBooks(searchTerm, userId, pageSize, lastCreatedAt);
    }
}