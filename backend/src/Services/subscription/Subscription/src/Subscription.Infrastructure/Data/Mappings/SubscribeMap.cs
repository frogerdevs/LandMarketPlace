namespace Subscription.Infrastructure.Data.Mappings
{
    public class SubscribeMap : IEntityTypeConfiguration<Subscribe>
    {
        public void Configure(EntityTypeBuilder<Subscribe> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("Subscribe");
        }
    }
}
