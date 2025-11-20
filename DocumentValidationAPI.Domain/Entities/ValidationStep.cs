namespace DocumentValidationAPI.Domain.Entities
{
    public class ValidationStep
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public Guid ApproverUserId { get; set; }
        public string Status { get; set; } = null!;
        public Guid DocumentId { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? Reason { get; set; }

        public virtual Document Document { get; set; } = null!;
    }
}
