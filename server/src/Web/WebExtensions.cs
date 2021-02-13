using Microsoft.Extensions.Hosting;

namespace Web
{
    public static class WebExtensions
    {
        public static bool IsTesting(this IHostEnvironment env)
        {
            return env.IsEnvironment("Testing");
        }
    }
}
