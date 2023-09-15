namespace IdentityServer.Data.Entites
{
    public class UserProfile : BaseEntity<string>
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
        public bool Verified { get; set; }
        public string? BrandName { get; set; }
        public string? SlugBrand { get; set; }
        public string? Contact { get; set; }
        public string? Vision { get; set; }
        public string? Mission { get; set; }
        public string? WhatsApp { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Tiktok { get; set; }
        public string? Website { get; set; }

        public AppUser User { get; set; }
    }
}
