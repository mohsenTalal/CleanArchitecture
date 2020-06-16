using EnterpriseApplicationIntegration.Core.Auth;
using EnterpriseApplicationIntegration.Core.HTTP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.Application
{
    public class AuthService : IAuthRepository
    {
        private readonly IMohsenRestClient restClient;

        public AuthService(IMohsenRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async Task<IEnumerable<string>> Authenticate(string userName, string password)
        {
            var requestUri = "Client/Authenticate";

            var response = await restClient.PostAsync<BasicAuthenticationResponse>(
                requestUri,
                new { ClientId = userName, ClientSecret = password });

            return response.Response.ScopesList;
        }
    }
}
