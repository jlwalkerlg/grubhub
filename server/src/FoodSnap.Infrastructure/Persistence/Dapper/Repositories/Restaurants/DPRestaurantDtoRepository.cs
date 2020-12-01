using System;
using System.Threading.Tasks;
using Dapper;
using FoodSnap.Application.Restaurants;

namespace FoodSnap.Infrastructure.Persistence.Dapper.Repositories.Restaurants
{
    public class DPRestaurantDtoRepository : IRestaurantDtoRepository
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public DPRestaurantDtoRepository(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<RestaurantDto> GetById(Guid id)
        {
            var sql = @"
                SELECT
                    r.id,
                    r.manager_id,
                    r.name,
                    r.phone_number,
                    r.address,
                    r.latitude,
                    r.longitude,
                    r.status,
                    r.monday_open,
                    r.monday_close,
                    r.tuesday_open,
                    r.tuesday_close,
                    r.wednesday_open,
                    r.wednesday_close,
                    r.thursday_open,
                    r.thursday_close,
                    r.friday_open,
                    r.friday_close,
                    r.saturday_open,
                    r.saturday_close,
                    r.sunday_open,
                    r.sunday_close
                FROM
                    restaurants r
                WHERE
                    r.id = @Id";

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var entry = await connection
                    .QuerySingleOrDefaultAsync<RestaurantEntry>(
                        sql,
                        new { Id = id });

                if (entry == null)
                {
                    return null;
                }

                return new RestaurantDto()
                {
                    Id = entry.Id,
                    ManagerId = entry.ManagerId,
                    Name = entry.Name,
                    PhoneNumber = entry.PhoneNumber,
                    Address = entry.Address,
                    Latitude = entry.Latitude,
                    Longitude = entry.Longitude,
                    Status = entry.Status,
                    OpeningTimes = new OpeningTimesDto()
                    {
                        Monday = entry.MondayOpen.HasValue ? new OpeningHoursDto()
                        {
                            Open = $"{entry.MondayOpen?.Hours.ToString().PadLeft(2, '0')}:{entry.MondayOpen?.Minutes.ToString().PadLeft(2, '0')}",
                            Close = $"{entry.MondayClose?.Hours.ToString().PadLeft(2, '0')}:{entry.MondayClose?.Minutes.ToString().PadLeft(2, '0')}",
                        } : null,
                        Tuesday = entry.TuesdayOpen.HasValue ? new OpeningHoursDto()
                        {
                            Open = $"{entry.TuesdayOpen?.Hours.ToString().PadLeft(2, '0')}:{entry.TuesdayOpen?.Minutes.ToString().PadLeft(2, '0')}",
                            Close = $"{entry.TuesdayClose?.Hours.ToString().PadLeft(2, '0')}:{entry.TuesdayClose?.Minutes.ToString().PadLeft(2, '0')}",
                        } : null,
                        Wednesday = entry.WednesdayOpen.HasValue ? new OpeningHoursDto()
                        {
                            Open = $"{entry.WednesdayOpen?.Hours.ToString().PadLeft(2, '0')}:{entry.WednesdayOpen?.Minutes.ToString().PadLeft(2, '0')}",
                            Close = $"{entry.WednesdayClose?.Hours.ToString().PadLeft(2, '0')}:{entry.WednesdayClose?.Minutes.ToString().PadLeft(2, '0')}",
                        } : null,
                        Thursday = entry.ThursdayOpen.HasValue ? new OpeningHoursDto()
                        {
                            Open = $"{entry.ThursdayOpen?.Hours.ToString().PadLeft(2, '0')}:{entry.ThursdayOpen?.Minutes.ToString().PadLeft(2, '0')}",
                            Close = $"{entry.ThursdayClose?.Hours.ToString().PadLeft(2, '0')}:{entry.ThursdayClose?.Minutes.ToString().PadLeft(2, '0')}",
                        } : null,
                        Friday = entry.FridayOpen.HasValue ? new OpeningHoursDto()
                        {
                            Open = $"{entry.FridayOpen?.Hours.ToString().PadLeft(2, '0')}:{entry.FridayOpen?.Minutes.ToString().PadLeft(2, '0')}",
                            Close = $"{entry.FridayClose?.Hours.ToString().PadLeft(2, '0')}:{entry.FridayClose?.Minutes.ToString().PadLeft(2, '0')}",
                        } : null,
                        Saturday = entry.SaturdayOpen.HasValue ? new OpeningHoursDto()
                        {
                            Open = $"{entry.SaturdayOpen?.Hours.ToString().PadLeft(2, '0')}:{entry.SaturdayOpen?.Minutes.ToString().PadLeft(2, '0')}",
                            Close = $"{entry.SaturdayClose?.Hours.ToString().PadLeft(2, '0')}:{entry.SaturdayClose?.Minutes.ToString().PadLeft(2, '0')}",
                        } : null,
                        Sunday = entry.SundayOpen.HasValue ? new OpeningHoursDto()
                        {
                            Open = $"{entry.SundayOpen?.Hours.ToString().PadLeft(2, '0')}:{entry.SundayOpen?.Minutes.ToString().PadLeft(2, '0')}",
                            Close = $"{entry.SundayClose?.Hours.ToString().PadLeft(2, '0')}:{entry.SundayClose?.Minutes.ToString().PadLeft(2, '0')}",
                        } : null,
                    },
                };
            }
        }

        private record RestaurantEntry
        {
            public Guid Id { get; init; }
            public Guid ManagerId { get; init; }
            public string Name { get; init; }
            public string PhoneNumber { get; init; }
            public string Address { get; init; }
            public float Latitude { get; init; }
            public float Longitude { get; init; }
            public string Status { get; init; }
            public TimeSpan? MondayOpen { get; init; }
            public TimeSpan? MondayClose { get; init; }
            public TimeSpan? TuesdayOpen { get; init; }
            public TimeSpan? TuesdayClose { get; init; }
            public TimeSpan? WednesdayOpen { get; init; }
            public TimeSpan? WednesdayClose { get; init; }
            public TimeSpan? ThursdayOpen { get; init; }
            public TimeSpan? ThursdayClose { get; init; }
            public TimeSpan? FridayOpen { get; init; }
            public TimeSpan? FridayClose { get; init; }
            public TimeSpan? SaturdayOpen { get; init; }
            public TimeSpan? SaturdayClose { get; init; }
            public TimeSpan? SundayOpen { get; init; }
            public TimeSpan? SundayClose { get; init; }
        }
    }
}
