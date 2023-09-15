namespace Subscription.Application.Dtos.Response.PackageDetails
{
    public class PackageItem
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int DiscountPercent { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }
        public string? ImageUrl { get; set; }

    }
}
