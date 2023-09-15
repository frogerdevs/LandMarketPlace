using System.Text.Json.Serialization;
using Web.Gateway.Dto.Response.Base;

namespace Web.Gateway.Dto.Response.Testimoni
{
    public class HomeTestimoniesResponse : BaseWithDataCountResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<HomeTestimoniItem>? Data { get; set; }
    }
}
