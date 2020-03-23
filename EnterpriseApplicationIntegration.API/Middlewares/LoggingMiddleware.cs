using EnterpriseApplicationIntegration.Core.LoggerBuilder;
using EnterpriseApplicationIntegration.Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.API.Middlewares
{
    /// <summary>
    /// Logging middleware.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="settings">The settings.</param>
        public LoggingMiddleware(RequestDelegate next, IOptions<AppSettings> settings)
        {
            _next = next;
            _settings = settings.Value;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="appLogger">Application logger instance</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IAppLogger appLogger, AppLog log)
        {
            if (IsAssetRequest(context.Request.Path.Value))
            {
                await _next(context);
                return;
            }
            var stopWatch = Stopwatch.StartNew();

            log.Request = GetBodyString(context.Request);
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                log.ApplicationVersion = _settings.AppVersion;
                log.DeviceInfo = context.Request.Headers["DeviceInfo"].FirstOrDefault();
                log.DeviceIP = context.Request.Headers["DeviceIP"].FirstOrDefault();
                log.OSType = context.Request.Headers["OSType"].FirstOrDefault();
                log.HostIP = GetIpAddress();
                log.RequestMethod = context.Request.Method;
                log.RequestURL = context.Request.Path;
                log.RequestHeaders = JsonConvert.SerializeObject(context.Request.Headers);
                await _next(context);
                var response = await FormatResponse(context.Response);
                log.HttpStatus = context.Response.StatusCode;
                log.Response = log.HttpStatus == 200 ? "Success" : response;
                log.ResponseTime = stopWatch.Elapsed.Milliseconds;
                context.Response.Headers.Add("Reference", log.ReferenceNumber.ToString());
                await appLogger.Commit(log);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private string GetBodyString(HttpRequest request)
        {
            var bodyStr = "";

            // Allows using several time the stream in ASP.Net Core
            //request.EnableRewind();

            // Arguments: Stream, Encoding, detect encoding, buffer size
            // AND, the most important: keep stream opened
            using (StreamReader reader
                      = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = reader.ReadToEnd();
            }

            // Rewind, so the core is not lost when it looks the body for the request
            request.Body.Position = 0;

            return string.IsNullOrEmpty(bodyStr) ? $"{request.Path}{request.QueryString}" : bodyStr;
        }

        private bool IsAssetRequest(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            return path.Contains('.') || path.Contains("swagger");
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }

        private string GetIpAddress()
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());

            return heserver.AddressList[heserver.AddressList.Length - 1].ToString();
        }
    }
}