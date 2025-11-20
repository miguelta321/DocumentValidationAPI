using DocumentValidationAPI.Application.Abstractions.UseCases;
using DocumentValidationAPI.Domain.Entities;
using DocumentValidationAPI.Domain.Enums;
using DocumentValidationAPI.Domain.Exceptions;
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Domain.ValueObjects;

namespace DocumentValidationAPI.Application.UseCases.Documents.RejectDocument
{
    public class RejectDocumentService : IRejectDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IValidationStepsRepository _validationStepsRepository;
        private readonly IDocumentActionRepository _documentActionRepository;

        public RejectDocumentService(
            IDocumentRepository documentRepository,
            IValidationStepsRepository validationStepsRepository,
            IDocumentActionRepository documentActionRepository)
        {
            _documentRepository = documentRepository;
            _validationStepsRepository = validationStepsRepository;
            _documentActionRepository = documentActionRepository;
        }

        public async Task<ChangeStateDocumentResult> HandleAsync(ChangeStateCommand command, CancellationToken cancellationToken = default)
        {
            var (steps, document) = await ValidateCommand(command, cancellationToken);

            var approverId = command.ApproverUserId;

            var approverStep = steps.SingleOrDefault(s => s.ApproverUserId == approverId);

            if (approverStep == null)
            {
                throw new ValidationException(
                    $"User {approverId} is not allowed to reject this document."
                );
            }

            foreach (var step in steps)
            {
                if (step.Status == ValidationStatus.Pending.Value)
                {
                    step.Status = ValidationStatus.Rejected.Value;
                    step.ApprovedAt = DateTime.UtcNow;
                    step.Reason = command.Reason;
                }
            }

            document.ValidationStatus = ValidationStatus.Rejected.Value;
            document.UpdatedAt = DateTime.UtcNow;

            var action = new DocumentAction
            {
                Id = Guid.NewGuid(),
                DocumentId = document.Id,
                ActorUserId = command.ApproverUserId,
                ActionType = DocumentActionTypes.Rejected,
                Reason = command.Reason,
                CreatedAt = DateTime.UtcNow
            };

            await _validationStepsRepository.UpdateRangeAsync(steps, cancellationToken);
            await _documentRepository.UpdateAsync(document, cancellationToken);
            await _documentActionRepository.AddAsync(action, cancellationToken);

            return new ChangeStateDocumentResult
            {
                DocumentId = document.Id,
                Message = "Document rejected successfully."
            };
        }

        private async Task<(IReadOnlyList<ValidationStep>, Document)> ValidateCommand(ChangeStateCommand command, CancellationToken cancellationToken = default)
        {
            var document = await _documentRepository.GetByIdAsync(command.DocumentId, cancellationToken);
            if (document == null)
                throw new ValidationException("Document not found.");

            if (document.ValidationStatus == null)
                throw new ValidationException("Validation flow is not enabled for this document.");

            if (document.ValidationStatus != ValidationStatus.Pending.Value)
                throw new ValidationException("Document is no longer in a pending state.");

            var steps = await _validationStepsRepository.GetByDocumentIdAsync(command.DocumentId, cancellationToken);
            if (steps == null || !steps.Any())
                throw new ValidationException("There are no validation steps configured for this document.");

            return (steps, document);
        }
    }
}
