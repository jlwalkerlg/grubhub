using System;
using System.Threading.Tasks;
using Application.Restaurants.RegisterRestaurant;
using Application.Services.Hashing;
using Domain;
using Domain.Menus;
using Domain.Restaurants;
using Domain.Users;

namespace Infrastructure.Persistence.EF
{
    public class DbSeeder
    {
        private readonly AppDbContext context;
        private readonly IHasher hasher;

        public DbSeeder(AppDbContext context, IHasher hasher)
        {
            this.context = context;
            this.hasher = hasher;
        }

        public async Task Seed()
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                hasher.Hash("password123")
            );

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("19 Bodmin Ave, Shipley BD18 1LT, UK"),
                new Coordinates(53.830975f, -1.75002f)
            );
            restaurant.Approve();
            restaurant.DeliveryFee = new Money(1.50m);
            restaurant.EstimatedDeliveryTimeInMinutes = 40;
            restaurant.MaxDeliveryDistanceInKm = 5;
            restaurant.MinimumDeliverySpend = new Money(10.00m);
            restaurant.OpeningTimes = OpeningTimes.FromDays(new()
            {
                {
                    DayOfWeek.Monday,
                    new OpeningHours(TimeSpan.Zero, new TimeSpan(23, 45, 0))
                },
                {
                    DayOfWeek.Tuesday,
                    new OpeningHours(TimeSpan.Zero, new TimeSpan(23, 45, 0))
                },
                {
                    DayOfWeek.Wednesday,
                    new OpeningHours(TimeSpan.Zero, new TimeSpan(23, 45, 0))
                },
                {
                    DayOfWeek.Thursday,
                    new OpeningHours(TimeSpan.Zero, new TimeSpan(23, 45, 0))
                },
                {
                    DayOfWeek.Friday,
                    new OpeningHours(TimeSpan.Zero, new TimeSpan(23, 45, 0))
                },
                {
                    DayOfWeek.Saturday,
                    new OpeningHours(TimeSpan.Zero, new TimeSpan(23, 45, 0))
                },
                {
                    DayOfWeek.Sunday,
                    new OpeningHours(TimeSpan.Zero, new TimeSpan(23, 45, 0))
                },
            });

            var menu = new Menu(restaurant.Id);
            menu.AddCategory("Pizza");
            menu.GetCategory("Pizza")
                .AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            var eventDto = new EventDto(
                new RestaurantRegisteredEvent(restaurant.Id, manager.Id));

            await context.AddRangeAsync(manager, restaurant, menu, eventDto);
            await context.SaveChangesAsync();
        }
    }
}
