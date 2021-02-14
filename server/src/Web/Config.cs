namespace Web
{
    public record Config
    {
        public string ServerUrl { get; set; }
        public string ClientUrl { get; set; }
        public string DbConnectionString { get; set; }
        public string GoogleGeocodingApiKey { get; set; }
        public string[] CorsOrigins { get; set; }
        public string StripePublishableKey { get; set; }
        public string StripeSecretKey { get; set; }
        public string StripeWebhookSigningSecret { get; set; }
        public string StripeConnectWebhookSigningSecret { get; set; }
        public string StripeOnboardingRefreshUrl => $"{ServerUrl}/stripe/onboarding/refresh";
        public string StripeOnboardingReturnUrl => $"{ClientUrl}/dashboard/billing";
        public decimal ServiceCharge => 0.50m;
    }
}
