using System;
using FoodSnap.Application;
using FoodSnap.Web.ErrorPresenters;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions
{
    public abstract class Presenter<TRequest, TResponse> : IPresenter<TRequest, TResponse>
        where TResponse : Result
    {
        private readonly IErrorPresenter errorPresenter;

        public Presenter(IErrorPresenter errorPresenter)
        {
            this.errorPresenter = errorPresenter;
        }

        public IActionResult Present(TResponse response)
        {
            if (response.IsSuccess)
            {
                return PresentSuccess(response);
            }

            return PresentError(response);
        }

        protected abstract IActionResult PresentSuccess(TResponse response);

        private IActionResult PresentError(TResponse response)
        {
            return errorPresenter.Present(response.Error);
        }
    }
}
