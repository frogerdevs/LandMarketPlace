namespace Web.Gateway.Dto.Response.Inspiration
{
    public class SaveInspirationItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("inspiration_id")]
        public int InspirationId { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("category_id")]
        public string? CategoryId { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("images")]
        public string? Images { get; set; }

        [JsonPropertyName("liked")]
        public int Liked { get; set; }

        [JsonPropertyName("views")]
        public int Views { get; set; }

        [JsonPropertyName("excerpt")]
        public string? Excerpt { get; set; }

        [JsonPropertyName("datePublish")]
        public string? DatePublish { get; set; }
    }
}
