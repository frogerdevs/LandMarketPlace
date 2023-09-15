namespace Web.Gateway.Dto.Response.Deals
{
    public class InDealsItem
    {
        public string? Slug { get; set; }
        public string? Title { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? MerchantName { get; set; }
        public string? MerchantSubscription { get; set; }
        public string? ImageUrl { get; set; }
        public string? MerchantSlug { get; set; }
    }
}
