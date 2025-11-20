namespace DocumentValidationAPI.Domain.Ports.Services
{
    public interface IStorageService
    {
        string GenerateUploadUrl(string bucketKey, string mimeType, long sizeBytes);
        string GenerateDownloadUrl(string bucketKey);
    }
}
