namespace Catalog.Domain.Entities.Products
{
    public class ProductFacility
    {
        public required string ProductId { get; set; }
        public required string FacilityId { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Facility? Facility { get; set; }

    }
}
