using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.Core.LoggerBuilder
{
    public interface IAppLogger
    {
        /// <summary>
        /// Save the logging object
        /// </summary>
        Task Commit(AppLog log);
    }
}