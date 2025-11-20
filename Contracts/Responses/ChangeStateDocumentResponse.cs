using System.Text.Json.Serialization;

namespace DocumentValidationAPI.Api.Contracts.Responses
{
    public class ChangeStateDocumentResponse
    {
        [JsonPropertyName("document_id")]
        public Guid DocumentId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;
    }
}
