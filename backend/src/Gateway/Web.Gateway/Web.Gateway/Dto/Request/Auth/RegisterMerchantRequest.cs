namespace Web.Gateway.Dto.Request.Auth
{
    public class RegisterMerchantRequest
    {
        public bool IsCompany { get; set; }
        public string CompanyName { get; set; }
        public string BrandName { get; set; }
        public string CategoryId { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string SubDistrict { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
