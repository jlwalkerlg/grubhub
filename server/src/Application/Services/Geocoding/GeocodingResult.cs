using Domain;

namespace Application.Services.Geocoding
{
    public record GeocodingResult
    {
        public string FormattedAddress { get; init; }
        public Coordinates Coordinates { get; init; }
    }
}
