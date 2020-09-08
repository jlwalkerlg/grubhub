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
                return Result<string>.Fail(new Error("Token expired."));
            }
            catch (JWT.Exceptions.InvalidTokenPartsException)
            {
                return Result<string>.Fail(new Error("Invalid token."));
            }
            catch (JWT.Exceptions.SignatureVerificationException)
            {
                return Result<string>.Fail(new Error("Signature invalid."));
            }
            catch (System.Exception)
            {
                return Result<string>.Fail(new Error());
            }
        }

        public string Encode(string data)
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
