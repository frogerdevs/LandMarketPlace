namespace Catalog.Application.Dtos.Response.HomeDeals
{
    public class DealsForHomeItem
    {
        public required string Id { get; set; }
        public required string ProductId { get; set; }
        public string? Title { get; set; }
        public required string ProductSlug { get; set; }
        public string? ImgUrl { get; set; }
        public bool Active { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
