namespace IdentityServer.Dtos.Responses.Users
{
    public class MerchantByCategoryItem
    {
        public string? Id { get; set; }
        public string? CategoryId { get; set; }
        public string? BrandName { get; set; }
        public string? BrandSlug { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsSeller { get; set; }
    }
}
