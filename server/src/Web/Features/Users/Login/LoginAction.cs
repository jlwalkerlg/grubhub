using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Services.Antiforgery;

namespace Web.Features.Users.Login
{
    public class LoginAction : Action
    {
        private readonly ISender sender;
        private readonly IAntiforgery antiforgery;

        public LoginAction(ISender sender, IAntiforgery antiforgery)
        {
            this.sender = sender;
            this.antiforgery = antiforgery;
        }

        [IgnoreAntiforgeryValidation]
        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await sender.Send(command);

            if (!result) return Problem(result.Error);

            var token = antiforgery.GetAndStoreTokens(HttpContext);

            return Ok(new LoginResponse()
            {
                XsrfToken = token.RequestToken,
            });
        }
    }
}
