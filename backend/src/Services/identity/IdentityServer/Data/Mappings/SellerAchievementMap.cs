namespace IdentityServer.Data.Mappings
{
    public class SellerAchievementMap : IEntityTypeConfiguration<SellerAchievement>
    {
        public void Configure(EntityTypeBuilder<SellerAchievement> builder)
        {
            builder.ToTable("SellerAchievement", "identity").HasKey(c => c.Id);
            builder.HasOne(e => e.User).WithMany(e => e.SellerAchievements)
                .HasForeignKey(c => c.UserId).IsRequired(true);
        }
    }
}
