namespace Subscription.Infrastructure.Data.Mappings
{
    public class VoucherMap : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("Voucher");
        }
    }
}
