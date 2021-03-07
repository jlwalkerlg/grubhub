using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Users.UpdateAccountDetails
{
    public class UpdateAccountDetailsAction : Action
    {
        private readonly ISender sender;

        public UpdateAccountDetailsAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpPut("/account/details")]
        public async Task<IActionResult> UpdateAccountDetails([FromBody] UpdateAccountDetailsCommand command)
        {
            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
