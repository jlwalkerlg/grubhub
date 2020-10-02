using System;
using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Web.Actions.Menus.AddMenuItem;
using FoodSnap.Web.Envelopes;
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
                Name = "Margherita",
                Description = "Cheese & tomato",
                Price = 9.99m
            };

            mediatorSpy.Result = Result.Ok(Guid.NewGuid());

            var response = await action.Execute(menuId, categoryId, request) as ObjectResult;

            Assert.Equal(201, response.StatusCode);

            var envelope = response.Value as DataEnvelope;

            Assert.IsType<Guid>(envelope.Data);
            Assert.NotEqual(default(Guid), envelope.Data);
        }
    }
}
