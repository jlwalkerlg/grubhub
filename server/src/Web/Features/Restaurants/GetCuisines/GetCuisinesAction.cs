using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Features.Restaurants.GetCuisines
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

            return result ? Ok(result.Value) : Error(result.Error);
        }
    }
}
