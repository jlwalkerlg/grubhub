﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.Users;

namespace Web.Features.Orders.CancelOrder
{
    public class CancelOrderAction : Action
    {
        private readonly ISender sender;

        public CancelOrderAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPut("/orders/{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder([FromRoute] string orderId)
        {
            var command = new CancelOrderCommand()
            {
                OrderId = orderId,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
