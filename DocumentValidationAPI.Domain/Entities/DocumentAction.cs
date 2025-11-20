namespace DocumentValidationAPI.Domain.Entities
{
    public class DocumentAction
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }

        public Guid ActorUserId { get; set; }
        public string ActionType { get; set; } = null!;
        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        public Document Document { get; set; } = null!;
    }
}
