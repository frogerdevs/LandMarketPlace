namespace Subscription.Infrastructure.Data.Mappings
{
    public class SubscribeDetailMap : IEntityTypeConfiguration<SubscribeDetail>
    {
        public void Configure(EntityTypeBuilder<SubscribeDetail> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("SubscribeDetail");
            builder.HasOne(c => c.Subscribe).WithMany(c => c.SubscribeDetails).HasForeignKey(c => c.SubscribeId);
        }
    }
}
