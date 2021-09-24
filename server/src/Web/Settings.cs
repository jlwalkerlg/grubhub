namespace Web
{
    public record Settings
    {
        public AppSettings App { get; init; }
        public DatabaseSettings Database { get; init; }
        public GeocodingSettings Geocoding { get; init; }
        public StripeSettings Stripe { get; init; }
        public MailSettings Mail { get; init; }
        public AwsSettings Aws { get; init; }
        public AzureSettings Azure { get; init; }
        public CacheSettings Cache { get; init; }
        public CapSettings Cap { get; init; }
    }

    public record AppSettings
    {
        public string ServerUrl { get; init; }
        public string ClientUrl { get; init; }
        public string[] CorsOrigins { get; init; }
        public string StripeOnboardingRefreshUrl => $"{ServerUrl}/stripe/onboarding/refresh";
        public string StripeOnboardingReturnUrl => $"{ClientUrl}/dashboard/billing";
    }

    public record DatabaseSettings
    {
        public string ConnectionString { get; init; }
    }

    public record GeocodingSettings
    {
        public string Driver { get; init; }
        public string GoogleApiKey { get; init; }
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

    public record AwsSettings
    {
        public string AccessKeyId { get; init; }
        public string SecretAccessKey { get; init; }
        public string Region { get; init; }
        public string Bucket { get; init; }
    }

    public record AzureSettings
    {
        public ServiceBusSettings ServiceBus { get; init; }

        public record ServiceBusSettings
        {
            public string ConnectionString { get; init; }
        }
    }

    public record CacheSettings
    {
        public CacheDriver Driver { get; init; }
        public RedisSettings Redis { get; init; }

        public enum CacheDriver
        {
            InMemory,
            Redis,
        }

        public record RedisSettings
        {
            public string ConnectionString { get; init; }
            public string InstanceName { get; init; }
        }
    }

    public record CapSettings
    {
        public TransportSettings Transport { get; init; }
        public StorageSettings Storage { get; init; }

        public record TransportSettings
        {
            public TransportDriver Driver { get; init; }

            public enum TransportDriver
            {
                InMemory,
                AmazonSQS,
                AzureServiceBus,
            }
        }

        public record StorageSettings
        {
            public StorageDriver Driver { get; init; }

            public enum StorageDriver
            {
                InMemory,
                PostgreSql,
            }
        }
    }
}
