using DocumentValidationAPI.Domain.Entities;
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DocumentValidationAPI.Infrastructure.Repositories
{
    public class BusinessEntityRepository : IBusinessEntityRepository
    {
        private readonly DocumentDbContext _context;

        public BusinessEntityRepository(DocumentDbContext context)
        {
            _context = context;
        }

        public Task<bool> ExistsAsync(
            Guid companyId,
            Guid businessEntityId,
            string entityType,
            CancellationToken cancellationToken = default)
        {
            return _context.BusinessEntities.AnyAsync(
                be =>
                    be.Id == businessEntityId &&
                    be.CompanyId == companyId &&
                    be.EntityType == entityType,
                cancellationToken);
        }
    }
}
