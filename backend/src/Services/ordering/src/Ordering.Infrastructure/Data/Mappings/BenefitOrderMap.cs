namespace Ordering.Infrastructure.Data.Mappings
{
    public class BenefitOrderMap : IEntityTypeConfiguration<BenefitOrder>
    {
        public void Configure(EntityTypeBuilder<BenefitOrder> builder)
        {
            builder.HasKey(e => new { e.Id });
            builder.ToTable("BenefitOrder");
        }
    }
}
