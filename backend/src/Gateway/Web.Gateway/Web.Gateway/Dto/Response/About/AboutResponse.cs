namespace Web.Gateway.Dto.Response.About
{
    public class AboutResponse
    {
        public required string UserId { get; set; }
        public string? Email { get; set; }
        public string? PriceFrom { get; set; }
        public string? PriceTo { get; set; }
        public string? Description { get; set; }
        public string? Vision { get; set; }
        public string? Mission { get; set; }
        public string? Contact { get; set; }
        public string? WhatsApp { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Tiktok { get; set; }
        public string? Website { get; set; }
        public ICollection<SellerAchievementResponse>? SellerAchievements { get; set; }
        public ICollection<SellerEventResponse>? SellerEvents { get; set; }
    }
    public class SellerAchievementResponse
    {
        public string? Title { get; set; }
        public string? Image { get; set; }
    }
    public class SellerEventResponse
    {
        public string? Title { get; set; }
        public ICollection<SellerEventImageResponse>? Images { get; set; }
    }
    public class SellerEventImageResponse
    {
        public string? Image { get; set; }
    }
}
