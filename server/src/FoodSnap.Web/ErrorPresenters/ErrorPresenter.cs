using FoodSnap.Application;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.ErrorPresenters
{
    public abstract class ErrorPresenter<TError> : IErrorPresenter
        where TError : IError
    {
        public IActionResult Present(IError error)
        {
            return PresentError((TError)error);
        }

        protected abstract IActionResult PresentError(TError error);
    }
}
