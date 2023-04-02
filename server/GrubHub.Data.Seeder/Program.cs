using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Web;
using Web.Data.EF;
using Web.Domain;
using Web.Domain.Billing;
using Web.Domain.Cuisines;
using Web.Domain.Menus;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Services.Hashing;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((context, builder) =>
    {
        var assembly = Assembly.Load(new AssemblyName(context.HostingEnvironment.ApplicationName));
        builder.AddUserSecrets(assembly);
    })
    .ConfigureServices((context, services) =>
    {
        var databaseSettings = context.Configuration.GetSection("Database").Get<DatabaseSettings>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(databaseSettings.ConnectionString, b =>
            {
                b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });

        services.AddSingleton<IHasher, Hasher>();
    }).Build();

var hasher = host.Services.GetRequiredService<IHasher>();

var random = new Random();

using var scope = host.Services.CreateScope();

await using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

var demoCustomer = await db.Users.FirstOrDefaultAsync(u => u.Email == new Email("demo@customer.com"));
if (demoCustomer is null)
{
    demoCustomer ??= new Customer(
        new UserId(Guid.NewGuid()),
        "Demo",
        "Customer",
        new Email("demo@customer.com"),
        hasher.Hash("password123"));

    await db.Users.AddAsync(demoCustomer);
}
demoCustomer.MobileNumber = new MobileNumber("+447123456789");
demoCustomer.DeliveryAddress = new Address(
    "Lilliana Park",
    null,
    "West Alycestad",
    new Postcode("BD18 1AA"));

var demoManager = await db.Users.FirstOrDefaultAsync(u => u.Email == new Email("demo@manager.com"));
if (demoManager is null)
{
    demoManager ??= new RestaurantManager(
        new UserId(Guid.NewGuid()),
        "Demo",
        "Manager",
        new Email("demo@manager.com"),
        hasher.Hash("password123"));

    await db.Users.AddAsync(demoManager);
}
demoManager.MobileNumber = new MobileNumber("+447123456789");
demoManager.DeliveryAddress = new Address(
    "282 King's Rd",
    null,
    "Bradford",
    new Postcode("BD2 1JP"));

var restaurant = await db.Restaurants.Include(r => r.Cuisines).FirstOrDefaultAsync(r => r.ManagerId == demoManager.Id);
if (restaurant is null)
{
    restaurant ??= new Restaurant(
        new RestaurantId(Guid.NewGuid()),
        demoManager.Id,
        "Demo Restaurant",
        new PhoneNumber("68552 273322"),
        new Address(
            "Krista Station",
            null,
            "New Carsonchester",
            new Postcode("BD18 1AA")),
        new Coordinates(53.8283f, -1.76106f));

    await db.Restaurants.AddAsync(restaurant);
}
restaurant.Description = "Demo Restaurant Description";
restaurant.OpeningTimes = new OpeningTimes
{
    Monday = new OpeningHours(TimeSpan.FromHours(6)),
    Tuesday = new OpeningHours(TimeSpan.FromHours(6)),
    Wednesday = new OpeningHours(TimeSpan.FromHours(6)),
    Thursday = new OpeningHours(TimeSpan.FromHours(6)),
    Friday = new OpeningHours(TimeSpan.FromHours(6)),
    Saturday = new OpeningHours(TimeSpan.FromHours(6)),
    Sunday = new OpeningHours(TimeSpan.FromHours(6)),
};
restaurant.MinimumDeliverySpend = Money.FromPence(700);
restaurant.DeliveryFee = Money.FromPence(101);
restaurant.MaxDeliveryDistance = Distance.FromKm(1500);
restaurant.EstimatedDeliveryTimeInMinutes = 25;
restaurant.Approve();

var cuisines = await db.Cuisines.ToListAsync();

if (cuisines.Count < 14)
{
    var categories = await GetCategories();

    foreach (var category in categories)
    {
        var cuisine = await db.Cuisines.FindAsync(category.Name);

        if (cuisine is null)
        {
            cuisine = new Cuisine(category.Name);
            await db.Cuisines.AddAsync(cuisine);
            cuisines.Add(cuisine);
        }
    }
}

if (restaurant.Cuisines.Count is not 8)
{
    var restaurantCuisines = cuisines.OrderBy(c => random.Next()).Take(8).ToArray();
    restaurant.SetCuisines(restaurantCuisines);
}

var menu = await db.Menus.Include(m => m.Categories).ThenInclude(mc => mc.Items).FirstOrDefaultAsync(m => m.RestaurantId == restaurant.Id);
if (menu is null)
{
    menu = new Menu(restaurant.Id);
    await db.Menus.AddAsync(menu);
}

while (menu.Categories.Count < 3)
{
    var cuisine = cuisines.OrderBy(c => random.Next()).First();
    menu.AddCategory(Guid.NewGuid(), cuisine.Name);
}

foreach (var category in menu.Categories)
{
    if (category.Items.Any()) continue;

    var meals = await GetMeals(category.Name);

    foreach (var meal in meals)
    {
        category.AddItem(Guid.NewGuid(), meal.Name, null, Money.FromPence(random.Next(500, 1100)));
    }
}

if (restaurant.BillingAccountId is null)
{
    restaurant.AddBillingAccount(new BillingAccountId("acct_1IIDXKPRU0NZyTXU"));
}

var billingAccount = await db.BillingAccounts.FindAsync(restaurant.BillingAccountId);
if (billingAccount is null)
{
    billingAccount = new BillingAccount(restaurant.BillingAccountId);
    await db.BillingAccounts.AddAsync(billingAccount);
}
billingAccount.Enable();

await db.SaveChangesAsync();

static async Task<GetCategoriesResponse.Category[]> GetCategories()
{
    using var client = new HttpClient();
    var response = await client.GetAsync("https://www.themealdb.com/api/json/v1/1/categories.php");
    var json = await response.Content.ReadAsStringAsync();
    var data = JsonSerializer.Deserialize<GetCategoriesResponse>(json)!;
    return data.Categories;
}

static async Task<GetMealsResponse.Meal[]> GetMeals(string category)
{
    using var client = new HttpClient();
    var response = await client.GetAsync($"https://www.themealdb.com/api/json/v1/1/filter.php?c={category}");
    var json = await response.Content.ReadAsStringAsync();
    var data = JsonSerializer.Deserialize<GetMealsResponse>(json)!;
    return data.Meals;
}

class GetCategoriesResponse
{
    [JsonPropertyName("categories")]
    public Category[] Categories { get; set; } = null!;

    public class Category
    {
        [JsonPropertyName("strCategory")]
        public string Name { get; set; } = null!;
    }
}

class GetMealsResponse
{
    [JsonPropertyName("meals")]
    public Meal[] Meals { get; set; } = null!;

    public class Meal
    {
        [JsonPropertyName("strMeal")]
        public string Name { get; set; } = null!;
    }
}
