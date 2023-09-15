namespace Web.Gateway.Dto.Request.Adsense
{
    public class AdsenseRequest
    {
        public required string ProductId { get; set; }
        public required string UserId { get; set; }
        public string? ImageUrl { get; set; }
        public required string Title { get; set; }
        public string? Content { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime StartTo { get; set; }
        public bool Active { get; set; }
        public string Channel { get; set; } = "unknown";
    }
}
