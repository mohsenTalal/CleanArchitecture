using System.Collections.Generic;

namespace EnterpriseApplicationIntegration.Core.HTTP
{
    /// <summary>
    /// This class will convert the response of the API to the specified type T
    /// </summary>
    public class APIResponse<T>
    {
        /// <summary>
        /// A generic type parameters that the client specifies in order to convert the returned json response content to the specified type.
        /// </summary>
        public T Response;

        /// <summary>
        /// This list will define all headers returned by response.
        /// </summary>
        public List<ResponseHeader> ResponseHeaderList;
    }
}