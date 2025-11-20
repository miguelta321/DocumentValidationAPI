namespace DocumentValidationAPI.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public long SizeBytes { get; set; }
        public string BucketKey { get; set; } = null!;
        public string? ValidationStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CompanyId { get; set; }
        public Guid BusinessEntityId { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ValidationStep> ValidationSteps { get; set; } = [];
        public ICollection<DocumentAction> Actions { get; set; } = [];
    }
}
