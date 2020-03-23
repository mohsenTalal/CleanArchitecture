using Microsoft.AspNetCore.Builder;

namespace EnterpriseApplicationIntegration.API.Middlewares
{
    /// <summary>
    /// Extension method used to add the middle wares to the HTTP request pipeline.
    /// </summary>
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Uses the exception middleware.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }

        /// <summary>
        /// Uses the logging middleware.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }

        public static IApplicationBuilder UseRequestCultureMiddleware(this IApplicationBuilder builder)
        {
            // SetCurrentCulture
            return builder.UseMiddleware<LocalizationMiddleware>();
        }
    }
}