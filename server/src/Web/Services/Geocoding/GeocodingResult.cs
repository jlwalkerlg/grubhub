using Web.Domain;

namespace Web.Services.Geocoding
{
    public record GeocodingResult
    {
        public string FormattedAddress { get; init; }
        public Coordinates Coordinates { get; init; }
    }
}
