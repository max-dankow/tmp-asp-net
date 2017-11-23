using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DemoWebApplication
{
    public static class LangExtension
    {
        public static CultureInfo GetMyCulture(this HttpContext context)
        {
            return context.Items[CultureMiddleware.LangItemsKey] as CultureInfo;
        }
    }
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _culturePrameterName;
        private readonly ILogger<CultureMiddleware> _logger;
        public static readonly object LangItemsKey = new object(); // Don't know exactly how to work with keys properly.

        public CultureMiddleware(RequestDelegate next, ILogger<CultureMiddleware> logger,
            string cultureParameterName = "lang")
        {
            _next = next;
            _logger = logger;
            _culturePrameterName = cultureParameterName;
        }

        public Task Invoke(HttpContext context)
        {
            string cultureName = context.Request.Query[_culturePrameterName];
            if (!string.IsNullOrEmpty(cultureName))
            {
                try
                {
                    var newCulture = new CultureInfo(cultureName);
                    CultureInfo.CurrentCulture = newCulture;
                    CultureInfo.CurrentUICulture = newCulture;
                    context.Items[LangItemsKey] = newCulture;
                    _logger.LogInformation("Set request culture {0}", newCulture.Name);
                }
                catch (CultureNotFoundException e)
                {
                    _logger.LogWarning("Culture {0} not found", e, cultureName);
                }

            }
            return _next.Invoke(context);
        }
    }
}