using DocumentValidationAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentValidationAPI.Infrastructure.Persistence.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(d => d.MimeType)
                   .IsRequired()
                   .HasMaxLength(100);
            
            builder.Property(d => d.EntityType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.SizeBytes)
                   .IsRequired();

            builder.Property(d => d.BucketKey)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(d => d.ValidationStatus)
                   .HasMaxLength(1); // "P", "A", "R" o null

            builder.Property(d => d.CreatedAt)
                   .IsRequired();
            
            builder.Property(d => d.CompanyId)
                   .IsRequired();
            
            builder.Property(d => d.BusinessEntityId)
                   .IsRequired();

            builder.HasMany(d => d.ValidationSteps)
                   .WithOne(s => s.Document)
                   .HasForeignKey(s => s.DocumentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Actions)
                   .WithOne(a => a.Document)
                   .HasForeignKey(a => a.DocumentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
