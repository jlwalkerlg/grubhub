namespace Web
{
    public record Config
    {
        public string Environment { get; init; } = "Production";
        public string ServerUrl { get; set; }
        public string ClientUrl { get; set; }
        public string DbConnectionString { get; set; }
        public string GoogleGeocodingApiKey { get; set; }
        public string[] CorsOrigins { get; set; }
        public string StripeSecretKey { get; set; }
        public string StripeWebhookSigningSecret { get; set; }
        public string StripeConnectWebhookSigningSecret { get; set; }
        public string StripeOnboardingRefreshUrl => $"{ServerUrl}/stripe/onboarding/refresh";
        public string StripeOnboardingReturnUrl => $"{ClientUrl}/dashboard/billing";
        public string MailFromAddress { get; set; }
        public string MailFromName { get; set; }
        public string MailUsername { get; set; }
        public string MailPassword { get; set; }
        public string MailHost { get; set; }
        public int MailPort { get; set; }
    }
}
