using DocumentValidationAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentValidationAPI.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // Relación Company → BusinessEntities (1:N)
            builder.HasMany<BusinessEntity>()
                   .WithOne(be => be.Company)
                   .HasForeignKey(be => be.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
