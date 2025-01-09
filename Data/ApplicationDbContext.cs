using contrarian_reads_backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<BookAlternative> BookAlternatives { get; set; } = null!;
    public DbSet<BookAlternativeUpvote> BookAlternativeUpvotes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Book>()
            .HasMany(b => b.BookAlternatives)
            .WithOne(ba => ba.OriginalBook)
            .HasForeignKey(ba => ba.OriginalBookId);

        builder.Entity<Book>()
            .HasMany(b => b.AlternativeToBooks)
            .WithOne(ba => ba.AlternativeBook)
            .HasForeignKey(ba => ba.AlternativeBookId);
    }
}

