namespace contrarian_reads_backend.Services.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public Guid SuggestionId { get; set; }
        public UserDTO User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // For handling replies
        public Guid? ParentId { get; set; }

        public IEnumerable<CommentDTO> Replies { get; set; }
    }
}