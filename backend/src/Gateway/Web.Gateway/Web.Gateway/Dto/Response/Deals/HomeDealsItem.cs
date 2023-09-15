namespace Web.Gateway.Dto.Response.Deals
{
    public class HomeDealsItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("slug")]
        public string? Slug { get; set; }
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}
