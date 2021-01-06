using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Features.Users.UpdateAuthUserDetails
{
    public class UpdateAuthUserDetailsAction : Action
    {
        private readonly ISender sender;

        public UpdateAuthUserDetailsAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPut("/auth/user")]
        public async Task<IActionResult> Execute([FromBody] UpdateAuthUserDetailsCommand command)
        {
            var result = await sender.Send(command);

            if (!result)
            {
                return Error(result.Error);
            }

            return Ok();
        }
    }
}
