using System.Collections.Generic;

namespace EnterpriseApplicationIntegration.Core.HTTP
{
    /// <summary>
    /// This class will map to one of the headers returned by the response
    /// </summary>
    public class ResponseHeader
    {
        /// <summary>
        /// This is the name of the header you need to add it to the header.
        /// </summary>
        public string HeaderName { get; set; }

        /// <summary>
        /// This is the value of the header you need to add it to the header.
        /// </summary>
        public List<string> HeaderValues { get; set; }

        public ResponseHeader()
        {
            HeaderValues = new List<string>();
        }
    }
}