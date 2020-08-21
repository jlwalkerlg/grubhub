using FoodSnap.Application;
using FoodSnap.Web.Actions.Restaurants.RegisterRestaurant;
using FoodSnap.WebTests.ErrorPresenters;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantPresenterTests
    {
        private readonly ErrorPresenterSpy errorPresenterSpy;
        private readonly RegisterRestaurantPresenter presenter;

        public RegisterRestaurantPresenterTests()
        {
            var errorPresenterFactoryStub = new ErrorPresenterFactoryStub();
            errorPresenterSpy = errorPresenterFactoryStub.ErrorPresenterSpy;

            presenter = new RegisterRestaurantPresenter(errorPresenterFactoryStub);
        }

        [Fact]
        public void It_Returns_A_201_Result_On_Success()
        {
            var result = presenter.Present(Result.Ok()) as StatusCodeResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void It_Returns_The_Error_Presenter_Result_On_Fail()
        {
            var result = presenter.Present(Result.Fail(new Error()));

            Assert.Same(errorPresenterSpy.Result, result);
        }
    }
}
