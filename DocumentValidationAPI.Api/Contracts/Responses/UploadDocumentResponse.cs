namespace DocumentValidationAPI.Api.Contracts.Responses
{
    public class UploadDocumentResponse
    {
        public Guid DocumentId { get; set; }
        public string UploadUrl { get; set; } = null!;
        public string BucketKey { get; set; } = null!;
    }
}
