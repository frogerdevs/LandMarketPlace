namespace IdentityServer.Configs
{
    public class AuthSettings
    {
        public GoogleSetting? Google { get; set; }
    }
    public class GoogleSetting
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
