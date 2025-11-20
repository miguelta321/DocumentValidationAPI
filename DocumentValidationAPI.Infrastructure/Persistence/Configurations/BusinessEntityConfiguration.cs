using DocumentValidationAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentValidationAPI.Infrastructure.Persistence.Configurations
{
    public class BusinessEntityConfiguration : IEntityTypeConfiguration<BusinessEntity>
    {
        public void Configure(EntityTypeBuilder<BusinessEntity> builder)
        {
            builder.ToTable("BusinessEntities");

            builder.HasKey(be => be.Id);

            builder.Property(be => be.EntityType)
                   .IsRequired()
                   .HasMaxLength(50); 

            builder.Property(be => be.ExternalReference)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(be => be.CompanyId)
                   .IsRequired();

            // Relación hacia Company (muchos → uno)
            builder.HasOne(be => be.Company)
                   .WithMany()
                   .HasForeignKey(be => be.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
