using DocumentValidationAPI.Domain.Entities;
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DocumentValidationAPI.Infrastructure.Repositories
{
    public class ValidationStepsRepository : IValidationStepsRepository
    {
        private readonly DocumentDbContext _context;

        public ValidationStepsRepository(DocumentDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<ValidationStep> steps, CancellationToken cancellationToken = default) 
        {
            await _context.ValidationSteps.AddRangeAsync(steps, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateRangeAsync(IEnumerable<ValidationStep> steps, CancellationToken cancellationToken = default) 
        {
            _context.ValidationSteps.UpdateRange(steps);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ValidationStep?> GetNextPendingStepAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            return await _context.ValidationSteps
                .Where(s => s.DocumentId == documentId && s.Status == "P")
                .OrderBy(s => s.Order)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> HasPendingStepsAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            return await _context.ValidationSteps
                .AnyAsync(s => s.DocumentId == documentId && s.Status == "P", cancellationToken);
        }

        public async Task UpdateAsync(ValidationStep step, CancellationToken cancellationToken = default)
        {
            _context.ValidationSteps.Update(step);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ValidationStep>> GetByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            return await _context.ValidationSteps
                .Where(s => s.DocumentId == documentId)
                .OrderBy(s => s.Order)
                .ToListAsync(cancellationToken);
        }
    }
}
