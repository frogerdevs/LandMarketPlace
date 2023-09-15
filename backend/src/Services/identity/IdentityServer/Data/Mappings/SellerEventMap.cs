namespace IdentityServer.Data.Mappings
{
    public class SellerEventMap : IEntityTypeConfiguration<SellerEvent>
    {
        public void Configure(EntityTypeBuilder<SellerEvent> builder)
        {
            builder.ToTable("SellerEvent", "identity").HasKey(c => c.Id);
            builder.HasOne(e => e.User).WithMany(e => e.SellerEvents)
                .HasForeignKey(c => c.UserId).IsRequired(true);
        }
    }
}
