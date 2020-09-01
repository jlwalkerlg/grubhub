using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.ApplicationTests.Doubles.GeocoderSpy
{
    public class GeocoderSpy : IGeocoder
    {
        public Coordinates Coordinates { get; set; }
        public Address Address { get; private set; }

        public Task<Result<Coordinates>> GetCoordinates(Address address)
        {
            Address = address;
            return Task.FromResult(Result.Ok(Coordinates));
        }
    }
}
