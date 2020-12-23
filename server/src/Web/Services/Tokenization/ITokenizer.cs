using System;
using Application;

namespace Web.Services.Tokenization
{
    public interface ITokenizer
    {
        string Encode(string data);
        string Encode(string data, DateTimeOffset expiry);
        Result<string> Decode(string token);
    }
}
