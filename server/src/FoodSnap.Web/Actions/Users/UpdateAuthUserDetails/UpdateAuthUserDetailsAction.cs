using System.Threading.Tasks;
using FoodSnap.Application.Users.UpdateAuthUserDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Users.UpdateAuthUserDetails
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

            if (!result.IsSuccess)
            {
                return Error(result.Error);
            }

            return Ok();
        }
    }
}
