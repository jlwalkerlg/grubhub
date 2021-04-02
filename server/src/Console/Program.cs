using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Web;

namespace Console
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Web.Program.CreateHostBuilder(args);
            builder.ConfigureServices((_, s) =>
            {
                s.AddScoped<Seeder>();
            });
            var host = builder.Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                if (args.Contains("seed"))
                {
                    var seeder = services.GetRequiredService<Seeder>();
                    await seeder.Seed();
                }
                else
                {
                    await Run(services);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        private static async Task Run(IServiceProvider services)
        {
            await Task.CompletedTask;

            var settings = services.GetRequiredService<AwsSettings>();

            // See: https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-post-example.html
            // for allowed conditions, see https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-HTTPPOSTConstructPolicy.html#sigv4-PolicyConditions
            var policy = new S3PostPolicy(DateTime.UtcNow);
            policy.AddCondition(new ExactCondition("bucket", settings.Bucket));
            policy.AddCondition(new RangeCondition("content-length-range", 1, 2 * (int)Math.Pow(2, 20)));
            policy.AddCondition(new ExactCondition("key", "filename.jpg"));
            policy.AddCondition(new ExactCondition("x-amz-algorithm", "AWS4-HMAC-SHA256"));
            policy.AddCondition(new ExactCondition("x-amz-credential", $"{settings.AccessKeyId}/{DateTime.UtcNow.ToString("yyyyMMdd")}/{settings.Region}/s3/aws4_request"));
            policy.AddCondition(new StartsWithCondition("Content-Type", "image/jpeg,image/png,image/gif"));

            var signature = policy.GetSignature(settings.SecretAccessKey, settings.Region, DateTime.UtcNow);

            System.Console.WriteLine(signature);
        }
    }

    public class S3PostPolicy
    {
        private readonly DateTime expiration;
        private readonly List<ICondition> conditions = new();

        public S3PostPolicy(DateTime expiration)
        {
            this.expiration = expiration;
        }

        public void AddCondition(ICondition condition)
        {
            conditions.Add(condition);
        }

        public string GetSignature(string secretAccessKey, string region, DateTime time)
        {
            var jsonPolicy = JsonSerializer.Serialize(ToDictionary(), new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            });
            var utf8Policy = Encoding.UTF8.GetBytes(jsonPolicy);
            var base64Policy = Convert.ToBase64String(utf8Policy);

            var dateKey = Hmac("AWS4" + secretAccessKey, time.ToString("yyyyMMdd"));
            var dateRegionKey = Hmac(dateKey, region);
            var dateRegionServiceKey = Hmac(dateRegionKey, "s3");
            var signingKey = Hmac(dateRegionServiceKey, "aws4_request");

            return Hex(Hmac(signingKey, base64Policy));
        }

        private Dictionary<string, object> ToDictionary()
        {
            return new()
            {
                {"expiration", expiration.ToString("O")},
                {"conditions", conditions.Select(x => x.ToArray()).ToList()},
            };
        }

        private static byte[] Hmac(string key, string unsigned)
        {
            return Hmac(Encoding.UTF8.GetBytes(key), unsigned);
        }

        private static byte[] Hmac(byte[] key, string unsigned)
        {
            using var mac = new HMACSHA256(key);
            return mac.ComputeHash(Encoding.UTF8.GetBytes(unsigned));
        }

        private static string Hex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }

    public interface ICondition
    {
        string[] ToArray();
    }

    public class ExactCondition : ICondition
    {
        private readonly string field;
        private readonly string value;

        public ExactCondition(string field, string value)
        {
            this.field = field;
            this.value = value;
        }

        public string[] ToArray()
        {
            return new[] {"eq", $"${field}", value};
        }
    }

    public class StartsWithCondition : ICondition
    {
        private readonly string field;
        private readonly string value;

        public StartsWithCondition(string field, string value)
        {
            this.field = field;
            this.value = value;
        }

        public string[] ToArray()
        {
            return new[] {"starts-with", $"${field}", value};
        }
    }

    public class RangeCondition : ICondition
    {
        private readonly string field;
        private readonly int min;
        private readonly int max;

        public RangeCondition(string field, int min, int max)
        {
            this.field = field;
            this.min = min;
            this.max = max;
        }

        public string[] ToArray()
        {
            return new[] {field, min.ToString(), max.ToString()};
        }
    }
}
