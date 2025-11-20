using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentValidationAPI.Api.Contracts.Requests
{
    public class UploadDocumentRequest
    {
        [Required(ErrorMessage = "The field company_id is required.")]
        [JsonPropertyName("company_id")]
        public Guid? CompanyId { get; set; }

        [Required(ErrorMessage = "The object entity is required.")]
        [JsonPropertyName("entity")]
        public Entity Entity { get; set; } = null!;

        [Required(ErrorMessage = "The object document is required.")]
        [JsonPropertyName("document")]
        public DocumentMetaData DocumentMetaData { get; set; } = null!;

        [Required(ErrorMessage = "The object validation_flow is required.")]
        [JsonPropertyName("validation_flow")]
        public ValidationFlow ValidationFlow { get; set; } = null!;
    }

    public class ValidationFlow
    {
        [Required(ErrorMessage = "The field enabled is required.")]
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

        [JsonPropertyName("steps")]
        public List<Steps>? Steps { get; set; }
    }

    public class Steps
    {
        [Required(ErrorMessage = "The field order is required.")]
        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [Required(ErrorMessage = "The field approver_user_id is required.")]
        [JsonPropertyName("approver_user_id")]
        public Guid? ApproverUserId { get; set; }
    }

    public class DocumentMetaData
    {
        [Required(ErrorMessage = "The field name is required.")]
        [MaxLength(255, ErrorMessage = "The field name must not exceed 255 characters.")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "The field mime_type is required.")]
        [MaxLength(100, ErrorMessage = "The field mime_type must not exceed 100 characters.")]
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = null!;

        [Required(ErrorMessage = "The field size_bytes is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "The field size_bytes must be greater than 0.")]
        [JsonPropertyName("size_bytes")]
        public long? SizeBytes { get; set; } = 0;

        [Required(ErrorMessage = "The field bucket_key is required.")]
        [MaxLength(1000, ErrorMessage = "The field bucket_key must not exceed 1000 characters.")]
        [JsonPropertyName("bucket_key")]
        public string BucketKey { get; set; } = null!;
    }

    public class Entity
    {
        [Required(ErrorMessage = "The field entity_type is required.")]
        [MaxLength(100, ErrorMessage = "The field entity_type must not exceed 100 characters.")]
        [JsonPropertyName("entity_type")]
        public string EntityType { get; set; } = null!;

        [Required(ErrorMessage = "The field entity_id is required.")]
        [JsonPropertyName("entity_id")]
        public Guid? EntityId { get; set; }
    }
}
