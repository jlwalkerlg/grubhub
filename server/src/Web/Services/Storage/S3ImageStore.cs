using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Services.DateTimeServices;

namespace Web.Services.Storage
{
    public class S3ImageStore : IImageStore
    {
        private readonly AwsSettings settings;
        private readonly IDateTimeProvider dateTimeProvider;

        public S3ImageStore(AwsSettings settings, IDateTimeProvider dateTimeProvider)
        {
            this.settings = settings;
            this.dateTimeProvider = dateTimeProvider;
        }

        public Task<ImageUploadParams> GenerateUploadParams(
            string filename,
            string contentType,
            int? maxSize = null,
            int minSize = 0)
        {
            var now = dateTimeProvider.UtcNow;

            var conditions = new List<object[]>()
            {
                new object[] {"eq", "$bucket", settings.Bucket},
                new object[] {"eq", "$key", filename},
                new object[] {"eq", "$acl", "public-read"},
                new object[] {"eq", "$Content-Type", contentType},
                new object[] {"eq", "$x-amz-algorithm", "AWS4-HMAC-SHA256"},
                new object[] {"eq", "$x-amz-server-side-encryption", "AES256"},
                new object[] {"eq", "$x-amz-credential", $"{settings.AccessKeyId}/{now:yyyyMMdd}/{settings.Region}/s3/aws4_request"},
                new object[] {"eq", "$x-amz-date", now.ToString("yyyyMMddTHHmmssZ")},
            };

            if (maxSize.HasValue)
            {
                conditions.Add(new object[] { "content-length-range", minSize, maxSize.Value });
            }

            var policy = new Dictionary<string, object>()
            {
                {"expiration", now.AddMinutes(10).ToString("O")},
                {"conditions", conditions},
            };

            var jsonPolicy = JsonSerializer.Serialize(policy, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            });
            var utf8Policy = Encoding.UTF8.GetBytes(jsonPolicy);
            var base64Policy = Convert.ToBase64String(utf8Policy);

            var dateKey = Hmac("AWS4" + settings.SecretAccessKey, now.ToString("yyyyMMdd"));
            var dateRegionKey = Hmac(dateKey, settings.Region);
            var dateRegionServiceKey = Hmac(dateRegionKey, "s3");
            var signingKey = Hmac(dateRegionServiceKey, "aws4_request");

            var signature = Hex(Hmac(signingKey, base64Policy)).ToLower();

            var url = $"http://{settings.Bucket}.s3.amazonaws.com/";

            var inputs = new Dictionary<string, string>()
            {
                {"key", filename},
                {"acl", "public-read"},
                {"Content-Type", contentType},
                {"x-amz-algorithm", "AWS4-HMAC-SHA256"},
                {"x-amz-server-side-encryption", "AES256"},
                {"x-amz-credential", $"{settings.AccessKeyId}/{now:yyyyMMdd}/{settings.Region}/s3/aws4_request"},
                {"x-amz-date", now.ToString("yyyyMMddTHHmmssZ")},
                {"Policy", base64Policy},
                {"x-amz-signature", signature},
            };

            return Task.FromResult(new ImageUploadParams()
            {
                Url = url,
                Inputs = inputs,
            });
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
}
