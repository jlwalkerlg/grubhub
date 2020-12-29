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

            User user = new RestaurantManager(
                new UserId(new Guid("979a79d6-7b7c-4c21-88c9-8f918be90d01")),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                hasher.Hash("jordan123")
            );

            var restaurant = new Restaurant(
                new RestaurantId(new Guid("015caf13-8252-476b-9e7f-c43767998c01")),
                user.Id,
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
                .AddItem(
                    "Margherita",
                    "Cheese & tomato",
                    new Money(9.99m));
            menu.GetCategory("Pizza")
                .AddItem(
                    "Hawaiian",
                    "Ham & pineapple",
                    new Money(11.99m));

            var eventDto = new EventDto(
                new RestaurantRegisteredEvent(
                    new RestaurantId(new Guid("979a79d6-7b7c-4c21-88c9-8f918be90d01")),
                    new UserId(new Guid("015caf13-8252-476b-9e7f-c43767998c01"))
                )
            );

            await context.AddRangeAsync(user, restaurant, menu, eventDto);

            user = new RestaurantManager(
                new UserId(new Guid("979a79d6-7b7c-4c21-88c9-8f918be90d02")),
                "Bruno",
                new Email("bruno@gmail.com"),
                hasher.Hash("bruno123")
            );

            restaurant = new Restaurant(
                new RestaurantId(new Guid("015caf13-8252-476b-9e7f-c43767998c02")),
                user.Id,
                "Bruno's Kitchen",
                new PhoneNumber("01234567890"),
                new Address("27 Lavell Mews, Bradford BD2 3HW, UK"),
                new Coordinates(53.8145456f, -1.7230635f)
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

            menu = new Menu(restaurant.Id);
            menu.AddCategory("Pizza");
            menu.GetCategory("Pizza")
                .AddItem("Margherita", "Cheese & tomato", new Money(9.99m));

            eventDto = new EventDto(
                new RestaurantRegisteredEvent(
                    new RestaurantId(new Guid("979a79d6-7b7c-4c21-88c9-8f918be90d02")),
                    new UserId(new Guid("015caf13-8252-476b-9e7f-c43767998c02"))
                )
            );

            await context.AddRangeAsync(user, restaurant, menu, eventDto);

            await context.SaveChangesAsync();
        }
    }
}
