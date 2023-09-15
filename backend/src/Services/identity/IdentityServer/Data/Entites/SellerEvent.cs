namespace IdentityServer.Data.Entites
{
    public class SellerEvent : BaseEntity<string>
    {
        public required string UserId { get; set; }
        public string? Title { get; set; }
        public AppUser? User { get; set; }
        public virtual ICollection<SellerEventImage>? Images { get; set; }
        public SellerEvent()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
