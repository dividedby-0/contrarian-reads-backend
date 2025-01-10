using AutoMapper;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.DTOs;
using contrarian_reads_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(ApplicationDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.BookAlternatives)
                .ThenInclude(ba => ba.AlternativeBook)
                .Include(b => b.AlternativeToBooks)
                .ThenInclude(ba => ba.OriginalBook)
                .ToListAsync();

            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);

            return Ok(bookDtos);
        }

        [HttpGet("{originalBookId}")]
        public async Task<ActionResult<Book>> GetBook(Guid originalBookId)
        {
            var book = await _context.Books.FindAsync(originalBookId);

            if (book == null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BookDto>(book);

            return Ok(bookDto);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] Book book)
        {
            book.Id = Guid.NewGuid();

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var bookDto = _mapper.Map<BookDto>(book);
            return CreatedAtAction(nameof(GetBook), new { originalBookId = book.Id }, bookDto);
        }

        [HttpGet("{originalBookId}/alternatives")]
        public async Task<ActionResult<IEnumerable<BookAlternativeDto>>> GetAlternativesForBook(Guid originalBookId)
        {
            var alternatives = await _context.BookAlternatives
                .Include(ba => ba.OriginalBook)
                .Include(ba => ba.AlternativeBook)
                .Where(ba => ba.OriginalBookId == originalBookId)
                .ToListAsync();

            if (alternatives.Count == 0)
            {
                return NotFound();
            }

            var alternativeDtos = _mapper.Map<IEnumerable<BookAlternativeDto>>(alternatives);
            return Ok(alternativeDtos);
        }

        [HttpPost("{originalBookId}/alternatives")]
        public async Task<ActionResult<BookAlternativeDto>> AddBookAlternative(Guid originalBookId, [FromBody] BookAlternative alternative)
        {
            var originalBook = await _context.Books.FindAsync(originalBookId);
            if (originalBook == null)
            {
                return NotFound("Original book not found");
            }

            var alternativeBook = await _context.Books.FindAsync(alternative.AlternativeBookId);
            if (alternativeBook == null)
            {
                return NotFound("Alternative book not found");
            }

            var existingAlternative = await _context.BookAlternatives
                .FirstOrDefaultAsync(ba => ba.AlternativeBookId == alternative.AlternativeBookId && ba.OriginalBookId == originalBookId);

            if (existingAlternative != null)
            {
                return Conflict("An alternative with the same ID already exists");
            }

            alternative.Id = Guid.NewGuid();
            alternative.OriginalBookId = originalBookId;

            _context.BookAlternatives.Add(alternative);
            await _context.SaveChangesAsync();

            var alternativeDto = _mapper.Map<BookAlternativeDto>(alternative);
            return Ok(alternativeDto);
        }

        [HttpGet("{originalBookId}/alternatives/{alternativeId}")]
        public async Task<ActionResult<BookAlternativeDto>> GetBookAlternatives(Guid originalBookId, Guid alternativeId)
        {
            var alternative = await _context.BookAlternatives
                .Include(ba => ba.OriginalBook)
                .Include(ba => ba.AlternativeBook)
                .FirstOrDefaultAsync(ba => ba.AlternativeBookId == alternativeId && ba.OriginalBookId == originalBookId);

            if (alternative == null)
            {
                return NotFound();
            }

            var alternativeDto = _mapper.Map<BookAlternativeDto>(alternative);
            return Ok(alternativeDto);
        }
    }
}

