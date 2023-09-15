namespace Web.Gateway.Dto.Response.Profile
{
    public class ProfileByBrandSlugResponse
    {
        public required string UserId { get; set; }
        public string? ImageUrl { get; set; }
        public string? BrandName { get; set; }
        public string? BrandSlug { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? SubDistrict { get; set; }
        public string? PostCode { get; set; }
        public bool Verified { get; set; }

    }
}
