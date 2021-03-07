using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Users.ChangePassword
{
    public class ChangePasswordAction : Action
    {
        private readonly ISender sender;

        public ChangePasswordAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpPut("/account/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
