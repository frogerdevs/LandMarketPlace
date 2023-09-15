﻿namespace Web.Gateway.Dto.Response.Inspiration
{
    public class InspirationByUserItemResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

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

        [JsonPropertyName("datePublish")]
        public string? DatePublish { get; set; }

        [JsonPropertyName("excerpt")]
        public string? Excerpt { get; set; }
    }
}
