using MediatR;
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
            await sender.Send(new LogoutCommand());

            return Ok();
        }
    }
}
