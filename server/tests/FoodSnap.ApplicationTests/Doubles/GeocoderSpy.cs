using System.Threading.Tasks;
using FoodSnap.Application.Services;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.ApplicationTests.Doubles.GeocoderSpy
{
    public class GeocoderSpy : IGeocoder
    {
        public Coordinates Coordinates { get; set; }

        public Task<Coordinates> GetCoordinates(Address address)
        {
            return Task.FromResult(Coordinates);
        }
    }
}
