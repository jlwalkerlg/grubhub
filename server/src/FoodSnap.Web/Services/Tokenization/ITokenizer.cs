namespace FoodSnap.Web.Services.Tokenization
{
    public interface ITokenizer
    {
        string Encode(object data);
        string Decode(string token);
    }
}
