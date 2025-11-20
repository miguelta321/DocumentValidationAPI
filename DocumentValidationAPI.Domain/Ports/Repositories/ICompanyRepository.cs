namespace DocumentValidationAPI.Domain.Ports.Repositories
{
    public interface ICompanyRepository
    {
        Task<bool> ExistsAsync(Guid companyId, CancellationToken cancellationToken = default);
    }

}
