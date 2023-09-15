namespace Web.Gateway.Dto.Request.ProductDiscount
{
    public class ProductDiscountRequest
    {
        public required string UserId { get; set; }
        public required string ProductId { get; set; }
        public string? DiscountName { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime DiscountStart { get; set; }
        public DateTime DiscountEnd { get; set; }
        public bool Active { get; set; }
    }
}
