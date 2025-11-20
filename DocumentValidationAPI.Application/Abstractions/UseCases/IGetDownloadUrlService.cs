using DocumentValidationAPI.Application.UseCases.Documents.GetDownloadUrl;

namespace DocumentValidationAPI.Application.Abstractions.UseCases
{
    public interface IGetDownloadUrlService
    {
        Task<GetDownloadUrlResult> HandleAsync(Guid documentId, CancellationToken cancellationToken = default);
    }
}
