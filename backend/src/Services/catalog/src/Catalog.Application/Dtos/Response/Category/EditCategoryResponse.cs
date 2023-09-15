using Catalog.Application.Dtos.Response.Base;

namespace Catalog.Application.Dtos.Response.Category
{
    public class EditCategoryResponse : BaseResponse
    {
        [JsonPropertyOrder(4)]
        public object? Data { get; set; }
    }
}
