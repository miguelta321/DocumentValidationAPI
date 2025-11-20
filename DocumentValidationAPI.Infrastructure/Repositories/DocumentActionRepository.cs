using DocumentValidationAPI.Domain.Entities;
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DocumentValidationAPI.Infrastructure.Repositories
{
    public class DocumentActionRepository : IDocumentActionRepository
    {
        private readonly DocumentDbContext _context;

        public DocumentActionRepository(DocumentDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DocumentAction action, CancellationToken cancellationToken = default)
        {
            await _context.DocumentActions.AddAsync(action, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<DocumentAction>> GetByDocumentIdAsync(
            Guid documentId,
            CancellationToken cancellationToken = default)
        {
            return await _context.DocumentActions
                .Where(a => a.DocumentId == documentId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
