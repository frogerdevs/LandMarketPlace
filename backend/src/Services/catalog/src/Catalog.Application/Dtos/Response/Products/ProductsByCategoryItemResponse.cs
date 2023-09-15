namespace Catalog.Application.Dtos.Response.Products
{
    public class ProductsByCategoryResponse : BaseResponse
    {
        public IEnumerable<ProductsByCategoryItemResponse>? Data { get; set; }
    }
    public class ProductsByCategoryItemResponse
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
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
    }
}
