using DocumentValidationAPI.Application.Abstractions.UseCases;
using DocumentValidationAPI.Domain.Entities;
using DocumentValidationAPI.Domain.Enums;
using DocumentValidationAPI.Domain.Exceptions;
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Domain.Ports.Services;
using DocumentValidationAPI.Domain.ValueObjects;

namespace DocumentValidationAPI.Application.UseCases.Documents.UploadDocument
{
    public class UploadDocumentService : IUploadDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IValidationStepsRepository _validationStepsRepository;
        private readonly IStorageService _storageService;
        private readonly ICompanyRepository _companyRepository;
        private readonly IBusinessEntityRepository _businessEntityRepository;
        private readonly IDocumentActionRepository _documentActionRepository;

        public UploadDocumentService(
            IDocumentRepository documentRepository,
            IStorageService storageService,
            IValidationStepsRepository validationStepsRepository,
            ICompanyRepository companyRepository,
            IBusinessEntityRepository businessEntityRepository,
            IDocumentActionRepository documentActionRepository)
        {
            _documentRepository = documentRepository;
            _storageService = storageService;
            _validationStepsRepository = validationStepsRepository;
            _companyRepository = companyRepository;
            _businessEntityRepository = businessEntityRepository;
            _documentActionRepository = documentActionRepository;
        }

        public async Task<UploadDocumentResult> HandleAsync(UploadDocumentCommand command, CancellationToken cancellationToken = default)
        {
            await ValidateCommand(command, cancellationToken);

            var bucketKey = BuildBucketKey(command);
            var uploadUrl = _storageService.GenerateUploadUrl(bucketKey, command.DocumentMetaData.MimeType, command.DocumentMetaData.SizeBytes);
            var initialStatus = command.ValidationFlow.Enabled ? ValidationStatus.Pending : null;

            var document = CreateDocument(command, bucketKey, initialStatus);
            await _documentRepository.AddAsync(document, cancellationToken);

            if (command.ValidationFlow.Enabled)
            {
                var validationSteps = CreateValidationSteps(command, document.Id, initialStatus!.Value);
                await _validationStepsRepository.AddRangeAsync(validationSteps, cancellationToken);
            }

            var uploadAction = new DocumentAction
            {
                Id = Guid.NewGuid(),
                DocumentId = document.Id,
                ActorUserId = Guid.Empty,
                ActionType = DocumentActionTypes.Uploaded,
                Reason = null,
                CreatedAt = DateTime.UtcNow
            };
            await _documentActionRepository.AddAsync(uploadAction, cancellationToken);

            return new UploadDocumentResult
            {
                DocumentId = document.Id,
                UploadUrl = uploadUrl,
                BucketKey = bucketKey
            };
        }

        private async Task ValidateCommand(UploadDocumentCommand command, CancellationToken cancellationToken = default)
        {
            if (command.ValidationFlow.Enabled)
            {
                if (command.ValidationFlow.Steps == null || !command.ValidationFlow.Steps.Any())
                    throw new ValidationException("Validation flow is enabled but no steps were provided.");

                var orders = command.ValidationFlow.Steps.Select(s => s.Order).ToList();

                if (orders.Distinct().Count() != orders.Count)
                    throw new ValidationException("Validation steps must have unique order values.");

                if (orders.Min() <= 0)
                    throw new ValidationException("Validation steps order must be greater than zero.");
            }

            var companyExists = await _companyRepository.ExistsAsync(command.CompanyId, cancellationToken);
            if (!companyExists) throw new ValidationException($"Company with id '{command.CompanyId}' does not exist.");

            var businessEntityExists = await _businessEntityRepository.ExistsAsync(
                command.CompanyId,
                command.Entity.EntityId,
                command.Entity.EntityType,
                cancellationToken);

            if (!businessEntityExists)
                throw new ValidationException(
                    $"Business entity '{command.Entity.EntityId}' of type '{command.Entity.EntityType}' does not exist for company '{command.CompanyId}'.");
        }

        private List<ValidationStep> CreateValidationSteps(UploadDocumentCommand command, Guid documentId, string status)
        {
            var steps = new List<ValidationStep>();

            foreach (var step in command!.ValidationFlow!.Steps!)
            {
                steps.Add(new ValidationStep
                {
                    Id = Guid.NewGuid(),
                    DocumentId = documentId,
                    Order = step.Order,
                    ApproverUserId = step.ApproverUserId,
                    Status = status,
                });
            }
            
            return steps;
        }

        private Document CreateDocument(UploadDocumentCommand command, string bucketKey, ValidationStatus? validationStatus) => new()
        {
            Id = Guid.NewGuid(),
            Name = command.DocumentMetaData.Name,
            MimeType = command.DocumentMetaData.MimeType,
            SizeBytes = command.DocumentMetaData.SizeBytes,
            BucketKey = bucketKey,
            CompanyId = command.CompanyId,
            EntityType = command.Entity.EntityType,
            BusinessEntityId = command.Entity.EntityId,
            ValidationStatus = validationStatus?.Value,
            CreatedAt = DateTime.UtcNow
        };


        private static string BuildBucketKey(UploadDocumentCommand command)
        {
            return $"companies/{command.CompanyId}/{command.Entity.EntityType}/{command.Entity.EntityId}/{command.DocumentMetaData.Name}";
        }
    }
}
