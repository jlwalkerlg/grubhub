using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Domain;
using Web.Domain.Cuisines;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Restaurants.RegisterRestaurant;
using Web.Services.Hashing;

namespace Console
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

            var json = await File.ReadAllTextAsync("seed.json");
            var jdoc = JsonDocument.Parse(json);

            var cuisines = jdoc.RootElement
                .GetProperty("cuisines")
                .EnumerateArray()
                .Select(x => new Cuisine(x.GetString()));

            await context.AddRangeAsync(cuisines);

            foreach (var userEl in jdoc.RootElement.GetProperty("users").EnumerateArray())
            {
                var role = userEl.GetProperty("role").GetString();

                if (role == "RestaurantManager")
                {
                    var user = new RestaurantManager(
                        new UserId(new Guid(userEl.GetProperty("id").GetString())),
                        userEl.GetProperty("name").GetString(),
                        new Email(userEl.GetProperty("email").GetString()),
                        hasher.Hash("password123")
                    );

                    var restaurantEl = userEl.GetProperty("restaurant");

                    var restaurant = new Restaurant(
                        new RestaurantId(new Guid(restaurantEl.GetProperty("id").GetString())),
                        user.Id,
                        restaurantEl.GetProperty("name").GetString(),
                        new PhoneNumber(restaurantEl.GetProperty("phone_number").GetString()),
                        new Address(restaurantEl.GetProperty("address").GetString()),
                        new Coordinates(
                            (float)restaurantEl.GetProperty("latitude").GetDouble(),
                            (float)restaurantEl.GetProperty("longitude").GetDouble()
                        )
                    );

                    restaurant.MaxDeliveryDistanceInKm = restaurantEl.GetProperty("max_delivery_distance_in_km").GetInt32();
                    restaurant.EstimatedDeliveryTimeInMinutes =
                        restaurantEl.GetProperty("estimated_delivery_time_in_minutes").GetInt32();
                    restaurant.MinimumDeliverySpend = new Money(restaurantEl.GetProperty("minimum_delivery_spend").GetDecimal());
                    restaurant.DeliveryFee = new Money(restaurantEl.GetProperty("delivery_fee").GetDecimal());

                    if (restaurantEl.GetProperty("status").GetString() == "Approved")
                    {
                        restaurant.Approve();
                    }

                    restaurant.OpeningTimes = new OpeningTimes()
                    {
                        Monday = OpeningHours.Parse(
                            restaurantEl.GetProperty("monday_open").GetString(),
                            restaurantEl.GetProperty("monday_close").GetString()
                        ),
                        Tuesday = OpeningHours.Parse(
                            restaurantEl.GetProperty("tuesday_open").GetString(),
                            restaurantEl.GetProperty("tuesday_close").GetString()
                        ),
                        Wednesday = OpeningHours.Parse(
                            restaurantEl.GetProperty("wednesday_open").GetString(),
                            restaurantEl.GetProperty("wednesday_close").GetString()
                        ),
                        Thursday = OpeningHours.Parse(
                            restaurantEl.GetProperty("thursday_open").GetString(),
                            restaurantEl.GetProperty("thursday_close").GetString()
                        ),
                        Friday = OpeningHours.Parse(
                            restaurantEl.GetProperty("friday_open").GetString(),
                            restaurantEl.GetProperty("friday_close").GetString()
                        ),
                        Saturday = OpeningHours.Parse(
                            restaurantEl.GetProperty("saturday_open").GetString(),
                            restaurantEl.GetProperty("saturday_close").GetString()
                        ),
                        Sunday = OpeningHours.Parse(
                            restaurantEl.GetProperty("sunday_open").GetString(),
                            restaurantEl.GetProperty("sunday_close").GetString()
                        ),
                    };

                    var menuEl = restaurantEl.GetProperty("menu");

                    var menu = new Menu(restaurant.Id);

                    foreach (var categoryEl in menuEl.GetProperty("categories").EnumerateArray())
                    {
                        var categoryName = categoryEl.GetProperty("name").GetString();

                        var category = menu.AddCategory(Guid.NewGuid(), categoryName).Value;

                        foreach (var itemEl in categoryEl.GetProperty("items").EnumerateArray())
                        {
                            category.AddItem(
                                Guid.NewGuid(),
                                itemEl.GetProperty("name").GetString(),
                                itemEl.GetProperty("description").GetString(),
                                new Money(itemEl.GetProperty("price").GetDecimal())
                            );
                        }
                    }

                    foreach (var cuisineEl in restaurantEl.GetProperty("cuisines").EnumerateArray())
                    {
                        var restaurantCuisine = new RestaurantCuisine()
                        {
                            RestaurantId = restaurant.Id,
                            CuisineName = cuisineEl.GetString(),
                        };

                        await context.AddAsync(restaurantCuisine);
                    }

                    var eventDto = new EventDto(
                        new RestaurantRegisteredEvent(restaurant.Id, user.Id, DateTime.UtcNow)
                    );

                    await context.AddRangeAsync(
                        user,
                        restaurant,
                        menu,
                        eventDto);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
