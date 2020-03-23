using System;

namespace EnterpriseApplicationIntegration.Core.LoggerBuilder
{
    public class AppLog
    {
        public AppLog()
        {
            ReferenceNumber = Guid.NewGuid();
            RequestDate = DateTime.Now;
        }

        public int HttpStatus { get; set; }
        public string Token { get; set; }
        public string MethodName { get; set; }
        public string ErrorDescription { get; set; }
        public string StackTrace { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int ResponseTime { get; set; }
        public string OSType { get; set; }
        public string ApplicationVersion { get; set; }
        public string RequestHeaders { get; set; }
        public string DeviceInfo { get; set; }
        public string DeviceIP { get; set; }
        public Guid ReferenceNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestURL { get; set; }
        public string RequestMethod { get; set; }
        public string ProviderResponse { get; set; }
        public string HostIP { get; set; }
    }
}