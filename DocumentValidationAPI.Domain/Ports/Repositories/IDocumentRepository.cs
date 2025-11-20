namespace DocumentValidationAPI.Domain.Ports.Repositories
{
    using DocumentValidationAPI.Domain.Entities;

    public interface IDocumentRepository
    {
        Task AddAsync(Document document, CancellationToken cancellationToken = default);
        Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Document document, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid documentId, CancellationToken cancellationToken = default);
    }
}
