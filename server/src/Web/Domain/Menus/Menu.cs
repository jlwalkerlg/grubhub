using System;
using System.Collections.Generic;
using System.Linq;
using Web.Domain.Restaurants;

namespace Web.Domain.Menus
{
    public class Menu : Entity<Menu>
    {
        private readonly List<MenuCategory> categories = new();

        public Menu(RestaurantId restaurantId)
        {
            if (restaurantId == null)
            {
                throw new ArgumentNullException(nameof(restaurantId));
            }

            RestaurantId = restaurantId;
        }

        private Menu() { } // EF Core

        public RestaurantId RestaurantId { get; }

        public IReadOnlyList<MenuCategory> Categories => categories;

        public MenuCategory GetCategoryById(Guid id)
        {
            return categories.SingleOrDefault(x => x.Id == id);
        }

        public Result<MenuCategory> AddCategory(Guid id, string name)
        {
            if (categories.Any(x => x.Name == name))
            {
                return Error.BadRequest("Category already exists.");
            }

            var category = new MenuCategory(id, name);

            categories.Add(category);

            return Result.Ok(category);
        }

        public Result RemoveCategory(Guid id)
        {
            var category = categories.SingleOrDefault(x => x.Id == id);

            if (category == null)
            {
                return Error.NotFound("Category doesn't exist.");
            }

            categories.Remove(category);

            return Result.Ok();
        }

        public Result RenameCategory(Guid id, string newName)
        {
            var category = categories.SingleOrDefault(x => x.Id == id);

            if (category == null)
            {
                return Error.NotFound("Category not found.");
            }

            if (newName != category.Name && categories.Any(x => x.Name == newName))
            {
                return Error.BadRequest("Category already exists.");
            }

            category.Name = newName;

            return Result.Ok();
        }

        protected override bool IdentityEquals(Menu other)
        {
            return RestaurantId == other.RestaurantId;
        }

        public override int GetHashCode()
        {
            return RestaurantId.GetHashCode();
        }
    }
}
