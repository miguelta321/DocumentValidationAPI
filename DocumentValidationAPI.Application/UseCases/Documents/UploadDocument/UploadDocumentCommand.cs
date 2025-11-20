namespace DocumentValidationAPI.Application.UseCases.Documents.UploadDocument
{
    public class UploadDocumentCommand
    {
        public Guid CompanyId { get; set; }
        public Entity Entity { get; set; } = null!;
        public DocumentMetaData DocumentMetaData { get; set; } = null!;
        public ValidationFlow ValidationFlow { get; set; } = null!;
    }

    public class ValidationFlow
    {
        public bool Enabled { get; set; }
        public List<Steps>? Steps { get; set; }

    }

    public class Steps
    {
        public int Order { get; set; }
        public Guid ApproverUserId { get; set; }
    }

    public class DocumentMetaData
    {
        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public long SizeBytes { get; set; } = 0;
        public string BucketKey { get; set; } = null!;
    }

    public class Entity
    {
        public string EntityType { get; set; } = null!;
        public Guid EntityId { get; set; }
    }
}
