namespace Web
{
    public record Config
    {
        public string DbConnectionString { get; set; }
        public string GoogleGeocodingApiKey { get; set; }
        public string JWTSecret { get; set; }
        public string[] CorsOrigins { get; set; }
        public string StripePublishableKey { get; set; }
        public string StripeSecretKey { get; set; }
        public string StripeOnboardingRefreshUrl { get; set; }
        public string StripeOnboardingReturnUrl { get; set; }
        public string StripeWebhookSigningSecret { get; set; }
        public string ServerUrl { get; set; }
        public string ClientUrl { get; set; }
    }
}
