namespace Web.Gateway.Dto.Request.Products
{
    public class ProductRequest
    {
        public required string UserId { get; set; }
        public required string CategoryId { get; set; }
        public required string SubCategoryId { get; set; }
        public required string Title { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? SubDistrict { get; set; }
        public string? PostCode { get; set; }
        public string? Address { get; set; }
        public string? CertificateId { get; set; }
        public DateTime? RegisteredSince { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public string? Description { get; set; }
        public string? Details { get; set; }
        public string? LocationLongitude { get; set; }
        public string? LocationLatitude { get; set; }
        //public bool Active { get; set; }
        public ICollection<ProductImageRequest>? ProductImages { get; set; }
        public ICollection<ProductFacilityRequest>? ProductFacilities { get; set; }
        public ICollection<ProductNearRequest>? ProductNears { get; set; }
        public ICollection<ProductSpecificationRequest>? ProductSpecifications { get; set; }
    }
}
