using System.Text.Json.Serialization;
using Web.Gateway.Dto.Response.Base;

namespace Web.Gateway.Dto.Response.Exhibition
{
    public class HomeExhibitionResponse : BaseWithDataCountResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<HomeExhibitionItem>? Data { get; set; }
    }
}
