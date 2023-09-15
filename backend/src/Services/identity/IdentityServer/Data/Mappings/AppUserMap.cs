namespace IdentityServer.Data.Mappings
{
    public class AppUserMap : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {

            builder.ToTable("User", "identity");

            builder.Property(u => u.SellerCategoryId).IsRequired(false);
            builder.Property(u => u.Address).IsRequired(false);
            builder.Property(u => u.Country).IsRequired(false);
            builder.Property(u => u.Province).IsRequired(false);
            builder.Property(u => u.City).IsRequired(false);
            builder.Property(u => u.PostCode).IsRequired(false);
            builder.Property(u => u.FirstName).IsRequired(false);
            builder.Property(u => u.LastName).IsRequired(false);
            builder.Property(u => u.CompanyName).IsRequired(false);

            builder.HasOne(e => e.SellerProfile).WithOne(e => e.User)
                .HasForeignKey<UserProfile>().IsRequired(false);

            //builder.HasMany(e => e.Claims)
            //    .WithOne()
            //    .HasForeignKey(uc => uc.UserId)
            //    .IsRequired();

            //// Each User can have many UserLogins
            //builder.HasMany(e => e.Logins)
            //    .WithOne()
            //    .HasForeignKey(ul => ul.UserId)
            //    .IsRequired();

            //// Each User can have many UserTokens
            //builder.HasMany(e => e.Tokens)
            //    .WithOne()
            //    .HasForeignKey(ut => ut.UserId)
            //    .IsRequired();

            //// Each User can have many entries in the UserRole join table
            //builder.HasMany(e => e.UserRoles)
            //    .WithOne()
            //    .HasForeignKey(ur => ur.UserId)
            //    .IsRequired();
        }

    }
}
