namespace IdentityServer.Data.Entites
{
    public class UserSubscription : BaseEntity<string>
    {
        public required string UserId { get; set; }
        public required string SubscriptionId { get; set; }
        public bool Active { get; set; }
        public int ActiveDurationDays { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal SubscriptionPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }

        public AppUser? AppUser { get; set; }
        public UserSubscription()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
