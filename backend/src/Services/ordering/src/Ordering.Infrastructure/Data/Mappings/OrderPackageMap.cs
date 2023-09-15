namespace Ordering.Infrastructure.Data.Mappings
{
    public class OrderPackageMap : IEntityTypeConfiguration<OrderPackage>
    {
        public void Configure(EntityTypeBuilder<OrderPackage> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("OrderPackage");
            builder.HasOne(c => c.BenefitOrder).WithMany(c => c.OrderPackages).HasForeignKey(c => c.BenefitOrderId).IsRequired(false);
        }
    }
}
