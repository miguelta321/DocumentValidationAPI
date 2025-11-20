using DocumentValidationAPI.Api.Contracts.Requests;
using DocumentValidationAPI.Application.UseCases.Documents.UploadDocument;

namespace DocumentValidationAPI.Api.Mappers
{
    public static class UploadDocumentMapper
    {
        public static UploadDocumentCommand ToUploadDocumentCommand(this UploadDocumentRequest request)
        {
            return new UploadDocumentCommand
            {
                CompanyId = request.CompanyId!.Value,
                Entity = new Application.UseCases.Documents.UploadDocument.Entity
                {
                    EntityType = request.Entity.EntityType,
                    EntityId = request.Entity.EntityId!.Value
                },
                DocumentMetaData = new Application.UseCases.Documents.UploadDocument.DocumentMetaData
                {
                    Name = request.DocumentMetaData.Name,
                    MimeType = request.DocumentMetaData.MimeType,
                    SizeBytes = request.DocumentMetaData.SizeBytes!.Value,
                    BucketKey = request.DocumentMetaData.BucketKey
                },
                ValidationFlow = new Application.UseCases.Documents.UploadDocument.ValidationFlow
                {
                    Enabled = request.ValidationFlow.Enabled!.Value,
                    Steps = request.ValidationFlow.Steps.ToCommandSteps()
                }
            };
        }
    }
}
