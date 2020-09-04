using FoodSnap.Web.Services.Tokenization;

namespace FoodSnap.WebTests.Doubles
{
    public class TokenizerSpy : ITokenizer
    {
        public string EncodedToken { get; set; }
        public string DecodedData { get; set; }

        public object Data { get; private set; }
        public string TokenToDecode { get; private set; }

        public string Encode(object data)
        {
            Data = data;
            return EncodedToken;
        }

        public string Decode(string token)
        {
            TokenToDecode = token;
            return DecodedData;
        }
    }
}
