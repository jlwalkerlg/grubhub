using Microsoft.AspNetCore.Http;
using System;

namespace Web.Services.Cookies
{
    public class CookieBag : ICookieBag
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HttpContext httpContext;

        public CookieBag(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public CookieBag(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        private HttpContext HttpContext => httpContext ?? httpContextAccessor.HttpContext;

        public void Add(string name, string value)
        {
            Add(name, value, null);
        }

        public void Add(string name, string value, CookieOptions options)
        {
            HttpContext
                .Response
                .Cookies
                .Append(name, value, options);
        }

        public void Delete(string name)
        {
            Delete(name, new());
        }

        public void Delete(string name, CookieOptions options)
        {
            options.Expires = DateTime.UnixEpoch;

            Add(name, "", options);
        }

        public string Get(string name)
        {
            var cookies = HttpContext
                .Request
                .Cookies;

            if (!cookies.ContainsKey(name))
            {
                return null;
            }

            return cookies[name];
        }
    }
}
