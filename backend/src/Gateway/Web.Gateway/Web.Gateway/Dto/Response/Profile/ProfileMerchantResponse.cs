namespace Web.Gateway.Dto.Response.Profile
{
    public class ProfileMerchantResponse : BaseResponse
    {
        [JsonPropertyOrder(4)]
        public ProfileMerchantItemResponse? Data { get; set; }
    }
    public class ProfileMerchantItemResponse
    {
        public bool IsSeller { get; set; }
        public bool IsCompany { get; set; }
        public string? CompanyName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool Active { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public bool NewsLetter { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? SubDistrict { get; set; }
        public string? PostCode { get; set; }
        public bool WillingContacted { get; set; }
    }
}
