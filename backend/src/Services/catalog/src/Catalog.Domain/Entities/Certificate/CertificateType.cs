namespace Catalog.Domain.Entities.Certificate
{
    public class CertificateType : BaseEntity<string>
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public CertificateType()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
