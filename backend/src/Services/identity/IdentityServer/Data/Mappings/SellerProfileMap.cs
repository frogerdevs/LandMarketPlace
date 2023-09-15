namespace IdentityServer.Data.Mappings
{
    public class SellerProfileMap : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("SellerProfile", "identity").HasKey(c => c.Id);

        }
    }
}
