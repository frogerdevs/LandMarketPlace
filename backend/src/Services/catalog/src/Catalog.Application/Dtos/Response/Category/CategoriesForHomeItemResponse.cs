namespace Catalog.Application.Dtos.Response.Category
{
    public class CategoriesForHomeItemResponse
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
    }
}
