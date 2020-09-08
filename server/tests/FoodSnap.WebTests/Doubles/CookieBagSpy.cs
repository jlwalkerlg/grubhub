using System.Collections.Generic;
using FoodSnap.Web.Services.Cookies;
using Microsoft.AspNetCore.Http;

namespace FoodSnap.WebTests.Doubles
{
    public class CookieBagSpy : ICookieBag
    {
        private Dictionary<string, object[]> Cookies { get; }
            = new Dictionary<string, object[]>();

        public string Get(string name)
        {
            if (!Cookies.ContainsKey(name))
            {
                return null;
            }

            return (string)Cookies[name][0];
        }

        public CookieOptions GetOptions(string name)
        {
            return Cookies[name][1] as CookieOptions;
        }

        public void Add(string name, string value)
        {
            Add(name, value, null);
        }

        public void Add(string name, string value, CookieOptions options)
        {
            Cookies.Add(name, new object[] { value, options });
        }
    }
}
