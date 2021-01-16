namespace Web
{
    public record Config
    {
        public string DbConnectionString { get; set; }
        public string GoogleGeocodingApiKey { get; set; }
        public string JWTSecret { get; set; }
        public string[] CorsOrigins { get; set; }
    }
}