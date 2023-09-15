namespace IdentityServer.Data.Entites
{
    public class SubDistrict : BaseEntity<string>
    {
        public string Name { get; set; }
        public string DistrictId { get; set; }
        public string PostCode { get; set; }
        public District District { get; set; }
    }
}
