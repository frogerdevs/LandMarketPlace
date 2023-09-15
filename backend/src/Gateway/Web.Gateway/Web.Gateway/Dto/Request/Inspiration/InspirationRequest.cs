namespace Web.Gateway.Dto.Request.Inspiration
{
    public class InspirationRequest : BaseRequest
    {
        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("images")]
        public IEnumerable<string>? Images { get; set; }

        [JsonPropertyName("category_id")]
        public string? CategoryId { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
}
