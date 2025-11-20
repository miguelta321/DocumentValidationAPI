using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DocumentValidationAPI.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DocumentDbContext _context;

        public CompanyRepository(DocumentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            return await _context.Companies
                .AnyAsync(c => c.Id == companyId, cancellationToken);
        }
    }
}
