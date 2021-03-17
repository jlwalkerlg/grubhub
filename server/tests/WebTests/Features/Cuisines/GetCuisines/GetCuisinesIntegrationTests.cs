using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Features.Cuisines;
using Web.Features.Cuisines.GetCuisines;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Cuisines.GetCuisines
{
    public class GetCuisinesIntegrationTests : IntegrationTestBase
    {
        public GetCuisinesIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_All_Cuisines()
        {
            var italian = new Cuisine()
            {
                Name = "Italian",
            };

            var thai = new Cuisine()
            {
                Name = "Thai",
            };

            var indian = new Cuisine()
            {
                Name = "Indian",
            };

            Insert(italian, thai, indian);

            var response = await factory.GetClient().Get("/cuisines");

            response.StatusCode.ShouldBe(200);

            var cuisines = await response.GetData<List<GetCuisinesAction.CuisineModel>>();

            cuisines.Count.ShouldBe(3);
            cuisines.ShouldContain(x => x.Name == "Italian");
            cuisines.ShouldContain(x => x.Name == "Thai");
            cuisines.ShouldContain(x => x.Name == "Indian");
        }
    }
}
