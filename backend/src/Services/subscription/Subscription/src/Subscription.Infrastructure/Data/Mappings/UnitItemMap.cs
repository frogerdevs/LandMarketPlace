namespace Subscription.Infrastructure.Data.Mappings
{
    public class UnitItemMap : IEntityTypeConfiguration<UnitItem>
    {
        public void Configure(EntityTypeBuilder<UnitItem> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("UnitItem");
            //builder.HasOne(c => c.UnitType).WithMany(c => c.UnitItems).HasForeignKey(c => c.BenefitTypeId);
        }
    }
}
