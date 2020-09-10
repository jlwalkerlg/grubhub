using System.Collections.Generic;
using FoodSnap.Web.Services.Cookies;
using Microsoft.AspNetCore.Http;

namespace FoodSnap.WebTests.Doubles
{
    public class CookieBagSpy : ICookieBag
    {
        public Dictionary<string, CookieData> Cookies { get; }
            = new Dictionary<string, CookieData>();

        public Dictionary<string, CookieData> Deleted { get; }
            = new Dictionary<string, CookieData>();

        public string Get(string name)
        {
            if (!Cookies.ContainsKey(name))
            {
                return null;
            }

            return Cookies[name].Value;
        }

        public void Add(string name, string value)
        {
            Add(name, value, null);
        }

        public void Add(string name, string value, CookieOptions options)
        {
            Cookies.Add(name, new CookieData { Value = value, Options = options });
        }

        public void Delete(string name)
        {
            Delete(name, null);
        }

        public void Delete(string name, CookieOptions options)
        {
            if (Cookies.ContainsKey(name))
            {
                Cookies.Remove(name);
            }

            Deleted.Add(name, new CookieData { Options = options });
        }

        public class CookieData
        {
            public string Value { get; set; }
            public CookieOptions Options { get; set; }
        }
    }
}
