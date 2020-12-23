using System.Threading.Tasks;
using Application.Services.Geocoding;
using Application;
using Domain;

namespace WebTests.Doubles
{
    public class GeocoderStub : IGeocoder
    {
        public static string Address { get; } = "1 Maine Road, Manchester, MN121NM, UK";
        public static int Latitude { get; } = 54;
        public static int Longitude { get; } = -2;

        public static string InvalidAddress { get; } = "12 Invalid Street";

        public Task<Result<GeocodingResult>> Geocode(string address)
        {
            if (address == InvalidAddress)
            {
                return Task.FromResult(
                    Result<GeocodingResult>.Fail(
                        Error.Internal("Invalid address.")));
            }

            return Task.FromResult(Result.Ok(new GeocodingResult
            {
                FormattedAddress = Address,
                Coordinates = new Coordinates(Latitude, Longitude),
            }));
        }
    }
}
