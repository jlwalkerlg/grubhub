using FoodSnap.Application;
using FoodSnap.Application.Validation;

namespace FoodSnap.Web.ErrorPresenters
{
    public class ErrorPresenterFactory : IErrorPresenterFactory
    {
        public IErrorPresenter Make(IError error)
        {
            if (error is ValidationError)
            {
                return new ValidationErrorPresenter();
            }

            return new DefaultErrorPresenter();
        }
    }
}
