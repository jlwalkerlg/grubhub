using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Services.Geocoding;

namespace FoodSnap.WebTests.Doubles
{
    // TODO: register this in web functional tests instead of real service
    public class GeocoderStub : IGeocoder
    {
        public Task<Result<CoordinatesDto>> GetCoordinates(AddressDto address)
        {
            return Task.FromResult(Result.Ok(new CoordinatesDto
            {
                Latitude = 1,
                Longitude = 2
            }));
        }
    }
}
