using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Application.Services.Geocoding
{
    public class GeocoderAdapter
    {
        private readonly IGeocoder geocoder;

        public GeocoderAdapter(IGeocoder geocoder)
        {
            this.geocoder = geocoder;
        }

        public async Task<Result<Coordinates>> GetCoordinates(Address address)
        {
            var addressDto = new AddressDto
            {
                Line1 = address.Line1,
                Line2 = address.Line2,
                Town = address.Town,
                Postcode = address.Postcode.Code
            };

            var result = await geocoder.GetCoordinates(addressDto);

            if (!result.IsSuccess)
            {
                return Result<Coordinates>.Fail(new GeocodingError(result.Error.Message));
            }

            return Result.Ok(new Coordinates(
                result.Value.Latitude,
                result.Value.Longitude));
        }
    }
}
