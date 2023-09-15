using System.Text.Json.Serialization;
using Web.Gateway.Dto.Response.Base;

namespace Web.Gateway.Dto.Response.Product
{
    public class ProductResponse : BasePagingResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<HomeProductItem>? Data { get; set; }
    }
}
