namespace Catalog.Domain.Entities.Facilities
{
    public class Facility : BaseEntity<string>
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<ProductFacility>? ProductFacilities { get; set; }
        public Facility()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
