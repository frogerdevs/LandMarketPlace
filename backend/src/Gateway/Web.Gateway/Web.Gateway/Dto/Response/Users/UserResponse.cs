using System.Text.Json.Serialization;
using Web.Gateway.Dto.Response.Base;

namespace Web.Gateway.Dto.Response.Users
{
    public class UserResponse : BasePagingResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<UserItemResponse>? Data { get; set; }

    }
}
