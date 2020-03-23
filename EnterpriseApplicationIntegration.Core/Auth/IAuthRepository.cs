using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.Core.Auth
{
    public interface IAuthRepository
    {
        Task<IEnumerable<string>> Authenticate(string userName, string password);
    }
}