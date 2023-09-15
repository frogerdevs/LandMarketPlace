namespace Catalog.Domain.Entities.Categories
{
    public class SubCategory : BaseEntity<string>
    {
        public required string CategoryId { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
        //public virtual Category? Category { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public SubCategory()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
