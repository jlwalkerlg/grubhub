using FoodSnap.Application;

namespace FoodSnap.Web.ErrorPresenters
{
    public interface IErrorPresenterFactory
    {
        IErrorPresenter Make(IError error);
    }
}
