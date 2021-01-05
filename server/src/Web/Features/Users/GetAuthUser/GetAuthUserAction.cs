﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Features.Users.GetAuthUser
{
    public class GetAuthUserAction : Action
    {
        private readonly ISender sender;

        public GetAuthUserAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("/auth/user")]
        public async Task<IActionResult> Execute()
        {
            var query = new GetAuthUserQuery();
            var result = await sender.Send(new GetAuthUserQuery());

            if (!result.IsSuccess)
            {
                return Error(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
