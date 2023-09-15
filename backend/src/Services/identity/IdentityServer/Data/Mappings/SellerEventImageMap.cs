namespace IdentityServer.Data.Mappings
{
    public class SellerEventImageMap : IEntityTypeConfiguration<SellerEventImage>
    {
        public void Configure(EntityTypeBuilder<SellerEventImage> builder)
        {
            builder.ToTable("SellerEventImage", "identity").HasKey(c => c.Id);
            builder.HasOne(e => e.SellerEvent).WithMany(e => e.Images)
                .HasForeignKey(c => c.SellerEventId).IsRequired(false);
        }
    }
}
