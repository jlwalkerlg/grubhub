using Microsoft.AspNetCore.Http;

namespace FoodSnap.Web.Services.Cookies
{
    public interface ICookieBag
    {
        void Add(string name, string value);
        void Add(string name, string value, CookieOptions options);
    }
}