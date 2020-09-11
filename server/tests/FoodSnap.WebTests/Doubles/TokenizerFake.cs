using System;
using FoodSnap.Application;
using FoodSnap.Web.Services.Tokenization;

namespace FoodSnap.WebTests.Doubles
{
    public class TokenizerFake : ITokenizer
    {
        public Result<string> Decode(string token)
        {
            if (!token.StartsWith("token:"))
            {
                return Result<string>.Fail(Error.BadRequest("Invalid token."));
            }

            if (token.Contains(":expiry:"))
            {
                return Result.Ok(token.Split(":expiry:")[0]);
            }

            return Result.Ok(token.Substring("token:".Length));
        }

        public string Encode(string data)
        {
            return $"token:{data}";
        }

        public string Encode(string data, DateTimeOffset expiry)
        {
            return $"token:{data}:expiry:{expiry.ToUnixTimeSeconds()}";
        }
    }
}
