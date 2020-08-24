using FoodSnap.Application;
using FoodSnap.Web.ErrorPresenters;

namespace FoodSnap.WebTests.ErrorPresenters
{
    public class ErrorPresenterFactoryStub : IErrorPresenterFactory
    {
        public ErrorPresenterSpy ErrorPresenterSpy { get; } = new ErrorPresenterSpy();

        public IErrorPresenter Make(Error error)
        {
            return ErrorPresenterSpy;
        }
    }
}
