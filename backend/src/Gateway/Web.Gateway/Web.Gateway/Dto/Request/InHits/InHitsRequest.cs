namespace Web.Gateway.Dto.Request.InHits
{
    public class InHitsRequest
    {
        [JsonPropertyName("inspiration_id")]
        public int InspirationId { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("images")]
        public List<string>? Images { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }
    public class InHitsPutRequest : InHitsRequest
    {
        public int Id { get; set; }
    }
}
