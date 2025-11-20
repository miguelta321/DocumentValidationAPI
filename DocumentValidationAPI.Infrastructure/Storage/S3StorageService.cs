using Amazon.S3;
using Amazon.S3.Model;
using DocumentValidationAPI.Domain.Ports.Services;

namespace DocumentValidationAPI.Infrastructure.Storage
{
    public class S3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _expirationMinutes;

        public S3StorageService(IAmazonS3 s3Client, string bucketName, string expirationMinutes)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
            _expirationMinutes = expirationMinutes;
        }

        public string GenerateUploadUrl(string bucketKey, string mimeType, long sizeBytes)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = bucketKey,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(GetExpirationMinutes()),
                ContentType = mimeType
            };

            return _s3Client.GetPreSignedURL(request);
        }

        private int GetExpirationMinutes()
        {
            if (int.TryParse(_expirationMinutes, out var minutes))
                return minutes;

            return 5; 
        }


        public string GenerateDownloadUrl(string bucketKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = bucketKey,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(GetExpirationMinutes())
            };

            return _s3Client.GetPreSignedURL(request);
        }
    }
}
