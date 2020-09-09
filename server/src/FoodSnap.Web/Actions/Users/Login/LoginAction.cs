using System.Threading.Tasks;
using FoodSnap.Application.Services.Hashing;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.Users.GetUserByEmail;
using FoodSnap.Web.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.Login
{
    public class LoginAction : Action
    {
        private readonly IMediator mediator;
        private readonly IHasher hasher;
        private readonly IAuthenticator authenticator;

        public LoginAction(
            IMediator mediator,
            IHasher hasher,
            IAuthenticator authenticator)
        {
            this.mediator = mediator;
            this.hasher = hasher;
            this.authenticator = authenticator;
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

            authenticator.SignIn(user);

            return Ok(new DataEnvelope(user));
        }
    }
}
