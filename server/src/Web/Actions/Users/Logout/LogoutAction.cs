using System.Threading.Tasks;
using Application.Users.Logout;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Actions.Users.Logout
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
