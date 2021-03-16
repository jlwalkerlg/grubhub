namespace Web
{
    public record Settings
    {
        public AppSettings App { get; init; }
        public DatabaseSettings Database { get; init; }
        public GeocodingSettings Geocoding { get; init; }
        public StripeSettings Stripe { get; init; }
        public MailSettings Mail { get; init; }
    }

    public record AppSettings
    {
        public string Environment { get; init; } = "Production";
        public string ServerUrl { get; init; }
        public string ClientUrl { get; init; }
        public string[] CorsOrigins { get; init; }
        public string StripeOnboardingRefreshUrl => $"{ServerUrl}/stripe/onboarding/refresh";
        public string StripeOnboardingReturnUrl => $"{ClientUrl}/dashboard/billing";
    }

    public record GeocodingSettings
    {
        public string GoogleApiKey { get; init; }
    }

    public record DatabaseSettings
    {
        public string ConnectionString { get; init; }
    }

    public record StripeSettings
    {
        public string SecretKey { get; init; }
        public string WebhookSigningSecret { get; init; }
        public string ConnectWebhookSigningSecret { get; init; }
    }

    public record MailSettings
    {
        public string FromAddress { get; init; }
        public string FromName { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Host { get; init; }
        public int Port { get; init; }
    }
}
