namespace contrarian_reads_backend.Models
{
    public class View
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Guid ViewedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }

}
