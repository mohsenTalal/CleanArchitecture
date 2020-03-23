using System;
using System.Collections.Generic;
using System.Net;

namespace EnterpriseApplicationIntegration.Core.HTTP
{
    /// <summary>
    /// GeneralException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299
    /// </summary>
    [Serializable]
    public class GeneralException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public string APIErrorJson { get; set; }

        public List<ResponseHeader> ResponseHeaderList;

        public GeneralException()
        {
        }

        protected GeneralException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}