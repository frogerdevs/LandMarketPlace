namespace Ordering.Infrastructure.Data.Mappings
{
    public class OrderSubscriptionMap : IEntityTypeConfiguration<OrderSubscribe>
    {
        public void Configure(EntityTypeBuilder<OrderSubscribe> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("OrderSubscription");
            builder.HasOne(c => c.BenefitOrder).WithMany(c => c.OrderSubscriptions).HasForeignKey(c => c.BenefitOrderId).IsRequired(false);
        }
    }
}
