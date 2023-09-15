namespace Catalog.Domain.Entities.Categories
{
    public class Category : BaseEntity<string>
    {
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
        public virtual IEnumerable<Product>? Products { get; set; }
        //public virtual IEnumerable<SubCategory>? SubCategories { get; set; }
        public Category()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
