using System;
using FoodSnap.Web.Queries.Users;
using FoodSnap.Web.Services.Cookies;
using FoodSnap.Web.Services.Tokenization;
using Microsoft.AspNetCore.Http;

namespace FoodSnap.Web.Services.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private readonly ITokenizer tokenizer;
        private readonly ICookieBag cookieBag;

        public Authenticator(ITokenizer tokenizer, ICookieBag cookieBag)
        {
            this.tokenizer = tokenizer;
            this.cookieBag = cookieBag;
        }

        public Guid? GetUserId()
        {
            var token = cookieBag.Get("auth_token");

            if (token == null)
            {
                return Guid.Empty;
            }

            var result = tokenizer.Decode(token);
            if (!result.IsSuccess)
            {
                return Guid.Empty;
            }

            if (!Guid.TryParse(result.Value, out Guid id))
            {
                return Guid.Empty;
            }

            return id;
        }

        public void SignIn(UserDto user)
        {
            var expiresIn = DateTimeOffset.UtcNow.AddDays(14);

            var token = tokenizer.Encode(user.Id.ToString());
            cookieBag.Add("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = expiresIn,
            });
        }
    }
}
