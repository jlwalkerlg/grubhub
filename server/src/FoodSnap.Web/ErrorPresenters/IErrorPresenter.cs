using FoodSnap.Application;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.ErrorPresenters
{
    public interface IErrorPresenter
    {
        IActionResult Present(IError error);
    }
}
