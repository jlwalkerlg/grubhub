using FoodSnap.Application;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.ErrorPresenters
{
    public abstract class ErrorPresenter<TError> : IErrorPresenter
        where TError : Error
    {
        public IActionResult Present(Error error)
        {
            return PresentError((TError)error);
        }

        protected abstract IActionResult PresentError(TError error);
    }
}
