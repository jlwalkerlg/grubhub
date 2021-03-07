using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Users;

namespace Web.Features.Orders.RejectOrder
{
    public class RejectOrderAction : Action
    {
        private readonly ISender sender;

        public RejectOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPut("/orders/{orderId}/reject")]
        public async Task<IActionResult> RejectOrder([FromRoute] string orderId)
        {
            var command = new RejectOrderCommand()
            {
                OrderId = orderId,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
