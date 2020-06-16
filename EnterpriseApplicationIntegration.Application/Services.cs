using EnterpriseApplicationIntegration.Core.HTTP;
using EnterpriseApplicationIntegration.Core.LoggerBuilder;
using EnterpriseApplicationIntegration.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.Application
{
    public class Services : IServices
    {
        private readonly AppLog _log;
        private readonly IMohsenRestClient _restClient;
        private readonly AppSettings _settings;

        public Services(IOptions<AppSettings> settings, AppLog log, IMohsenRestClient restClient)
        {
            _log = log;
            _restClient = restClient;
            _settings = settings.Value;
            _restClient.BaseURL = _settings.ServiceUrl;
            _restClient.Username = _settings.ServiceUsername;
            _restClient.Password = _settings.ServicePassword;
            _restClient.CustomHeaderList = _restClient.CustomHeaderList ?? new List<RequestHeader>();
        }

        public async Task<object> GetTypes()
        {
            return new System.NotImplementedException();
        }
    }
}
