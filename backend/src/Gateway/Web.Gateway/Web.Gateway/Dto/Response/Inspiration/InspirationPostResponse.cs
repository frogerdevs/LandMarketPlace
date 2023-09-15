using System.Text.Json.Serialization;
using Web.Gateway.Dto.Response.Base;

namespace Web.Gateway.Dto.Response.Inspiration
{
    public class InspirationPostResponse : BaseWithDataResponse
    {
        [JsonPropertyOrder(4)]
        public new InspirationItem? Data { get; set; }
    }
}