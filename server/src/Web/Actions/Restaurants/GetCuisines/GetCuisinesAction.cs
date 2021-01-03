using System.Threading.Tasks;
using Application.Restaurants.GetCuisines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Actions.Restaurants.GetCuisines
{
    public class GetCuisinesAction : Action
    {
        private readonly ISender sender;

        public GetCuisinesAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("/cuisines")]
        public async Task<IActionResult> Execute()
        {
            var query = new GetCuisinesQuery();

            var result = await sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : Error(result.Error);
        }
    }
}
