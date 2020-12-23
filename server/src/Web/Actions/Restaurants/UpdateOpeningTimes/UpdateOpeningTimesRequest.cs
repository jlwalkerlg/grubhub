namespace Web.Actions.Restaurants.UpdateOpeningTimes
{
    public record UpdateOpeningTimesRequest
    {
        public string MondayOpen { get; init; }
        public string MondayClose { get; init; }
        public string TuesdayOpen { get; init; }
        public string TuesdayClose { get; init; }
        public string WednesdayOpen { get; init; }
        public string WednesdayClose { get; init; }
        public string ThursdayOpen { get; init; }
        public string ThursdayClose { get; init; }
        public string FridayOpen { get; init; }
        public string FridayClose { get; init; }
        public string SaturdayOpen { get; init; }
        public string SaturdayClose { get; init; }
        public string SundayOpen { get; init; }
        public string SundayClose { get; init; }
    }
}
