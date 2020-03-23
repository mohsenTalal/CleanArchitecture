using EnterpriseApplicationIntegration.Core;
using EnterpriseApplicationIntegration.Core.HTTP;
using EnterpriseApplicationIntegration.Core.LoggerBuilder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.API.Middlewares
{
    /// <summary>
    /// Exception middle ware.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private AppLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="log">The log object</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext, AppLog log)
        {
            try
            {
                _log = log;
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.Headers.Clear();
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string code, message;
            HttpStatusCode httpStatusCode;
            _log.ErrorDescription = exception.ToString();
            Console.Write(exception);

            switch (exception)
            {
                case APIException ex:
                    code = ex.ErrorCode;
                    message = ex.Message;
                    httpStatusCode = ex.HttpStatusCode;
                    break;

                case UnauthorizedAccessException ex:
                    code = ErrorCodes.Unauthorized;
                    message = ex.Message;
                    httpStatusCode = HttpStatusCode.Unauthorized;
                    break;

                case BusinessException ex:
                    code = ErrorCodes.Other((HttpStatusCode)ex.Status);
                    message = ex.Title ?? ex.Message;
                    httpStatusCode = (HttpStatusCode)ex.Status;
                    break;

                default:
                    code = ErrorCodes.InternalServerError;
                    message = "Internal Server Error!";
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonConvert.SerializeObject(new ErrorResponse
            {
                Code = code,
                Message = message,
                ReferenceId = _log.ReferenceNumber
            });

            context.Response.ContentType = @"application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            await context.Response.WriteAsync(result);
        }
    }
}