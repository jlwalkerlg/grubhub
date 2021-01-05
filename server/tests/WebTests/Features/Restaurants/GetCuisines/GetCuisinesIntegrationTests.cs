using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Features.Restaurants;
using Web.Domain.Restaurants;
using Xunit;

namespace WebTests.Features.Restaurants.GetCuisines
{
    public class GetCuisinesIntegrationTests : WebIntegrationTestBase
    {
        public GetCuisinesIntegrationTests(WebIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_All_Cuisines()
        {
            var italian = new Cuisine("Italian");
            var thai = new Cuisine("Thai");
            var indian = new Cuisine("Indian");

            await fixture.InsertDb(italian, thai, indian);

            var cuisines = await Get<List<CuisineDto>>("/cuisines");

            Assert.Equal(3, cuisines.Count);
            Assert.Contains(cuisines, x => x.Name == "Italian");
            Assert.Contains(cuisines, x => x.Name == "Thai");
            Assert.Contains(cuisines, x => x.Name == "Indian");
        }
    }
}
