namespace Web.Gateway.Dto.Request.SubCategories
{
    public class SubCategoryRequest
    {
        public required string CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class SubCategoryPutRequest : SubCategoryRequest
    {
        public required string Id { get; set; }
    }

}
