namespace DocumentValidationAPI.Domain.Entities
{
    public class BusinessEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string EntityType { get; set; } = null!;
        public string ExternalReference { get; set; } = null!;
        public Company Company { get; set; } = null!;
    }
}
