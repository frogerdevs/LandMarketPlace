using System.Text.Json.Serialization;

namespace Web.Gateway.Dto.Response.Exhibition
{
    public class HomeExhibitionItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("datePublish")]
        public string? DatePublish { get; set; }

    }
}
