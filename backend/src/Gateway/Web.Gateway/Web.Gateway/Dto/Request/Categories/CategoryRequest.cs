namespace Web.Gateway.Dto.Request.Categories
{
    public class CategoryRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }

    }
    public class CategoryPutRequest : CategoryRequest
    {
        public required string Id { get; set; }
    }
}
