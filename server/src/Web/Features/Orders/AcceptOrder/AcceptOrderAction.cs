using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Users;

namespace Web.Features.Orders.AcceptOrder
{
    public class AcceptOrderAction : Action
    {
        private readonly ISender sender;

        public AcceptOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPut("/orders/{orderId:guid}/accept")]
        public async Task<IActionResult> AcceptOrder([FromRoute] string orderId)
        {
            var command = new AcceptOrderCommand()
            {
                OrderId = orderId,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
