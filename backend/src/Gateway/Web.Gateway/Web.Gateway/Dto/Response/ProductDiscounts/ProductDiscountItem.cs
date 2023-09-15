namespace Web.Gateway.Dto.Response.ProductDiscounts
{
    public class ProductDiscountItem
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string ProductId { get; set; }
        public required string ProductTitle { get; set; }
        public string? DiscountName { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime DiscountStart { get; set; }
        public DateTime DiscountEnd { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
    }
}
