using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentValidationAPI.Api.Contracts.Requests
{
    public class UploadDocumentRequest
    {
        [Required]
        [JsonPropertyName("company_id")]
        public Guid CompanyId { get; set; }

        [Required]
        [JsonPropertyName("entity")]
        public Entity Entity { get; set; } = null!;

        [Required]
        [JsonPropertyName("document")]
        public DocumentMetaData DocumentMetaData { get; set; } = null!;

        [Required]
        [JsonPropertyName("validation_flow")]
        public ValidationFlow ValidationFlow { get; set; } = null!;
    }

    public class ValidationFlow
    {
        [Required]
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("steps")]
        public List<Steps>? Steps { get; set; }

    }

    public class Steps
    {
        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("approver_user_id")]
        public Guid ApproverUserId { get; set; }
    }

    public class DocumentMetaData
    {
        [Required]
        [MaxLength(255)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [Required]
        [JsonPropertyName("mime_type")]
        [MaxLength(100)]
        public string MimeType { get; set; } = null!;

        [Required]
        [Range(1, long.MaxValue)]
        [JsonPropertyName("size_bytes")]
        public long SizeBytes { get; set; } = 0;

        [Required]
        [MaxLength(1000)]
        [JsonPropertyName("bucket_key")]
        public string BucketKey { get; set; } = null!;
    }

    public class Entity
    {
        [Required]
        [MaxLength(100)]
        [JsonPropertyName("entity_type")]
        public string EntityType { get; set; } = null!;

        [Required]
        [JsonPropertyName("entity_id")]
        public Guid EntityId { get; set; }
    }
}
