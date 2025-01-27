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
            // A Book is added by one User
            .HasOne(b => b.User)
            // A User can add many Books
            .WithMany(u => u.Books)
            .HasForeignKey(b => b.AddedBy)
            // If a User is deleted, the AddedBy field for their Books is set to null
            .OnDelete(DeleteBehavior.SetNull);

        // Suggestion relationships
        modelBuilder.Entity<Suggestion>()
            // A Suggestion references one Book, but a Book can exist independently of Suggestions
            .HasOne(s => s.Book)
            .WithMany()
            .HasForeignKey(s => s.BookId)
            // Deleting a Book will delete all associated Suggestions
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Suggestion>()
            // A Suggestion can reference another Book (SuggestedBook), but SuggestedBook can exist independently
            .HasOne(s => s.SuggestedBook)
            .WithMany()
            .HasForeignKey(s => s.SuggestedBookId)
            // Deleting a SuggestedBook does not delete the Suggestion
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Suggestion>()
            // A Suggestion is created by one User (SuggestedByUser)
            .HasOne(s => s.SuggestedByUser)
            // A User can create many Suggestions
            .WithMany(u => u.Suggestions)
            .HasForeignKey(s => s.SuggestedBy)
            // Prevent deletion of a User if they are referenced by a Suggestion
            .OnDelete(DeleteBehavior.Restrict);

        // Upvote relationships
        modelBuilder.Entity<Upvote>()
            // An Upvote belongs to one Suggestion, and a Suggestion can have many Upvotes
            .HasOne(u => u.Suggestion)
            .WithMany(s => s.Upvotes)
            .HasForeignKey(u => u.SuggestionId)
            // Deleting a Suggestion will delete its associated Upvotes
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Upvote>()
            // An Upvote belongs to one User, and a User can create many Upvotes
            .HasOne(u => u.User)
            .WithMany(u => u.Upvotes)
            .HasForeignKey(u => u.UpvotedBy)
            // Prevent deletion of a User if they are referenced by an Upvote
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Upvote>()
            // Ensures that a User can only upvote a Suggestion once
            .HasIndex(u => new { u.SuggestionId, u.UpvotedBy }).IsUnique();

        // Comment relationships
        modelBuilder.Entity<Comment>()
            // A Comment belongs to one Suggestion, and a Suggestion can have many Comments
            .HasOne(c => c.Suggestion)
            .WithMany(s => s.Comments)
            .HasForeignKey(c => c.SuggestionId)
            // Deleting a Suggestion will delete its associated Comments
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            // A Comment is created by one User, and a User can create many Comments
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.CommentedBy)
            // Prevent deletion of a User if they are referenced by a Comment
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            // A Comment can optionally reference another Comment as its parent, enabling nested comments
            .HasOne(c => c.Parent)
            // A Parent Comment can have many Replies
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentId)
            // Prevent deletion of a Parent Comment if it is referenced by child Comments
            .OnDelete(DeleteBehavior.Restrict);

        // Book-Tag many-to-many
        modelBuilder.Entity<BookTag>()
            // A BookTag links to one Book, and a Book can have many BookTags
            .HasOne(bt => bt.Book)
            .WithMany(b => b.BookTags)
            .HasForeignKey(bt => bt.BookId)
            // When a BookTag is deleted, prevent deletion of the Book
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookTag>()
            // A BookTag links to one Tag, and a Tag can have many BookTags
            .HasOne(bt => bt.Tag)
            .WithMany(t => t.BookTags)
            .HasForeignKey(bt => bt.TagId);

        // View relationships
        modelBuilder.Entity<View>()
            // A View is created by one User, and a User can create many Views
            .HasOne(v => v.User)
            // Views do not have a defined navigation property back to Users
            .WithMany()
            .HasForeignKey(v => v.ViewedBy)
            // When a View is deleted, prevent deletion of the EntityId and User
            .OnDelete(DeleteBehavior.Restrict);
    }
}