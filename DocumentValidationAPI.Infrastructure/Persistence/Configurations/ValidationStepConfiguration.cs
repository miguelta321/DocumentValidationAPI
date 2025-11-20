using DocumentValidationAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentValidationAPI.Infrastructure.Persistence.Configurations
{
    public class ValidationStepConfiguration : IEntityTypeConfiguration<ValidationStep>
    {
        public void Configure(EntityTypeBuilder<ValidationStep> builder)
        {
            builder.ToTable("ValidationSteps");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Order)
                   .IsRequired();

            builder.Property(s => s.ApproverUserId)
                   .IsRequired();

            builder.Property(s => s.Status)
                   .IsRequired()
                   .HasMaxLength(1);

            builder.Property(s => s.DocumentId)
                   .IsRequired();

            builder.Property(s => s.Reason)
                   .HasMaxLength(500);
        }
    }
}
