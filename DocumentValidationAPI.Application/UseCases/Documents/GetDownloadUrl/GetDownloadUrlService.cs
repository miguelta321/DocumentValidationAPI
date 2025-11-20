using DocumentValidationAPI.Application.Abstractions.UseCases;
using DocumentValidationAPI.Domain.Entities;
using DocumentValidationAPI.Domain.Enums;
using DocumentValidationAPI.Domain.Exceptions;
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Domain.Ports.Services;

namespace DocumentValidationAPI.Application.UseCases.Documents.GetDownloadUrl
{
    public class GetDownloadUrlService : IGetDownloadUrlService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IStorageService _storageService;
        private readonly IDocumentActionRepository _documentActionRepository;

        public GetDownloadUrlService(
            IDocumentRepository documentRepository, 
            IStorageService storageService,
            IDocumentActionRepository documentActionRepository)
        {
            _documentRepository = documentRepository;
            _storageService = storageService;
            _documentActionRepository = documentActionRepository;
        }

        public async Task<GetDownloadUrlResult> HandleAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var document = await _documentRepository.GetByIdAsync(documentId, cancellationToken);
            if (document == null) throw new ValidationException($"Document with id '{documentId}' does not exist.");

            var downloadUrl = _storageService.GenerateDownloadUrl(document.BucketKey);

            var action = new DocumentAction
            {
                Id = Guid.NewGuid(),
                DocumentId = document.Id,
                ActorUserId = Guid.Empty,
                ActionType = DocumentActionTypes.Downloaded,
                Reason = null,
                CreatedAt = DateTime.UtcNow
            };
            await _documentActionRepository.AddAsync(action, cancellationToken);

            return new GetDownloadUrlResult { DownloadUrl = downloadUrl };
        }

    }
}
