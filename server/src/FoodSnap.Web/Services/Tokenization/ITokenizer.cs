using FoodSnap.Application;

namespace FoodSnap.Web.Services.Tokenization
{
    public interface ITokenizer
    {
        string Encode(string data);
        Result<string> Decode(string token);
    }
}
