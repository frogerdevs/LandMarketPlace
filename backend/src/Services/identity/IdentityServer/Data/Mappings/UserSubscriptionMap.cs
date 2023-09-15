namespace IdentityServer.Data.Mappings
{
    public class UserSubscriptionMap : IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder)
        {
            builder.ToTable("UserSubscription", "identity").HasKey(c => new { c.UserId, c.SubscriptionId });

            builder.HasOne(us => us.AppUser)
                .WithMany(b => b.UserSubscriptions)
                .HasForeignKey(ba => ba.UserId);

        }
    }
}
