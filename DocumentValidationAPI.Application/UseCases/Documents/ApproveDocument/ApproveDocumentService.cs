using DocumentValidationAPI.Application.Abstractions.UseCases;
using DocumentValidationAPI.Domain.Entities;
using DocumentValidationAPI.Domain.Enums;
using DocumentValidationAPI.Domain.Exceptions;
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Domain.ValueObjects;

namespace DocumentValidationAPI.Application.UseCases.Documents.ApproveDocument
{
    public class ApproveDocumentService : IApproveDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IValidationStepsRepository _validationStepsRepository;
        private readonly IDocumentActionRepository _documentActionRepository;

        public ApproveDocumentService(
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

            var orderedSteps = steps.OrderBy(s => s.Order).ToList();

            var approverId = command.ApproverUserId;

            var approverStep = orderedSteps.SingleOrDefault(s => s.ApproverUserId == approverId) ?? throw new ValidationException(
                    $"User {approverId} is not allowed to approve this document.");

            var maxOrderToApprove = approverStep.Order;
            var stepsToApprove = orderedSteps
                .Where(s =>
                    s.Status == ValidationStatus.Pending.Value &&
                    s.Order <= maxOrderToApprove)
                .ToList();

            if (stepsToApprove.Count == 0)
            {
                throw new ValidationException("There are no pending validation steps for this approver.");
            }

            foreach (var step in stepsToApprove)
            {
                step.Status = ValidationStatus.Approved.Value;
                step.Reason = command.Reason;
                step.ApprovedAt = DateTime.UtcNow;
            }

            var hasPending = orderedSteps.Any(s => s.Status == ValidationStatus.Pending.Value);

            var result = new ChangeStateDocumentResult
            {
                DocumentId = command.DocumentId
            };

            if (!hasPending)
            {
                document.ValidationStatus = ValidationStatus.Approved.Value;
                document.UpdatedAt = DateTime.UtcNow;
                result.Message = "Document approved successfully.";
            }
            else
            {
                result.Message = "Validation steps approved successfully.";
            }

            var action = new DocumentAction
            {
                Id = Guid.NewGuid(),
                DocumentId = document.Id,
                ActorUserId = command.ApproverUserId,
                ActionType = DocumentActionTypes.Approved,
                Reason = command.Reason,
                CreatedAt = DateTime.UtcNow
            };

            await _validationStepsRepository.UpdateRangeAsync(orderedSteps, cancellationToken);
            await _documentRepository.UpdateAsync(document, cancellationToken);
            await _documentActionRepository.AddAsync(action, cancellationToken);

            return result;
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
