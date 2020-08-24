using FoodSnap.Application;
using FoodSnap.Web.ErrorPresenters;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.WebTests.ErrorPresenters
{
    public class ErrorPresenterSpy : IErrorPresenter
    {
        public IActionResult Result { get; set; } = new StatusCodeResult(500);

        public IActionResult Present(Error error)
        {
            return Result;
        }
    }
}
