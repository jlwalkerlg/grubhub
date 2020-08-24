using FoodSnap.Application;
using FoodSnap.Application.Services.Geocoding;
using FoodSnap.Application.Validation;

namespace FoodSnap.Web.ErrorPresenters
{
    public class ErrorPresenterFactory : IErrorPresenterFactory
    {
        public IErrorPresenter Make(Error error)
        {
            if (error is ValidationError)
            {
                return new ValidationErrorPresenter();
            }

            if (error is GeocodingError)
            {
                return new GeocodingErrorPresenter();
            }

            return new DefaultErrorPresenter();
        }
    }
}
