using System.Text.Json.Serialization;

namespace DocumentValidationAPI.Api.Contracts.Responses
{
    public class DownloadDocumentResponse
    {
        [JsonPropertyName("download_url")]
        public string DownloadUrl { get; set; } = null!;
    }
}
