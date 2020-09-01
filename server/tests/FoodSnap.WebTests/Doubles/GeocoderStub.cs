using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.WebTests.Doubles
{
    // TODO: register this in web functional tests instead of real service
    public class GeocoderStub : IGeocoder
    {
        public Task<Result<Coordinates>> GetCoordinates(Address address)
        {
            return Task.FromResult(Result.Ok(new Coordinates(1, 1)));
        }
    }
}
