namespace IdentityServer.Data.Entites
{
    public class AppUser : IdentityUser
    {
        public bool IsSeller { get; set; }
        public string? SellerCategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsCompany { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? SubDistrict { get; set; }
        public string? PostCode { get; set; }
        public bool NewsLetter { get; set; }
        public bool WillingContacted { get; set; }
        public bool Active { get; set; }
        public bool Verified { get; set; }
        //public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        //public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        //public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        //public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public virtual ICollection<UserSubscription>? UserSubscriptions { get; set; }
        public virtual ICollection<SellerAchievement>? SellerAchievements { get; set; }
        public virtual ICollection<SellerEvent>? SellerEvents { get; set; }
        public UserProfile? SellerProfile { get; set; }
    }
}
