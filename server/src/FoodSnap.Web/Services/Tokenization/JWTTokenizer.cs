using System.Collections.Generic;
using System;
using JWT.Algorithms;
using JWT.Builder;

namespace FoodSnap.Web.Services.Tokenization
{
    public class JWTTokenizer : ITokenizer
    {
        private readonly string secret;

        public JWTTokenizer(string secret)
        {
            this.secret = secret;
        }

        public string Decode(string token)
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secret)
                .MustVerifySignature()
                .Decode<Dictionary<string, string>>(token)["payload"];
        }

        public string Encode(object data)
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secret)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddDays(14).ToUnixTimeSeconds())
                .AddClaim("payload", data)
                .Encode();
        }
    }
}
