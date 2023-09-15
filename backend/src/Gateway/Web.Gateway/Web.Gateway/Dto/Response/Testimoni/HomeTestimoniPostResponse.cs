using System.Text.Json.Serialization;
using Web.Gateway.Dto.Response.Base;

namespace Web.Gateway.Dto.Response.Testimoni
{
    public class HomeTestimoniPostResponse : BaseWithDataResponse
    {
        [JsonPropertyOrder(4)]
        public new HomeTestimoniItem? Data { get; set; }
    }
}
