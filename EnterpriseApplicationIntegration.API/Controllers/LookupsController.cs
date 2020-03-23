using EnterpriseApplicationIntegration.Application;
using EnterpriseApplicationIntegration.Core.LoggerBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.API.Controllers
{
    /// <summary>
    /// API Controller
    /// </summary>
    [Route("api/lookups")]
    [ApiController]
    [AllowAnonymous]
    //[Authorize(Roles = "GetFreonTypes")]
    public class LookupsController : ControllerBase
    {
        private readonly IServices _cocService;
        private readonly AppLog _log;

        /// <summary>
        /// Instantiate a new instance of <see cref="LookupsController"/>
        /// </summary>
        /// <param name="cocService">Coc Service</param>
        /// <param name="log"></param>
        public LookupsController(IServices cocService, AppLog log)
        {
            _cocService = cocService;
            _log = log;
        }

        /// <summary>This API will return a list of active freon types which exist in Saber system</summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        //[Produces("application/json")]
        [ProducesResponseType(typeof(IActionResult), 200, Type = typeof(object[]))]
        public async Task<IActionResult> Get()
        {
            _log.MethodName = "GetTypes";
            var result = await _cocService.GetTypes();

            return Ok(result);
        }
    }
}