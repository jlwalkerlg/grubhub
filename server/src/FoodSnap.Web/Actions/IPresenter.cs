using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions
{
    public interface IPresenter<TRequest, TResponse>
    {
        IActionResult Present(TResponse response);
    }
}
