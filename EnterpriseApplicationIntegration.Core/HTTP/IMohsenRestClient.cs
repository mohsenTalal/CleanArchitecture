using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.Core.HTTP
{
    /// <summary>
    /// Version 1.0.1
    /// </summary>
    public interface IMohsenRestClient
    {
        /// <summary>Gets or sets the base URL.</summary>
        /// <value>The base URL.</value>
        string BaseURL { get; set; }

        /// <summary>Gets or sets the username.</summary>
        /// <value>The username.</value>
        string Username { get; set; }

        /// <summary>Gets or sets the password.</summary>
        /// <value>The password.</value>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the custom header list.
        /// </summary>
        /// <value>
        /// The custom header list.
        /// </value>
        List<RequestHeader> CustomHeaderList { get; set; }

        /// <summary>
        /// Send a GET request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="pathAndQuery">The PathAndQuery property contains the path for the API resource and the query information or parameter information sent with the request.</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the HTTP Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        APIResponse<T> Get<T>(string pathAndQuery);

        /// <summary>
        /// Send a GET request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="pathAndQuery">The PathAndQuery property contains the path for the API resource and the query information or parameter information sent with the request.</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the HTTP Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        Task<APIResponse<T>> GetAsync<T>(string pathAndQuery, bool isCustomResolver = true);

        /// <summary>
        /// Send a POST request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the HTTP Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        APIResponse<T> Post<T>(string path, object requestBody);

        /// <summary>
        /// Send a POST request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the HTTP Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        Task<APIResponse<T>> PostAsync<T>(string path, object requestBody);

        /// <summary>
        /// Send a Put request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the HTTP Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        APIResponse<T> Put<T>(string path, object requestBody);

        /// <summary>
        /// Send a Put request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the HTTP Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        Task<APIResponse<T>> PutAsync<T>(string path, object requestBody);

        /// <summary>
        /// Send a Delete request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the HTTP Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        APIResponse<T> Delete<T>(string path);

        /// <summary>
        /// Send a Delete request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the HTTP Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        Task<APIResponse<T>> DeleteAsync<T>(string path);
    }
}
