using DocumentValidationAPI.Domain.Entities;

namespace DocumentValidationAPI.Domain.Ports.Repositories
{
    public interface IValidationStepsRepository
    {
        Task AddRangeAsync(IEnumerable<ValidationStep> steps, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<ValidationStep> steps, CancellationToken cancellationToken = default);
        Task<ValidationStep?> GetNextPendingStepAsync(Guid documentId, CancellationToken cancellationToken = default);
        Task<bool> HasPendingStepsAsync(Guid documentId, CancellationToken cancellationToken = default);
        Task UpdateAsync(ValidationStep step, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ValidationStep>> GetByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default);
    }
}
