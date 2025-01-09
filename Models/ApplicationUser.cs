using Microsoft.AspNetCore.Identity;

namespace contrarian_reads_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<BookAlternative>? SuggestedAlternatives { get; set; }
        public ICollection<BookAlternativeUpvote>? Upvotes { get; set; }
    }
}