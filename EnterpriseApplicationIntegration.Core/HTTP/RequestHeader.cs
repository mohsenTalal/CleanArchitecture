namespace EnterpriseApplicationIntegration.Core.HTTP
{
    /// <summary>
    /// This class will define the extra headers you need to add to the request header.
    /// </summary>
    public class RequestHeader
    {
        /// <summary>
        /// This is the name of the header you need to add it to the header.
        /// </summary>
        public string HeaderName { get; set; }

        /// <summary>
        /// This is the value of the header you need to add it to the header.
        /// </summary>
        public string HeaderValue { get; set; }
    }
}