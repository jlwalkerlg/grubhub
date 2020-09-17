using System;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Domain.Users;
using FoodSnap.Web.Services.Cookies;
using FoodSnap.Web.Services.Tokenization;
using Microsoft.AspNetCore.Http;

namespace FoodSnap.Web.Services.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private readonly ITokenizer tokenizer;
        private readonly ICookieBag cookies;

        public Authenticator(ITokenizer tokenizer, ICookieBag cookies)
        {
            this.tokenizer = tokenizer;
            this.cookies = cookies;
        }

        public bool IsAuthenticated => GetUserId() != Guid.Empty;

        public Guid UserId => userId ??= GetUserId();

        private Guid? userId = null;

        public Guid GetUserId()
        {
            var token = cookies.Get("auth_token");

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

        public void SignIn(User user)
        {
            var expiresIn = DateTimeOffset.UtcNow.AddDays(14);

            var token = tokenizer.Encode(user.Id.ToString());

            cookies.Add("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = expiresIn,
                Path = "/",
            });
        }

        public void SignOut()
        {
            cookies.Delete("auth_token", new CookieOptions
            {
                HttpOnly = true,
                Path = "/",
            });
        }
    }
}
