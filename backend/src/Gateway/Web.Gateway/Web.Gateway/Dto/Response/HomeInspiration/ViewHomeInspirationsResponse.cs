using System.Text.Json.Serialization;
using Web.Gateway.Dto.Response.Base;

namespace Web.Gateway.Dto.Response.HomeInspiration
{
    public class ViewHomeInspirationsResponse : BaseWithDataCountResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<ViewHomeInspirationItem>? Data { get; set; }
    }
}
