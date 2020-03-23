using System.Collections.Generic;

namespace EnterpriseApplicationIntegration.Core.Auth
{
    public class BasicAuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }
        public List<string> ScopesList { get; set; }
    }
}