using EnterpriseApplicationIntegration.Core.HTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.API.Middlewares
{
    /// <summary>
    /// Custom Validation Result
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.IActionResult" />
    public class CustomValidationResult : IActionResult
    {
        /// <summary>
        /// Executes the result operation of the action method asynchronously. This method is called by MVC to process
        /// the result of an action method.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes
        /// information about the action that was executed and request information.</param>
        /// <returns>
        /// A task that represents the asynchronous execute operation.
        /// </returns>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var modelStateEntries = context.ModelState.Where(e => e.Value.Errors.Count > 0).ToArray();
            var errors = new List<ValidationError>();

            if (modelStateEntries.Any())
            {
                foreach (var modelStateEntry in modelStateEntries)
                {
                    foreach (var modelStateError in modelStateEntry.Value.Errors)
                    {
                        var error = new ValidationError
                        {
                            Code = modelStateEntry.Key,
                            Message = modelStateError.ErrorMessage
                        };

                        errors.Add(error);
                    }
                }
            }

            var result = new
            {
                Code = "EAI-400",
                ResultMessage = "Validation Error",
                HttpStatusCode.BadRequest,
                ValidationErrors = errors
            };
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}