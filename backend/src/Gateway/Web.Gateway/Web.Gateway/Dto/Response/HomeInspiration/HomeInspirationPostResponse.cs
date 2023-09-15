using System.Text.Json.Serialization;
using Web.Gateway.Dto.Response.Base;

namespace Web.Gateway.Dto.Response.HomeInspiration
{
    public class HomeInspirationPostResponse : BaseResponse
    {
        [JsonPropertyOrder(4)]
        public HomeInspirationItem? Data { get; set; }
    }
}
