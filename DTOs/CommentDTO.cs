namespace contrarian_reads_backend.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int SuggestionId { get; set; }
        public UserDTO User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // For handling replies
        public int? ParentId { get; set; }
        public IEnumerable<CommentDTO> Replies { get; set; }
    }
}
