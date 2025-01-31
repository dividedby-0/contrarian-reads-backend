﻿using AutoMapper;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.Models;
using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.BookTags)
                .ThenInclude(bt => bt.Tag)
                .Include(b => b.User)
                .ToListAsync();

            var bookDTOs = _mapper.Map<List<BookDTO>>(books);
            return Ok(bookDTOs);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var book = await _context.Books
                .Include(b => b.BookTags)
                .ThenInclude(bt => bt.Tag)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == guidId);

            if (book == null)
            {
                return NotFound();
            }

            var bookDTO = _mapper.Map<BookDTO>(book);
            return Ok(bookDTO);
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook(CreateBookDTO createBookDTO)
        {
            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b =>
                    b.Title.ToLower() == createBookDTO.Title.ToLower() &&
                    b.Author.ToLower() == createBookDTO.Author.ToLower());

            if (existingBook != null)
            {
                var existingBookDTO = _mapper.Map<BookDTO>(existingBook);
                return Ok(existingBookDTO);
            }
            else
            {
                var book = _mapper.Map<Book>(createBookDTO);

                book.Id = Guid.NewGuid();
                book.AddedBy = Guid.Parse(createBookDTO.AddedBy);

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBook", new { id = book.Id }, book);
            }
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, BookDTO bookDTO)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var existingBook = await _context.Books.FindAsync(guidId);

            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = bookDTO.Title;
            existingBook.Author = bookDTO.Author;
            existingBook.Description = bookDTO.Description;
            existingBook.CoverImageUrl = bookDTO.CoverImageUrl;

            await _context.SaveChangesAsync();

            var updatedBookDTO = _mapper.Map<BookDTO>(existingBook);
            return Ok(updatedBookDTO);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var book = await _context.Books.FindAsync(guidId);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Books/search?searchTerm=&pageSize=10
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> SearchBooks(string? searchTerm, int pageSize = 20, string? lastEvaluatedKey = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                //TODO: reimplement pagination
                var bookIds = await _context.Books
                    .Select(b => b.Id)
                    .Union(_context.Suggestions
                        .Include(s => s.SuggestedBook)
                        .Select(s => s.BookId))
                    .Distinct()
                    .OrderBy(id => id)
                    //.Take(pageSize)
                    .ToListAsync();

                if (bookIds == null || !bookIds.Any())
                {
                    return NotFound("No books found matching the search term.");
                }

                var books = await _context.Books
                    .Where(b => bookIds.Contains(b.Id))
                    .Include(b => b.BookTags)
                        .ThenInclude(bt => bt.Tag)
                    .Include(b => b.User)
                    .ToListAsync();

                var suggestions = await _context.Suggestions
                    .Where(s => bookIds.Contains(s.BookId))
                    .Include(s => s.SuggestedBook)
                    .Include(s => s.SuggestedByUser)
                    .Include(s => s.Comments)
                        .ThenInclude(c => c.User)
                    .ToListAsync();

                var groupedSuggestions = suggestions
                    .GroupBy(s => s.BookId)
                    .ToDictionary(
                        g => g.Key,
                        g => _mapper.Map<List<SuggestionWithCommentsDTO>>(g.ToList())
                    );

                var booksWithSuggestions = books
                    .Select(b => new BookWithSuggestionsDTO
                    {
                        Book = _mapper.Map<BookDTO>(b),
                        Suggestions = groupedSuggestions.ContainsKey(b.Id) ? groupedSuggestions[b.Id] : new List<SuggestionWithCommentsDTO>()
                    })
                    .Where(bws => bws.Suggestions.Any())
                    .ToList();

                string? nextLastEvaluatedKey = null;
                if (bookIds.Count == pageSize)
                {
                    nextLastEvaluatedKey = bookIds.LastOrDefault().ToString();
                }

                return Ok(new
                {
                    BooksWithSuggestions = booksWithSuggestions,
                    NextLastEvaluatedKey = nextLastEvaluatedKey
                });
            }
            else
            {
                var bookIds = await _context.Books
                    .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
                    .Select(b => b.Id)
                    .Union(_context.Suggestions
                        .Include(s => s.SuggestedBook)
                        .Where(s => s.SuggestedBook.Title.Contains(searchTerm) || s.SuggestedBook.Author.Contains(searchTerm))
                        .Select(s => s.BookId))
                    .Distinct()
                    .OrderBy(id => id)
                    .ToListAsync();

                if (bookIds == null || !bookIds.Any())
                {
                    return NotFound("No books found matching the search term.");
                }

                var books = await _context.Books
                    .Where(b => bookIds.Contains(b.Id))
                    .Include(b => b.BookTags)
                        .ThenInclude(bt => bt.Tag)
                    .Include(b => b.User)
                    .ToListAsync();

                var suggestions = await _context.Suggestions
                    .Where(s => bookIds.Contains(s.BookId))
                    .Include(s => s.SuggestedBook)
                    .Include(s => s.SuggestedByUser)
                    .Include(s => s.Comments)
                        .ThenInclude(c => c.User)
                    .ToListAsync();

                var groupedSuggestions = suggestions
                    .GroupBy(s => s.BookId)
                    .ToDictionary(
                        g => g.Key,
                        g => _mapper.Map<List<SuggestionWithCommentsDTO>>(g.ToList())
                    );

                var booksWithSuggestions = books
                    .Select(b => new BookWithSuggestionsDTO
                    {
                        Book = _mapper.Map<BookDTO>(b),
                        Suggestions = groupedSuggestions.ContainsKey(b.Id) ? groupedSuggestions[b.Id] : new List<SuggestionWithCommentsDTO>()
                    })
                    .Where(bws => bws.Suggestions.Any())
                    .ToList();

                string? nextLastEvaluatedKey = null;
                if (bookIds.Count == pageSize)
                {
                    nextLastEvaluatedKey = bookIds.LastOrDefault().ToString();
                }

                return Ok(new
                {
                    BooksWithSuggestions = booksWithSuggestions,
                    NextLastEvaluatedKey = nextLastEvaluatedKey
                });
            }
        }
    }
}