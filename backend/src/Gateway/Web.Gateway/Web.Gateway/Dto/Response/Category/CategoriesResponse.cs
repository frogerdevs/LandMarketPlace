namespace Web.Gateway.Dto.Response.Category
{
    public class CategoriesResponse : BaseWithDataCountResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<CategoryItem>? Data { get; set; }
    }
}
