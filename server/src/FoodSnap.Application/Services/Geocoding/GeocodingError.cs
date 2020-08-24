namespace FoodSnap.Application.Services.Geocoding
{
    public class GeocodingError : Error
    {
        public GeocodingError()
        {
        }

        public GeocodingError(string message) : base(message)
        {
        }
    }
}
