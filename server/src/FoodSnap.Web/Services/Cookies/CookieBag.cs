using Microsoft.AspNetCore.Http;

namespace FoodSnap.Web.Services.Cookies
{
    public class CookieBag : ICookieBag
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CookieBag(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Add(string name, string value)
        {
            Add(name, value, null);
        }

        public void Add(string name, string value, CookieOptions options)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Append(name, value, options);
        }
    }
}
