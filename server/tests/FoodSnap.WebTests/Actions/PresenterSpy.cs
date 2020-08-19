using FoodSnap.Web.Actions;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.WebTests.Actions
{
    public class PresenterSpy<TRequest, TResult> : IPresenter<TRequest, TResult>
    {
        public IActionResult Result { get; set; } = new ObjectResult(null);

        public IActionResult Present(TResult result)
        {
            return Result;
        }
    }
}
