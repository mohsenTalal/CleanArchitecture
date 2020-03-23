using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.Application
{
    public interface IServices
    {
        Task<object> GetTypes();
    }
}