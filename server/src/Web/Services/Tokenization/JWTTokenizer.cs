using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;

namespace Web.Services.Tokenization
{
    public class JWTTokenizer : ITokenizer
    {
        private readonly string secret;

        public JWTTokenizer(string secret)
        {
            this.secret = secret;
        }

        public Result<string> Decode(string token)
        {
            try
            {
                return Result.Ok(new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret)
                    .MustVerifySignature()
                    .Decode<Dictionary<string, string>>(token)["payload"]);
            }
            catch (JWT.Exceptions.TokenExpiredException)
            {
                return Error.Internal("Token expired.");
            }
            catch (JWT.Exceptions.InvalidTokenPartsException)
            {
                return Error.Internal("Invalid token.");
            }
            catch (JWT.Exceptions.SignatureVerificationException)
            {
                return Error.Internal("Signature invalid.");
            }
        }

        public string Encode(string data)
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secret)
                .AddClaim("payload", data)
                .Encode();
        }

        public string Encode(string data, DateTimeOffset expiry)
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secret)
                .AddClaim("exp", expiry.ToUnixTimeSeconds())
                .AddClaim("payload", data)
                .Encode();
        }
    }
}
