namespace Catalog.Application.Dtos.Response.HomeDeals
{
    public class HomeDealsListItem
    {
        public required string Id { get; set; }
        public required string ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
