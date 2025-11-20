namespace DocumentValidationAPI.Application.UseCases.Documents
{
    public class ChangeStateDocumentResult
    {
        public Guid DocumentId { get; set; }
        public string Message { get; set; } = null!;
    }
}
