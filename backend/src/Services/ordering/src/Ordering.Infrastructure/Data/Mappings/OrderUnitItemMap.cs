namespace Ordering.Infrastructure.Data.Mappings
{
    public class OrderUnitItemMap : IEntityTypeConfiguration<OrderUnitItem>
    {
        public void Configure(EntityTypeBuilder<OrderUnitItem> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("OrderUnitItem");
            builder.HasOne(c => c.BenefitOrder).WithMany(c => c.OrderUnitItems).HasForeignKey(c => c.BenefitOrderId).IsRequired(false);
        }
    }
}
