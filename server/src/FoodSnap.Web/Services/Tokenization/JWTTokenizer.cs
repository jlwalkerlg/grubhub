using System.Collections.Generic;
using System;
using JWT.Algorithms;
using JWT.Builder;
using FoodSnap.Application;

namespace FoodSnap.Web.Services.Tokenization
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
                return Result<string>.Fail(Error.BadRequest("Token expired."));
            }
            catch (JWT.Exceptions.InvalidTokenPartsException)
            {
                return Result<string>.Fail(Error.BadRequest("Invalid token."));
            }
            catch (JWT.Exceptions.SignatureVerificationException)
            {
                return Result<string>.Fail(Error.BadRequest("Signature invalid."));
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
