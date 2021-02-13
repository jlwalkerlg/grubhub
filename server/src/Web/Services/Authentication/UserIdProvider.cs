using Microsoft.AspNetCore.SignalR;
using Web.Services.Cookies;
using Web.Services.Tokenization;

namespace Web.Services.Authentication
{
    public class UserIdProvider : IUserIdProvider
    {
        private readonly Config config;
        private readonly ITokenizer tokenizer;

        public UserIdProvider(Config config, ITokenizer tokenizer)
        {
            this.config = config;
            this.tokenizer = tokenizer;
        }

        public string GetUserId(HubConnectionContext connection)
        {
            var cookies = new CookieBag(connection.GetHttpContext());
            var authenticator = new Authenticator(tokenizer, cookies);

            return authenticator.UserId.Value.ToString();
        }
    }
}
