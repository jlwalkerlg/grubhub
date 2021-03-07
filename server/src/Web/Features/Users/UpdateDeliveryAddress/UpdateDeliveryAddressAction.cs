using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Users.UpdateDeliveryAddress
{
    public class UpdateDeliveryAddressAction : Action
    {
        private readonly ISender sender;

        public UpdateDeliveryAddressAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpPut("/account/delivery-address")]
        public async Task<IActionResult> UpdateDeliveryAddress([FromBody] UpdateDeliveryAddressCommand command)
        {
            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
