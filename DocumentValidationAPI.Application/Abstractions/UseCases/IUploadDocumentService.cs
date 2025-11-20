using DocumentValidationAPI.Application.UseCases.Documents.UploadDocument;

namespace DocumentValidationAPI.Application.Abstractions.UseCases
{
    public interface IUploadDocumentService
    {
        Task<UploadDocumentResult> HandleAsync(UploadDocumentCommand command, CancellationToken cancellationToken = default);
    }
}
