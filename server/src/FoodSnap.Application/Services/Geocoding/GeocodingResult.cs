namespace FoodSnap.Application.Services.Geocoding
{
    public record GeocodingResult
    {
        public string FormattedAddress { get; init; }
        public float Latitude { get; init; }
        public float Longitude { get; init; }
    }
}
