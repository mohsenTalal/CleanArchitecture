namespace EnterpriseApplicationIntegration.Core.Settings
{
    public class AppSettings
    {
        public bool AllowSwagger { get; set; }
        public bool IsDevelopment { get; set; }

        public string AppVersion { get; set; }
        public string GatewayUrl { get; set; }
        public string GatewayUsername { get; set; }
        public string GatewayPassword { get; set; }

        public string ServiceUrl { get; set; }
        public string ServiceUsername { get; set; }
        public string ServicePassword { get; set; }
    }
}