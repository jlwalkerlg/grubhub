using System;
using Application.Services.Authentication;
using Domain.Users;
using Web.Services.Cookies;
using Web.Services.Tokenization;
using Microsoft.AspNetCore.Http;

namespace Web.Services.Authentication
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

        public bool IsAuthenticated => UserId != null;

        public UserId UserId
        {
            get
            {
                if (idValue == null)
                {
                    idValue = GetUserId();
                }

                return idValue == Guid.Empty ? null : new UserId(idValue.Value);
            }
        }

        private Guid? idValue = null;

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

            var token = tokenizer.Encode(user.Id.Value.ToString());

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
