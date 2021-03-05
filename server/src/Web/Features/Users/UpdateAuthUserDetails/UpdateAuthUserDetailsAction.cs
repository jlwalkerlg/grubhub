using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Web.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsAction : Action
    {
        private readonly ISender sender;

        public UpdateAuthUserDetailsAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpPut("/auth/user")]
        public async Task<IActionResult> Execute([FromBody] UpdateAuthUserDetailsCommand command)
        {
            var result = await sender.Send(command);

            if (!result)
            {
                return Problem(result.Error);
            }

            return Ok();
        }
    }
}
