namespace Web.Gateway.Dto.Response.ProductDiscounts
{
    public class ProductDiscountsOfTheWeekItem
    {
        public required string UserId { get; set; }
        public string? CategoryId { get; set; }
        public string? CategorySlug { get; set; }
        public string? DiscountId { get; set; }
        public string? DiscountName { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime DiscountStart { get; set; }
        public DateTime DiscountEnd { get; set; }
        public required string ProductId { get; set; }
        public required string ProductTitle { get; set; }
        public required string ProductSlug { get; set; }
        public bool ProductActive { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public string? ImageUrl { get; set; }
    }
}
