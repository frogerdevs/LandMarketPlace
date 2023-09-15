namespace IdentityServer.Data.Entites
{
    public class SellerAchievement : BaseEntity<string>
    {
        public required string UserId { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public AppUser? User { get; set; }
        public SellerAchievement()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
