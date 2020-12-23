using System;
using Microsoft.AspNetCore.Http;

namespace Web.Services.Cookies
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
            httpContextAccessor
                .HttpContext
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
            var cookies = httpContextAccessor
                .HttpContext
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
