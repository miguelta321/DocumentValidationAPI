using Amazon.S3;
using DocumentValidationAPI.Domain.Ports.Repositories;
using DocumentValidationAPI.Domain.Ports.Services;
using DocumentValidationAPI.Infrastructure.Persistence;
using DocumentValidationAPI.Infrastructure.Repositories;
using DocumentValidationAPI.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentValidationAPI.Infrastructure.Configuration
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var bucketName = configuration["Storage:BucketName"];
            var expirationMinutes = configuration["Storage:ExpiresMinutes"];

            services.AddDbContext<DocumentDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
            });

            // AWS S3 client
            services.AddSingleton<IAmazonS3, AmazonS3Client>();

            // Repos
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IValidationStepsRepository, ValidationStepsRepository>();
            services.AddScoped<IBusinessEntityRepository, BusinessEntityRepository>();
            services.AddScoped<IDocumentActionRepository, DocumentActionRepository>();

            // Storage
            services.AddScoped<IStorageService>(sp =>
            {
                var s3Client = sp.GetRequiredService<IAmazonS3>();
                return new S3StorageService(s3Client, bucketName!, expirationMinutes!);
            });

            return services;
        }
    }
}
