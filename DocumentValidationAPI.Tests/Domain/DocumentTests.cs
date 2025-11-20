using DocumentValidationAPI.Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace DocumentValidationAPI.Tests.Domain
{
    public class DocumentTests
    {
        [Fact]
        public void New_document_should_start_with_null_validation_status_when_flow_is_disabled()
        {
            var now = DateTime.UtcNow;
            var document = new Document
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                EntityType = "vehicle",
                BusinessEntityId = Guid.NewGuid(),
                Name = "soat.pdf",
                MimeType = "application/pdf",
                SizeBytes = 12345,
                BucketKey = "companies/.../soat.pdf",
                CreatedAt = now
            };

            document.ValidationStatus.Should().BeNull();
            document.CreatedAt.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        }
    }
}
