using EnterpriseApplicationIntegration.Core.HTTP;
using System;
using System.Collections.Generic;
using System.Net;

namespace EnterpriseApplicationIntegration.Core
{
    /// <summary>
    /// APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299
    /// </summary>
    [Serializable]
    public class APIException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string ErrorCode { get; set; }

        public List<ResponseHeader> ResponseHeaderList;

        public APIException()
        { }

        protected APIException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}