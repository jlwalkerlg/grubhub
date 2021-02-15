using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await sender.Send(command);

            if (!result)
            {
                return Error(result.Error);
            }

            var token = antiforgery.GetAndStoreTokens(HttpContext);

            HttpContext.Response.Cookies.Append(
                "XSRF-TOKEN",
                token.RequestToken,
                new CookieOptions()
                {
                    HttpOnly = false,
                });

            return Ok();
        }
    }
}
