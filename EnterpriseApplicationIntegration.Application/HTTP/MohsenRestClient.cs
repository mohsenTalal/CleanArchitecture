using EnterpriseApplicationIntegration.Core;
using EnterpriseApplicationIntegration.Core.HTTP;
using EnterpriseApplicationIntegration.Core.LoggerBuilder;
using EnterpriseApplicationIntegration.Core.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApplicationIntegration.Application
{
    /// <summary>
    /// Version 1.0.1
    /// </summary>
    public class MohsenRestClient : IMohsenRestClient
    {
        private readonly AppSettings _settings;
        private readonly AppLog log;
        private string Token { get; set; }
        private AuthenticationMode AuthenticationMode { get; set; }
        private AcceptLanguage AcceptLanguage { get; set; }
        public List<RequestHeader> CustomHeaderList { get; set; }
        private bool generateAPIException { get; set; }

        /// <summary>Gets or sets the base URL.</summary>
        /// <value>The base URL.</value>
        public string BaseURL { get; set; }

        /// <summary>Gets or sets the username.</summary>
        /// <value>The username.</value>
        public string Username { get; set; }

        /// <summary>Gets or sets the password.</summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the IMohsenRestClient  using Basic Authentication
        /// </summary>
        /// <param name="baseURL">The base URL used for all operations</param>
        /// <param name="acceptLanguage">The Accept-Language request HTTP header to determine which language the client uses</param>
        /// <param name="username">An identification used by the client to access the API</param>
        /// <param name="password">A password is a word or string of characters used for authentication to prove identity or access approval to gain access to the API. which is to be kept secret from those not allowed access.</param>
        /// <param name="generateAPIException">if true the service return APIEception if false it will return a GeneralException</param>
        public IMohsenRestClient(IOptions<AppSettings> settings, AppLog log)
        // string baseURL, AcceptLanguage acceptLanguage, string username, string password, bool generateAPIException, IAppLogger appLogger)
        {
            this.log = log;

            _settings = settings.Value;
            BaseURL = settings.Value.GatewayUrl;
            Username = settings.Value.GatewayUsername;
            Password = settings.Value.GatewayPassword;

            AcceptLanguage = AcceptLanguage.ar;
            AuthenticationMode = AuthenticationMode.Basic;
            generateAPIException = true;
        }

        /// <summary>
        /// Send a GET request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="pathAndQuery">The PathAndQuery property contains the path for the API resource and the query information or parameter information sent with the request.</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public APIResponse<T> Get<T>(string pathAndQuery)
        {
            using (var client = this.Initialize())
            {
                var response = client.GetAsync(pathAndQuery).Result;
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = response.Content.ReadAsStringAsync().Result;
                log.ProviderResponse = content;

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a GET request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="pathAndQuery">The PathAndQuery property contains the path for the API resource and the query information or parameter information sent with the request.</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public async Task<APIResponse<T>> GetAsync<T>(string pathAndQuery, bool isCustomResolver = true)
        {
            using (var client = this.Initialize())
            {
                var response = await client.GetAsync(pathAndQuery);
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    log.ProviderResponse = content;
                    throw GenerateException(HttpStatusCode.InternalServerError, content, headerList);

                }
                else
                {
                    log.ProviderResponse = "Success";
                    return CreateAPIResponse<T>(content, headerList, isCustomResolver);
                }
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public APIResponse<T> Post<T>(string path, object requestBody)
        {
            using (var client = this.Initialize())
            {
                var contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                var formatter = new JsonSerializerSettings() { Formatting = Formatting.Indented, ContractResolver = contractResolver };
                var body = new StringContent(JsonConvert.SerializeObject(requestBody, formatter), Encoding.UTF8, "application/json");
                var response = client.PostAsync(path, body).Result;
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a POST request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public async Task<APIResponse<T>> PostAsync<T>(string path, object requestBody)
        {

            using (var client = this.Initialize())
            {
                var body = FormatBody(requestBody);
                var response = await client.PostAsync(path, body);
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                log.ProviderResponse = content;

                return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a Put request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public APIResponse<T> Put<T>(string path, object requestBody)
        {
            using (var client = this.Initialize())
            {
                var response = client.PutAsync(path, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")).Result;
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a Put request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <param name="requestBody">The request body object that should be parsed as JSON <c><b>** DO NOT CONVERT THE OBJECT TO JSON **</b></c></param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public async Task<APIResponse<T>> PutAsync<T>(string path, object requestBody)
        {
            using (var client = this.Initialize())
            {
                var response = await client.PutAsync(path, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a Delete request to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public APIResponse<T> Delete<T>(string path)
        {
            using (var client = this.Initialize())
            {
                var response = client.DeleteAsync(path).Result;
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        /// <summary>
        /// Send a Delete request asynchronously to the specified Uri
        /// </summary>
        /// <typeparam name="T">A generic type parameters that the client specifies in order to convert the returned json content to the specified type</typeparam>
        /// <param name="path">The path property contains the path for the API resource</param>
        /// <returns>Returns the specified object represented by the client.</returns>
        /// <exception cref="APIException">APIException raised when IsSuccessStatusCode is return false which will check the http Response StatusCode greater than or equal to 200 and less than or equal to 299</exception>
        public async Task<APIResponse<T>> DeleteAsync<T>(string path)
        {
            using (var client = this.Initialize())
            {
                var response = await client.DeleteAsync(path);
                List<ResponseHeader> headerList = FillResponseHeaderList(response);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw GenerateException(response.StatusCode, content, headerList);
                else
                    return CreateAPIResponse<T>(content, headerList);
            }
        }

        #region "Private Methods"

        private HttpClient Initialize()
        {
            var client = default(HttpClient);
            if (_settings.IsDevelopment)
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                client = new HttpClient(httpClientHandler);
            }
            else
            {
                client = new HttpClient();
            }

            client.BaseAddress = new Uri(BaseURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (this.AuthenticationMode == AuthenticationMode.Basic)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Password)));
            else if (this.AuthenticationMode == AuthenticationMode.Token)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);
            client.DefaultRequestHeaders.Add("Accept-Language", this.AcceptLanguage.ToString());
            if (CustomHeaderList != null)
            {
                foreach (var customHeader in CustomHeaderList)
                {
                    client.DefaultRequestHeaders.Add(customHeader.HeaderName, customHeader.HeaderValue);
                }
            }

            return client;
        }

        private List<ResponseHeader> FillResponseHeaderList(HttpResponseMessage response)
        {
            List<ResponseHeader> headerList = new List<ResponseHeader>();

            var headers = response.Headers.ToList();
            foreach (var header in headers)
            {
                var apiHeader = new ResponseHeader();
                IEnumerable<string> values = null;
                if (response.Headers.TryGetValues(header.Key, out values))
                {
                    apiHeader.HeaderName = header.Key;
                    apiHeader.HeaderValues = values.ToList();
                    headerList.Add(apiHeader);
                }
            }

            return headerList;
        }

        private APIResponse<T> CreateAPIResponse<T>(string content, List<ResponseHeader> responseHeaderList, bool isCustomResolver = false)
        {
            APIResponse<T> apiResponse = new APIResponse<T>();
            apiResponse.ResponseHeaderList = responseHeaderList;
            apiResponse.Response = JsonConvert.DeserializeObject<T>(content);
            return apiResponse;
        }

        private BusinessException GenerateException(HttpStatusCode httpStatusCode, string content, List<ResponseHeader> responseHeaderList)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BusinessException(httpStatusCode, nameof(HttpStatusCode.BadRequest));
                case HttpStatusCode.NotFound:
                    throw new BusinessException(httpStatusCode, nameof(HttpStatusCode.NotFound));
                default:
                    throw new GeneralException { HttpStatusCode = httpStatusCode, APIErrorJson = content, ResponseHeaderList = responseHeaderList };
            }
        }

        private HttpContent FormatBody(object body)
        {
            if (body == null)
            {
                return null;
            }

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var formatter = new JsonSerializerSettings() { Formatting = Formatting.Indented, ContractResolver = contractResolver };
            var bodyStr = JsonConvert.SerializeObject(body, formatter);
            return new StringContent(bodyStr, Encoding.UTF8, "application/json");
        }

        #endregion "Private Methods"
    }
}
