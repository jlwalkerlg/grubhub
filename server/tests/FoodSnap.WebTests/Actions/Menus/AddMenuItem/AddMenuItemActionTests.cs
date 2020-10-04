using System;
using System.Threading.Tasks;
using FoodSnap.Domain;
using FoodSnap.Web.Actions.Menus.AddMenuItem;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Menus.AddMenuItem
{
    public class AddMenuItemActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly AddMenuItemAction action;

        public AddMenuItemActionTests()
        {
            mediatorSpy = new MediatorSpy();

            action = new AddMenuItemAction(mediatorSpy);
        }

        [Fact]
        public async Task It_Returns_201_On_Success()
        {
            var menuId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var request = new AddMenuItemRequest
            {
                Category = "Pizza",
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m
            };

            mediatorSpy.Result = Result.Ok(Guid.NewGuid());

            var response = await action.Execute(menuId, request) as StatusCodeResult;

            Assert.Equal(201, response.StatusCode);
        }
    }
}
