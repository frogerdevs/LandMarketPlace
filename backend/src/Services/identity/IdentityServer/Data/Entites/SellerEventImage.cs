namespace IdentityServer.Data.Entites
{
    public class SellerEventImage : BaseEntity<string>
    {
        public required string SellerEventId { get; set; }
        public string? Image { get; set; }
        public virtual SellerEvent? SellerEvent { get; set; }
        public SellerEventImage()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
