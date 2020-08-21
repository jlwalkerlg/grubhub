using FoodSnap.Application;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.ErrorPresenters
{
    public class DefaultErrorPresenter : ErrorPresenter<Error>
    {
        protected override IActionResult PresentError(Error error)
        {
            return new StatusCodeResult(500);
        }
    }
}
