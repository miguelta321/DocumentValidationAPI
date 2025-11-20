using DocumentValidationAPI.Domain.Entities;
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DocumentValidationAPI.Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentDbContext _context;

        public DocumentRepository(DocumentDbContext context) => _context = context;

        public async Task AddAsync(Document document, CancellationToken cancellationToken = default)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _context.Documents
                .Include(d => d.ValidationSteps)
                .Include(d => d.Actions)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Document document, CancellationToken cancellationToken = default)
        {
            _context.Documents.Update(document);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            return await _context.Documents
                .AnyAsync(c => c.Id == documentId, cancellationToken);
        }
    }
}
