using DocumentValidationAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentValidationAPI.Infrastructure.Persistence.Configurations
{
    public class DocumentActionConfiguration : IEntityTypeConfiguration<DocumentAction>
    {
        public void Configure(EntityTypeBuilder<DocumentAction> builder)
        {
            builder.ToTable("DocumentActions");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.ActorUserId)
                   .IsRequired();

            builder.Property(a => a.ActionType)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(a => a.Reason)
                   .HasMaxLength(500);

            builder.Property(a => a.CreatedAt)
                   .IsRequired();
        }
    }
}
