namespace Web.Features.Restaurants
{
    public record OpeningTimesDto
    {
        public OpeningHoursDto Monday { get; init; }
        public OpeningHoursDto Tuesday { get; init; }
        public OpeningHoursDto Wednesday { get; init; }
        public OpeningHoursDto Thursday { get; init; }
        public OpeningHoursDto Friday { get; init; }
        public OpeningHoursDto Saturday { get; init; }
        public OpeningHoursDto Sunday { get; init; }
    }

    public record OpeningHoursDto
    {
        public string Open { get; init; }
        public string Close { get; init; }
    }
}
