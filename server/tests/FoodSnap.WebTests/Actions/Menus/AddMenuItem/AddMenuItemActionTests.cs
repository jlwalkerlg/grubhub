using System;
using System.Threading.Tasks;
using FoodSnap.Shared;
using FoodSnap.Web.Actions.Menus.AddMenuItem;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Menus.AddMenuItem
{
    public class AddMenuItemActionTests
    {
        private readonly SenderSpy senderSpy;
        private readonly AddMenuItemAction action;

        public AddMenuItemActionTests()
        {
            senderSpy = new SenderSpy();

            action = new AddMenuItemAction(senderSpy);
        }

        [Fact]
        public async Task It_Returns_201_On_Success()
        {
            var menuId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var request = new AddMenuItemRequest
            {
                CategoryName = "Pizza",
                ItemName = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m
            };

            senderSpy.Result = Result.Ok(Guid.NewGuid());

            var response = await action.Execute(menuId, request) as StatusCodeResult;

            Assert.Equal(201, response.StatusCode);
        }
    }
}
