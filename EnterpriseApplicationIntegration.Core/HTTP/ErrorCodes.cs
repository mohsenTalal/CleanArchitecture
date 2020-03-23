using System.Net;

namespace EnterpriseApplicationIntegration.Core.HTTP
{
    public static class ErrorCodes
    {
        public const string BadRequest = "EAI-400";
        public const string Unauthorized = "EAI-401";
        public const string InternalServerError = "EAI-500";

        public static string Other(HttpStatusCode httpStatus)
        {
            return $"EAI-{(int)httpStatus}";
        }
    }
}