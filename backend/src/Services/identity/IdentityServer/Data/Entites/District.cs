namespace IdentityServer.Data.Entites
{
    public class District : BaseEntity<string>
    {
        public string Name { get; set; }
        public string CityId { get; set; }
        public City City { get; set; }
        public List<SubDistrict>? SubDistricts { get; set; }
    }
}
