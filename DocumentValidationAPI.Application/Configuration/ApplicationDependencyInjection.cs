using DocumentValidationAPI.Application.Abstractions.UseCases;
using DocumentValidationAPI.Application.UseCases.Documents.ApproveDocument;
using DocumentValidationAPI.Application.UseCases.Documents.GetDownloadUrl;
using DocumentValidationAPI.Application.UseCases.Documents.RejectDocument;
using DocumentValidationAPI.Application.UseCases.Documents.UploadDocument;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentValidationAPI.Application.Configuration
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUploadDocumentService, UploadDocumentService>();
            services.AddScoped<IGetDownloadUrlService, GetDownloadUrlService>();
            services.AddScoped<IApproveDocumentService, ApproveDocumentService>();
            services.AddScoped<IRejectDocumentService, RejectDocumentService>();

            return services;
        }
    }
}
