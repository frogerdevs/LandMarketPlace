using System.Text.Json.Serialization;
using Web.Gateway.Dto.Request.Base;

namespace Web.Gateway.Dto.Request.Testimoni
{
    public class CustomerContactRequest : BaseRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
