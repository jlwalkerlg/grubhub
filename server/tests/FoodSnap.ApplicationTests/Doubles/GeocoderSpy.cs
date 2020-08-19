using System.Threading.Tasks;
using FoodSnap.Application.Services.Geocoding;

namespace FoodSnap.ApplicationTests.Doubles.GeocoderSpy
{
    public class GeocoderSpy : IGeocoder
    {
        public CoordinatesDto CoordinatesDto { get; set; }

        public Task<CoordinatesDto> GetCoordinates(AddressDto address)
        {
            return Task.FromResult(CoordinatesDto);
        }
    }
}
