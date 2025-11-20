using DocumentValidationAPI.Application.UseCases.Documents;
using DocumentValidationAPI.Application.UseCases.Documents.ApproveDocument;

namespace DocumentValidationAPI.Application.Abstractions.UseCases
{
    public interface IApproveDocumentService
    {
        Task<ChangeStateDocumentResult> HandleAsync(ChangeStateCommand command, CancellationToken cancellationToken = default);
    }
}
