using Catalog.Domain.Entities.Facilities;

namespace Catalog.Infrastructure.Data.Mappings
{
    public class FacilityMap : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("Facility");

        }

    }

}
