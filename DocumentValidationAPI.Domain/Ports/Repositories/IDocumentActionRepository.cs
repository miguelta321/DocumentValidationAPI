using DocumentValidationAPI.Domain.Entities;

namespace DocumentValidationAPI.Domain.Ports.Repositories
{
    public interface IDocumentActionRepository
    {
        Task AddAsync(DocumentAction action, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<DocumentAction>> GetByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default);
    }
}
