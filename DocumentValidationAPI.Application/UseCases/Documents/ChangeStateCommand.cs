namespace DocumentValidationAPI.Application.UseCases.Documents
{
    public class ChangeStateCommand
    {
        public Guid DocumentId { get; set; }
        public Guid ApproverUserId { get; set; }
        public string? Reason { get; set; }
    }
}
