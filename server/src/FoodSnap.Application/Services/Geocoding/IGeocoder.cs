using System.Threading.Tasks;

namespace FoodSnap.Application.Services.Geocoding
{
    public interface IGeocoder
    {
        Task<CoordinatesDto> GetCoordinates(AddressDto address);
    }
}
