using System.Threading.Tasks;
using FoodSnap.Application.Services.Hashing;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.GetUserByEmail;
using FoodSnap.Web.Services.Cookies;
using FoodSnap.Web.Services.Tokenization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.Login
{
    public class LoginAction : Action
    {
        private readonly IMediator mediator;
        private readonly IHasher hasher;
        private readonly ITokenizer tokenizer;
        private readonly ICookieBag cookieBag;

        public LoginAction(
            IMediator mediator,
            IHasher hasher,
            ITokenizer tokenizer,
            ICookieBag cookieBag)
        {
            this.mediator = mediator;
            this.hasher = hasher;
            this.tokenizer = tokenizer;
            this.cookieBag = cookieBag;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var query = new GetUserByEmailQuery(request.Email);
            var user = (await mediator.Send(query)).Value;

            if (user is null || !hasher.CheckMatch(request.Password, user.Password))
            {
                return BadRequest(new ErrorEnvelope("Invalid credentials."));
            }

            var token = tokenizer.Encode(user.Id);
            cookieBag.Add("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
            });

            return Ok(new DataEnvelope(user));
        }
    }
}
