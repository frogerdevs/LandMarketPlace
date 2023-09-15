namespace Web.Gateway.Dto.Request.Inspiration
{
    public class SaveInspirationRequest
    {
        [JsonPropertyName("inspiration_id")]
        public int? InspirationId { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }
    public class SaveInspirationGetRequest
    {
        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }
}
