namespace Web.Gateway.Dto.Response.Product
{
    public class ProductsByCategoryItemResponse
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? BrandSlug { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategorySlug { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
        public string? UrlSlug { get; set; }
    }
}
