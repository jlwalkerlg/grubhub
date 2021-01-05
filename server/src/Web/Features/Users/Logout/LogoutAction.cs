using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Features.Users.Logout
{
    public class LogoutAction : Action
    {
        private readonly ISender sender;

        public LogoutAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/auth/logout")]
        public async Task<IActionResult> Execute()
        {
            var result = await sender.Send(new LogoutCommand());

            return Ok();
        }
    }
}
