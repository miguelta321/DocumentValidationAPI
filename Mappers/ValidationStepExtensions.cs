

using DocumentValidationAPI.Application.UseCases.Documents.UploadDocument;

public static class ValidationStepExtensions
{
    public static List<Steps>? ToCommandSteps(this IEnumerable<DocumentValidationAPI.Api.Contracts.Requests.Steps>? steps)
    {
        if (steps == null || !steps.Any())
            return null;

        return [.. steps.Select(s => new Steps
        {
            Order = s.Order!.Value,
            ApproverUserId = s.ApproverUserId!.Value
        })];
    }
}
