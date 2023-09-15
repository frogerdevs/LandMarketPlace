using System.Text.Json.Serialization;

namespace Web.Gateway.Dto.Response.Testimoni
{
    public class HomeTestimoniItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("imgUrl")]
        public string? ImgUrl { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("datePublish")]
        public string? DatePublish { get; set; }


    }
}
