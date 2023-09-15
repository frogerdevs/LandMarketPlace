namespace Web.Gateway.Dto.Request.Profiles
{
    public class EditProfileMerchantRequest
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? SellerCategoryId { get; set; }
        public string? BrandName { get; set; }
        public string? ImageUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
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
    }
}
