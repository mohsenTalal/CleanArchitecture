using EnterpriseApplicationIntegration.Core.HTTP;
using EnterpriseApplicationIntegration.Core.LoggerBuilder;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.Application
{
    public class AppLogger : IAppLogger
    {
        private readonly IMohsenRestClient _restClient;

        public AppLogger(IMohsenRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task Commit(AppLog log)
        {
            try
            {
                await _restClient.PostAsync<object>("Logs", log);
            }
            catch (Exception ex)
            {
                AddLogToFile(log, ex);
            }
        }

        private void AddLogToFile(AppLog log, Exception exception)
        {
            try
            {
                var logMessage = new StringBuilder();
                logMessage.AppendLine($"HttpStatus: {log.HttpStatus}");
                logMessage.AppendLine($"Token: {log.Token}");
                logMessage.AppendLine($"MethodName: {log.MethodName}");
                logMessage.AppendLine($"ErrorDescription: {log.ErrorDescription}");
                logMessage.AppendLine($"Request: {log.Request}");
                logMessage.AppendLine($"Response: {log.Response}");
                logMessage.AppendLine($"ResponseTime: {log.ResponseTime}");
                logMessage.AppendLine($"OSType: {log.OSType}");
                logMessage.AppendLine($"ApplicationVersion: {log.ApplicationVersion}");
                logMessage.AppendLine($"RequestHeaders: {log.RequestHeaders}");
                logMessage.AppendLine($"ReferenceNumber: {log.ReferenceNumber}");
                logMessage.AppendLine($"RequestURL: {log.RequestURL}");
                logMessage.AppendLine($"RequestMethod: {log.RequestMethod}");
                logMessage.AppendLine($"ProviderResponse: {log.ProviderResponse}");
                logMessage.AppendLine($"HostIP: {log.HostIP}");
                logMessage.AppendLine($"DeviceInfo: {log.DeviceInfo}");
                logMessage.AppendLine($"Error: {exception.Message}");
                logMessage.AppendLine($"StackTrace: {exception.StackTrace}");
                logMessage.AppendLine("==============================================================================");

                // TODO: to log into file
                // var logPath =  System.IO.Path.GetTempFileName();
                var logPath = $".\\Logs\\Error-{DateTime.Now.Ticks}.txt";
                var logFile = System.IO.File.Create(logPath);
                var logWriter = new System.IO.StreamWriter(logFile);
                logWriter.WriteLine(logMessage);
                logWriter.Dispose();
            }
            catch (Exception ex) { }
        }
    }
}
