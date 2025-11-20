using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentValidationAPI.Api.Contracts.Requests
{
    public class ChangeStateDocumentRequest
    {
        [Required(ErrorMessage = "The field actor_user_id is required")]
        [JsonPropertyName("actor_user_id")]
        public Guid? ApproverUserId { get; set; }

        [MaxLength(500, ErrorMessage = "The field reason must not exceed 500 characters.")]
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }
}
