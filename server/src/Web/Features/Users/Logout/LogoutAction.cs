using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Services.Antiforgery;

namespace Web.Features.Users.Logout
{
    public class LogoutAction : Action
    {
        private readonly ISender sender;

        public LogoutAction(ISender sender)
        {
            this.sender = sender;
        }

        [IgnoreAntiforgeryValidation]
        [HttpPost("/auth/logout")]
        public async Task<IActionResult> Execute()
        {
            var result = await sender.Send(new LogoutCommand());

            HttpContext.Response.Cookies.Delete(
                "XSRF-TOKEN",
                new CookieOptions()
                {
                    HttpOnly = false,
                });

            return Ok();
        }
    }
}
