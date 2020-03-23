using System;
using System.Net;

namespace EnterpriseApplicationIntegration.Core.HTTP
{
    [Serializable]
    public class BusinessException : Exception
    {
        public BusinessException(HttpStatusCode httpStatusCode, string message)
        {
            Status = (int)httpStatusCode;
            Message = message;
        }

        public BusinessException(int status, string entityName, string errorKey, string type, string title, string path, string detail, string @params, string message)
        {
            Status = status;
            EntityName = entityName;
            ErrorKey = errorKey;
            Type = type;
            Title = title;
            Path = path;
            Detail = detail;
            Params = @params;
            Message = message;
        }

        public int Status { get; }
        public string EntityName { get; }
        public string ErrorKey { get; }
        public string Type { get; }
        public string Title { get; }
        public string Path { get; }
        public string Detail { get; }
        public string Params { get; }
        public new string Message { get; }
    }
}