using DocumentValidationAPI.Domain.Entities;

namespace DocumentValidationAPI.Domain.Ports.Repositories
{
    public interface IBusinessEntityRepository
    {
        Task<bool> ExistsAsync(
            Guid companyId,
            Guid businessEntityId,
            string entityType,
            CancellationToken cancellationToken = default);
    }
}
