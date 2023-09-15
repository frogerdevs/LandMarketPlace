namespace Subscription.Infrastructure.Data.Mappings
{
    public class UserVoucherMap : IEntityTypeConfiguration<UserVoucher>
    {
        public void Configure(EntityTypeBuilder<UserVoucher> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("UserVoucher");
            builder.HasOne(c => c.Voucher).WithMany(c => c.UserVouchers).HasForeignKey(c => c.VoucherId).IsRequired(false);
        }
    }
}
