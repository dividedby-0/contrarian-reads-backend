namespace contrarian_reads_backend.Data;

using contrarian_reads_backend.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Suggestion> Suggestions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Upvote> Upvotes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BookTag> BookTags { get; set; }
    public DbSet<View> Views { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Book-User relationship
        modelBuilder.Entity<Book>()
            .HasOne(b => b.User)
            .WithMany(u => u.Books)
            .HasForeignKey(b => b.AddedBy);

        // Suggestion relationships
        modelBuilder.Entity<Suggestion>()
            .HasOne(s => s.Book)
            .WithMany()
            .HasForeignKey(s => s.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Suggestion>()
            .HasOne(s => s.SuggestedBook)
            .WithMany()
            .HasForeignKey(s => s.SuggestedBookId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Suggestion>()
            .HasOne(s => s.SuggestedByUser)
            .WithMany(u => u.Suggestions)
            .HasForeignKey(s => s.SuggestedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Upvote relationships
        modelBuilder.Entity<Upvote>()
            .HasOne(u => u.Suggestion)
            .WithMany(s => s.Upvotes)
            .HasForeignKey(u => u.SuggestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Upvote>()
            .HasOne(u => u.User)
            .WithMany(u => u.Upvotes)
            .HasForeignKey(u => u.UpvotedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Upvote>()
            .HasIndex(u => new { u.SuggestionId, u.UpvotedBy }).IsUnique();

        // Comment relationships
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Suggestion)
            .WithMany(s => s.Comments)
            .HasForeignKey(c => c.SuggestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.CommentedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Book-Tag many-to-many
        modelBuilder.Entity<BookTag>()
            .HasOne(bt => bt.Book)
            .WithMany(b => b.BookTags)
            .HasForeignKey(bt => bt.BookId);

        modelBuilder.Entity<BookTag>()
            .HasOne(bt => bt.Tag)
            .WithMany(t => t.BookTags)
            .HasForeignKey(bt => bt.TagId);

        // View relationships
        modelBuilder.Entity<View>()
            .HasOne(v => v.User)
            .WithMany()
            .HasForeignKey(v => v.ViewedBy);
    }
}
