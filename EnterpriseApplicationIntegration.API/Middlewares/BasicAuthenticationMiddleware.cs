using EnterpriseApplicationIntegration.Core.Auth;
using EnterpriseApplicationIntegration.Core.LoggerBuilder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.API.Middlewares
{
    /// <summary>
    /// Basic Authentication middleware.
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private IAuthRepository _repository;
        private readonly AppLog _log;

        /// <summary>Initializes a new instance of the <see cref="BasicAuthenticationHandler"/> class.</summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="encoder">The encoder.</param>
        /// <param name="clock">The clock.</param>
        /// <param name="authService">The authentication service.</param>
        /// <param name="log">Application logger</param>
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthRepository authService,
            AppLog log)
            : base(options, logger, encoder, clock)
        {
            _repository = authService;
            _log = log;
        }

        /// <summary>
        /// Handles the authenticate asynchronous.
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            var username = default(string);
            var scopes = default(IEnumerable<string>);
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                _log.Token = authHeader.Parameter;

                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var password = credentials[1];
                username = credentials[0];
                scopes = await _repository.Authenticate(username, password);
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (scopes == null)
                return AuthenticateResult.Fail("Invalid Username or Password");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username),
            };

            foreach (var scope in scopes)
            {
                claims.Add(new Claim(ClaimTypes.Role, scope));
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}