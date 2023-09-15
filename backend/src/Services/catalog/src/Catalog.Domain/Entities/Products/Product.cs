namespace Catalog.Domain.Entities.Products
{
    public class Product : BaseAuditableEntity<string>
    {
        public required string UserId { get; set; }
        public required string CategoryId { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public string? SubCategoryId { get; set; }
        public string? CertificateId { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? SubDistrict { get; set; }
        public string? PostCode { get; set; }
        public string? Address { get; set; }
        public DateTime? RegisteredSince { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public string? Description { get; set; }
        public string? Details { get; set; }
        public string? LocationLongitude { get; set; }
        public string? LocationLatitude { get; set; }
        public bool Active { get; set; }
        public string Channel { get; set; } = "unknown";
        public virtual Category? Category { get; set; }
        public virtual SubCategory? SubCategory { get; set; }
        public virtual CertificateType? CertificateType { get; set; }
        public virtual ICollection<ProductImage>? ProductImages { get; set; }
        public virtual IEnumerable<Adsense>? Adsenses { get; set; }
        public virtual ICollection<HomeDeal>? HomeDeals { get; set; }
        public virtual ICollection<ProductFacility>? ProductFacilities { get; set; }
        public virtual ICollection<ProductDiscount>? ProductDiscounts { get; set; }
        public virtual ICollection<ProductNear>? ProductNears { get; set; }
        public virtual ICollection<ProductSpecification>? ProductSpecifications { get; set; }

        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
