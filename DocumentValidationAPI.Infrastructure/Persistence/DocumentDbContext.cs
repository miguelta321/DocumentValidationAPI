using DocumentValidationAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Document = DocumentValidationAPI.Domain.Entities.Document;

namespace DocumentValidationAPI.Infrastructure.Persistence
{
    public class DocumentDbContext : DbContext
    {
        public DocumentDbContext(DbContextOptions<DocumentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents => Set<Document>();
        public DbSet<ValidationStep> ValidationSteps => Set<ValidationStep>();
        public DbSet<DocumentAction> DocumentActions => Set<DocumentAction>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<BusinessEntity> BusinessEntities => Set<BusinessEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
