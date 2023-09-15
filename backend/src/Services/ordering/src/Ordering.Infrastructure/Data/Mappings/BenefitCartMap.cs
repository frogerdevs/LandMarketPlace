namespace Ordering.Infrastructure.Data.Mappings
{
    public class BenefitCartMap : IEntityTypeConfiguration<BenefitCart>
    {
        public void Configure(EntityTypeBuilder<BenefitCart> builder)
        {
            builder.HasKey(e => new { e.Id });
            builder.ToTable("BenefitCart");

        }
    }
}
