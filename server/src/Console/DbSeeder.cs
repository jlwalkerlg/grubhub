using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using Web.Data;
using Web.Data.EF;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Cuisines;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Services.Hashing;

namespace Console
{
    public class DbSeeder
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly AppDbContext context;
        private readonly IHasher hasher;

        public DbSeeder(
            IDbConnectionFactory dbConnectionFactory,
            AppDbContext context,
            IHasher hasher)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.context = context;
            this.hasher = hasher;
        }

        public async Task Seed()
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            using (var connection = await dbConnectionFactory.OpenConnection())
            {
                var sql = await File.ReadAllTextAsync("quartz.sql");
                var command = new NpgsqlCommand(sql, (NpgsqlConnection)connection);
                command.ExecuteNonQuery();
            }

            var json = await File.ReadAllTextAsync("seed.json");
            var jdoc = JsonDocument.Parse(json);

            var cuisines = jdoc.RootElement
                .GetProperty("cuisines")
                .EnumerateArray()
                .Select(x => new Cuisine(x.GetString()));

            await context.AddRangeAsync(cuisines);

            foreach (var userEl in jdoc.RootElement.GetProperty("users").EnumerateArray())
            {
                var role = Enum.Parse<UserRole>(userEl.GetProperty("role").GetString());

                if (role == UserRole.Customer)
                {
                    var customer = new Customer(
                        new UserId(new Guid(userEl.GetProperty("id").GetString())),
                        userEl.GetProperty("firstName").GetString(),
                        userEl.GetProperty("lastName").GetString(),
                        new Email(userEl.GetProperty("email").GetString()),
                        hasher.Hash(userEl.GetProperty("password").GetString())
                    );

                    await context.AddAsync(customer);
                }

                if (role == UserRole.RestaurantManager)
                {
                    var manager = new RestaurantManager(
                        new UserId(new Guid(userEl.GetProperty("id").GetString())),
                        userEl.GetProperty("firstName").GetString(),
                        userEl.GetProperty("lastName").GetString(),
                        new Email(userEl.GetProperty("email").GetString()),
                        hasher.Hash(userEl.GetProperty("password").GetString())
                    );

                    var restaurantEl = userEl.GetProperty("restaurant");

                    var restaurant = new Restaurant(
                        new RestaurantId(new Guid(restaurantEl.GetProperty("id").GetString())),
                        manager.Id,
                        restaurantEl.GetProperty("name").GetString(),
                        new PhoneNumber(restaurantEl.GetProperty("phone_number").GetString()),
                        new Address(
                            restaurantEl.GetProperty("addressLine1").GetString(),
                            restaurantEl.GetProperty("addressLine2").GetString(),
                            restaurantEl.GetProperty("addressCity").GetString(),
                            new Postcode(restaurantEl.GetProperty("addressPostcode").GetString())),
                        new Coordinates(
                            (float)restaurantEl.GetProperty("latitude").GetDouble(),
                            (float)restaurantEl.GetProperty("longitude").GetDouble()
                        )
                    );

                    restaurant.MaxDeliveryDistance = Distance.FromKm(restaurantEl.GetProperty("max_delivery_distance").GetInt32());
                    restaurant.EstimatedDeliveryTimeInMinutes =
                        restaurantEl.GetProperty("estimated_delivery_time_in_minutes").GetInt32();
                    restaurant.MinimumDeliverySpend = Money.FromPounds(restaurantEl.GetProperty("minimum_delivery_spend").GetDecimal());
                    restaurant.DeliveryFee = Money.FromPounds(restaurantEl.GetProperty("delivery_fee").GetDecimal());

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
                                Money.FromPounds(itemEl.GetProperty("price").GetDecimal())
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

                    if (restaurantEl.TryGetProperty("billing_account", out var accountEl))
                    {
                        var account = new BillingAccount(
                            new BillingAccountId(accountEl.GetProperty("id").GetString()),
                            restaurant.Id
                        );

                        if (accountEl.GetProperty("enabled").GetBoolean())
                        {
                            account.Enable();
                        }

                        await context.AddAsync(account);
                    }

                    await context.AddRangeAsync(
                        manager,
                        restaurant,
                        menu);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
