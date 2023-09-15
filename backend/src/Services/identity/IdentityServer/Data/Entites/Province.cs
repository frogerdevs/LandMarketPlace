namespace IdentityServer.Data.Entites
{
    public class Province : BaseEntity<string>
    {
        public string Name { get; set; }
        public List<City> Cities { get; set; }
    }
}
