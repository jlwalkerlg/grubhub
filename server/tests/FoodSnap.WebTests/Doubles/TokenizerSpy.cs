using System;
using FoodSnap.Shared;
using FoodSnap.Web.Services.Tokenization;

namespace FoodSnap.WebTests.Doubles
{
    public class TokenizerSpy : ITokenizer
    {
        public string EncodedToken { get; set; }
        public Result<string> DecodeResult { get; set; }

        public string Data { get; private set; }
        public string TokenToDecode { get; private set; }

        public string Encode(string data)
        {
            Data = data;
            return EncodedToken;
        }

        public string Encode(string data, DateTimeOffset expiry)
        {
            return Encode(data);
        }

        public Result<string> Decode(string token)
        {
            TokenToDecode = token;
            return DecodeResult;
        }
    }
}
