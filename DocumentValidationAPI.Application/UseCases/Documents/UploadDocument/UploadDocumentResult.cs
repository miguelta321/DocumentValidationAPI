namespace DocumentValidationAPI.Application.UseCases.Documents.UploadDocument
{
    public class UploadDocumentResult
    {
        public Guid DocumentId { get; init; }
        public string UploadUrl { get; init; } = null!;
        public string BucketKey { get; init; } = null!;
    }
}
