﻿namespace contrarian_reads_backend.Models;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Guid? AddedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public string CoverImageUrl { get; set; }
    public ICollection<BookTag> BookTags { get; set; }

    public User User { get; set; } // Navigation property
}