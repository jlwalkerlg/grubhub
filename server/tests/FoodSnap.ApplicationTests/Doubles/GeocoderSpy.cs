using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Services.Geocoding;

namespace FoodSnap.ApplicationTests.Doubles.GeocoderSpy
{
    public class GeocoderSpy : IGeocoder
    {
        public CoordinatesDto CoordinatesDto { get; set; }

        public Task<Result<CoordinatesDto>> GetCoordinates(AddressDto address)
        {
            return Task.FromResult(Result.Ok(CoordinatesDto));
        }
    }
}
