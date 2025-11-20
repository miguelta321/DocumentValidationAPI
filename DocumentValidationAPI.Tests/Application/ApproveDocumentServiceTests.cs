using DocumentValidationAPI.Application.UseCases.Documents.ApproveDocument;
using DocumentValidationAPI.Domain.Entities;
using DocumentValidationAPI.Domain.Enums; // donde tienes ValidationStatus si lo pusiste así
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DocumentValidationAPI.Tests.Application
{
    public class ApproveDocumentServiceTests
    {
        [Fact]
        public async Task Approving_with_highest_order_should_approve_all_pending_steps_and_document()
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var companyId = Guid.NewGuid();

            var document = new Document
            {
                Id = documentId,
                CompanyId = companyId,
                EntityType = "vehicle",
                BusinessEntityId = Guid.NewGuid(),
                Name = "soat.pdf",
                MimeType = "application/pdf",
                SizeBytes = 123,
                BucketKey = "companies/.../soat.pdf",
                ValidationStatus = ValidationStatus.Pending.Value,
                CreatedAt = DateTime.UtcNow
            };

            var steps = new List<ValidationStep>
            {
                new() { Id = Guid.NewGuid(), DocumentId = documentId, Order = 1, ApproverUserId = Guid.NewGuid(), Status = ValidationStatus.Pending.Value },
                new() { Id = Guid.NewGuid(), DocumentId = documentId, Order = 2, ApproverUserId = Guid.NewGuid(), Status = ValidationStatus.Pending.Value },
                new() { Id = Guid.NewGuid(), DocumentId = documentId, Order = 3, ApproverUserId = Guid.NewGuid(), Status = ValidationStatus.Pending.Value }
            };

            var highestOrderApproverId = steps.MaxBy(s => s.Order)!.ApproverUserId;

            var documentRepoMock = new Mock<IDocumentRepository>();
            var stepsRepoMock = new Mock<IValidationStepsRepository>();
            var actionsRepoMock = new Mock<IDocumentActionRepository>(); // si lo usas en el servicio

            documentRepoMock
                .Setup(r => r.GetByIdAsync(documentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(document);

            stepsRepoMock
                .Setup(r => r.GetByDocumentIdAsync(documentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(steps);

            var service = new ApproveDocumentService(
                documentRepoMock.Object,
                stepsRepoMock.Object,
                actionsRepoMock.Object
            );

            var command = new DocumentValidationAPI.Application.UseCases.Documents.ChangeStateCommand
            {
                DocumentId = documentId,
                ApproverUserId = highestOrderApproverId,
                Reason = "All good"
            };

            // Act
            var result = await service.HandleAsync(command, CancellationToken.None);

            // Assert
            result.Message.Should().Be("Document approved successfully.");

            steps.All(s => s.Status == ValidationStatus.Approved.Value).Should().BeTrue();
            document.ValidationStatus.Should().Be(ValidationStatus.Approved.Value);

            stepsRepoMock.Verify(r => r.UpdateRangeAsync(
                It.Is<IEnumerable<ValidationStep>>(ss => ss.All(s => s.Status == ValidationStatus.Approved.Value)),
                It.IsAny<CancellationToken>()),
                Times.Once);

            documentRepoMock.Verify(r => r.UpdateAsync(
                It.Is<Document>(d => d.ValidationStatus == ValidationStatus.Approved.Value),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
