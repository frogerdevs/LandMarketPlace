namespace Web.Gateway.Dto.Response.Product
{
    public class ProductsItem
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategorySlug { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
    }
}
