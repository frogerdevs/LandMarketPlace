namespace Web.Gateway.Dto.Request.About
{
    public class EditAboutRequest
    {
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
        public ICollection<SellerDetailItem>? SellerAchievements { get; set; }
        public ICollection<SellerEventRequest>? SellerEvents { get; set; }
    }
    public class SellerDetailItem
    {
        public string? Title { get; set; }
        public string? Images { get; set; }
    }
    public class SellerEventRequest
    {
        public string? Title { get; set; }
        public ICollection<SellerEventImageRequest>? Images { get; set; }
    }
    public class SellerEventImageRequest
    {
        public string? Image { get; set; }
    }

}
