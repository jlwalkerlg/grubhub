using FoodSnap.Application;
using FoodSnap.Web.ErrorPresenters;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.ErrorPresenters
{
    public class DefaultErrorPresenterTests
    {
        private readonly DefaultErrorPresenter presenter;

        public DefaultErrorPresenterTests()
        {
            presenter = new DefaultErrorPresenter();
        }

        [Fact]
        public void It_Returns_A_500_Response()
        {
            var error = new Error();
            var result = presenter.Present(error) as StatusCodeResult;

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }
    }
}
