using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.API.Middlewares
{
    /// <summary>
    /// Localization middle ware.
    /// </summary>
    public class LocalizationMiddleware
    {
        private const string enUSCulture = "en-US";
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var langs = context.Request.Headers["Accept-Language"].ToString();
                var firstLang = langs.Split(',').FirstOrDefault();
                var defaultLang = string.IsNullOrEmpty(firstLang) ? enUSCulture : firstLang;
                CultureInfo.CurrentCulture = new CultureInfo(defaultLang);
                CultureInfo.CurrentUICulture = new CultureInfo(defaultLang);
                // var culture = $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";// rqf.RequestCulture.Culture;

                await _next(context);
            }
            catch
            {
                CultureInfo.CurrentCulture = new CultureInfo(enUSCulture);
            }
        }
    }
}