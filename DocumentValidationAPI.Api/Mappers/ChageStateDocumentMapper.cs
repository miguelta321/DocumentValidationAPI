using DocumentValidationAPI.Api.Contracts.Requests;
using DocumentValidationAPI.Application.UseCases.Documents;

namespace DocumentValidationAPI.Api.Mappers
{
    public static class ChangeStateDocumentMapper
    {
        public static ChangeStateCommand ToChangeStateCommand(this ChangeStateDocumentRequest request, Guid documentId)
        {
            return new ChangeStateCommand
            {
                DocumentId = documentId,
                ApproverUserId = request.ApproverUserId!.Value,
                Reason = request.Reason
            };
        }
    }
}
