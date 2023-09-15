using System.Text.Json.Serialization;

namespace Web.Gateway.Dto.Response.HomeInspiration
{
    public class ViewHomeInspirationItem
    {
        [JsonPropertyName("inspiration_id")]
        public int InspirationId { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("liked")]
        public int Liked { get; set; }

        [JsonPropertyName("views")]
        public int Views { get; set; }

        [JsonPropertyName("datePublish")]
        public string? DatePublish { get; set; }
    }
}
