using contrarian_reads_backend.Data;
using contrarian_reads_backend.DTOs;
using contrarian_reads_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Controllers
{
    // /api/books
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/books/{originalBookId}
        [HttpGet("{originalBookId}")]
        public async Task<ActionResult<Book>> GetBook(Guid originalBookId)
        {
            var book = await _context.Books.FindAsync(originalBookId);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
        {
            book.Id = Guid.NewGuid();

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { originalBookId = book.Id }, book);
        }

        // GET: api/books/{originalBookId}/alternatives
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

            var responses = alternatives.Select(alternative => new BookAlternativeDto
            {
                Id = alternative.Id,
                OriginalBook = new BookDto
                {
                    Id = alternative.OriginalBook.Id,
                    Title = alternative.OriginalBook.Title,
                    Author = alternative.OriginalBook.Author,
                },
                AlternativeBook = new BookDto
                {
                    Id = alternative.AlternativeBook.Id,
                    Title = alternative.AlternativeBook.Title,
                    Author = alternative.AlternativeBook.Author,
                },
            });

            return Ok(responses);
        }

        // POST: api/books/{originalBookId}/alternatives
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

            var response = new BookAlternativeDto
            {
                Id = alternative.Id,
                OriginalBook = new BookDto
                {
                    Id = originalBook.Id,
                    Title = originalBook.Title,
                    Author = originalBook.Author,
                },
                AlternativeBook = new BookDto
                {
                    Id = alternativeBook.Id,
                    Title = alternativeBook.Title,
                    Author = alternativeBook.Author,
                },
            };

            return Ok(response);
        }

        // GET: api/books/{originalBookId}/alternatives/{alternativeId}
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

            return new BookAlternativeDto
            {
                Id = alternative.Id,
                OriginalBook = new BookDto
                {
                    Id = alternative.OriginalBook.Id,
                    Title = alternative.OriginalBook.Title,
                    Author = alternative.OriginalBook.Author,
                },
                AlternativeBook = new BookDto
                {
                    Id = alternative.AlternativeBook.Id,
                    Title = alternative.AlternativeBook.Title,
                    Author = alternative.AlternativeBook.Author,
                },
            };
        }
    }
}

