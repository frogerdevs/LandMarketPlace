namespace Subscription.Infrastructure.Data.Mappings
{
    public class UnitTypeMap : IEntityTypeConfiguration<BenefitType>
    {
        public void Configure(EntityTypeBuilder<BenefitType> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("UnitType");
        }
    }
}
