using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;

namespace Web.Filters
{
    public class CacheControlAttribute : ActionFilterAttribute
    {
        public int Duration { get; set; } = 0;

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (ResultIsSuccess(context.Result))
            {
                SetCacheControlHeaders(context.HttpContext.Response);
            }
        }

        private bool ResultIsSuccess(IActionResult result)
        {
            return result is IStatusCodeActionResult statusCodeActionResult && statusCodeActionResult.StatusCode is >= 200 and < 300;
        }

        private void SetCacheControlHeaders(HttpResponse response)
        {
            response.Headers[HeaderNames.CacheControl] = $"public,max-age={Duration}";
        }
    }
}
