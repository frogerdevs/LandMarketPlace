namespace IdentityServer.Data.Entites
{
    public class City : BaseEntity<string>
    {
        public string Name { get; set; }
        public string ProvinceId { get; set; }
        public Province Province { get; set; }
        public List<District> Districts { get; set; }
    }
}
