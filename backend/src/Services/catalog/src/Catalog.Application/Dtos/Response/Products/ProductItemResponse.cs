namespace Catalog.Application.Dtos.Response.Products
{
    public class ProductItemResponse
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
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
        public bool Active { get; set; }
        public ICollection<ProductImageItemResponse>? ProductImages { get; set; }
        public ICollection<ProductFacilityResponse>? ProductFacilities { get; set; }
        public ICollection<ProductNearResponse>? ProductNears { get; set; }
        public ICollection<ProductSpecificationResponse>? ProductSpecifications { get; set; }
    }
    public class ProductImageItemResponse
    {
        public required string ProductId { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageType { get; set; }
        public string? ImageName { get; set; }

    }
    public class ProductFacilityResponse
    {
        public required string ProductId { get; set; }
        public required string FacilityId { get; set; }
        public string? FacilityName { get; set; }

    }
    public class ProductNearResponse
    {
        public required string ProductId { get; set; }
        public string? Title { get; set; }
        public virtual ICollection<ProductNearItemResponse>? ProductNearItems { get; set; }
    }
    public class ProductNearItemResponse
    {
        public required string ProductId { get; set; }
        public required string ProductNearId { get; set; }
        public string? Title { get; set; }

    }
    public class ProductSpecificationResponse
    {
        public required string ProductId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
