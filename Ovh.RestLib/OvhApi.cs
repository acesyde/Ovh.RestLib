using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Ovh.RestLib.Core;
using Ovh.RestLib.Models.Request;
using Ovh.RestLib.Models.Response;
using RestSharp;

namespace Ovh.RestLib
{
    /// <summary>
    /// Ovh API
    /// </summary>
    public class OvhApi
    {
        /// <summary>
        /// The _rest client
        /// </summary>
        private readonly RestClient _restClient;

        /// <summary>
        /// The _regions
        /// </summary>
        private readonly IDictionary<string, string> _regions = new Dictionary<string, string>
            {
                {"ovh-eu","https://api.ovh.com/1.0"},
                {"ovh-ca","https://ca.api.ovh.com/1.0"},
                {"runabove-ca","https://api.runabove.com/1.0"}
            };

        /// <summary>
        /// The _delta time
        /// </summary>
        private int? _deltaTime;

        /// <summary>
        /// Gets or sets the application key.
        /// </summary>
        /// <value>
        /// The application key.
        /// </value>
        public string ApplicationKey { get; private set; }
        /// <summary>
        /// Gets or sets the application secret.
        /// </summary>
        /// <value>
        /// The application secret.
        /// </value>
        public string ApplicationSecret { get; private set; }
        /// <summary>
        /// Gets or sets the consumer key.
        /// </summary>
        /// <value>
        /// The consumer key.
        /// </value>
        public string ConsumerKey { get; set; }
        /// <summary>
        /// Gets or sets the end name of the point.
        /// </summary>
        /// <value>
        /// The end name of the point.
        /// </value>
        public string EndPointName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OvhApi" /> class.
        /// </summary>
        /// <param name="applicationKey">The application key.</param>
        /// <param name="applicationSecret">The application secret.</param>
        /// <param name="region">The region. [ovh-eu,ovh-ca,runabove-ca]</param>
        /// <remarks>The region as defaut value "ovh-eu"</remarks>
        public OvhApi(string applicationKey, string applicationSecret, string region = "ovh-eu")
        {
            if (string.IsNullOrEmpty(applicationKey))
                throw new ArgumentNullException("applicationKey cannot be null or empty");

            if (string.IsNullOrEmpty(applicationSecret))
                throw new ArgumentNullException("applicationSecret cannot be null or empty");

            if (string.IsNullOrEmpty(region))
                throw new ArgumentNullException("region cannot be null or empty");

            if (!_regions.ContainsKey(region))
                throw new ArgumentNullException("Unknow provided region");

            ApplicationSecret = applicationSecret;
            ApplicationKey = applicationKey;
            EndPointName = _regions[region];
            _restClient = new RestClient(EndPointName);
        }

        /// <summary>
        /// Requests the consumer key.
        /// </summary>
        /// <returns>
        /// Return String
        /// </returns>
        public string RequestConsumerKey()
        {
            var restRequest = InitBaseRequest("auth/credential");

            var oAuthRequest = new OAuthRequest
            {
                AccessRules = new[] { new AccessRules { Method = "GET", Path = "/*" } },
                Redirection = "http://localhost"
            };
            restRequest.AddJsonBody(oAuthRequest);

            var response = _restClient.ExecuteAsPost<OAuthResponse>(restRequest, "POST");

            if (response != null && response.StatusCode == HttpStatusCode.OK && response.Data != null)
            {
                ConsumerKey = response.Data.ConsumerKey;
                return response.Data.ValidationUrl;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the specified URL.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public TResponse Get<TResponse>(string url) where TResponse : class, new()
        {
            var restRequest = InitBaseRequest(url);

            AddCommonHeaders(restRequest);

            var restResponse = _restClient.ExecuteAsGet<TResponse>(restRequest, Method.GET.ToString());

            if (restResponse != null && restResponse.StatusCode == HttpStatusCode.OK)
                return restResponse.Data;

            return null;
        }

        /// <summary>
        /// Posts the specified URL.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Post<TRequest, TResponse>(string url, TRequest request)
            where TResponse : class, new()
            where TRequest : class
        {
            if (request == null)
                throw new ArgumentNullException("request cannot be null");

            return RawRequest<TRequest, TResponse>(Method.POST, url, request);
        }

        /// <summary>
        /// Deletes the specified URL.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Delete<TRequest, TResponse>(string url, TRequest request)
            where TResponse : class, new()
            where TRequest : class
        {
            return RawRequest<TRequest, TResponse>(Method.DELETE, url, request);
        }

        /// <summary>
        /// Puts the specified URL.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TResponse Put<TRequest, TResponse>(string url, TRequest request)
            where TResponse : class, new()
            where TRequest : class
        {
            return RawRequest<TRequest, TResponse>(Method.PUT, url, request);
        }

        /// <summary>
        /// Raws the request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Return t response
        /// </returns>
        private TResponse RawRequest<TRequest, TResponse>(Method method, string url, TRequest request)
            where TResponse : class, new()
            where TRequest : class
        {
            var restRequest = InitBaseRequest(url, method);

            if (request != null)
                restRequest.AddJsonBody(restRequest);

            AddCommonHeaders(restRequest);

            var restResponse = _restClient.ExecuteAsPost<TResponse>(restRequest, method.ToString());

            if (restResponse != null && restResponse.StatusCode == HttpStatusCode.OK)
                return restResponse.Data;

            return null;
        }

        /// <summary>
        /// Adds the common headers.
        /// </summary>
        /// <param name="restRequest">The rest request.</param>
        private void AddCommonHeaders(RestRequest restRequest)
        {
            string stringBody = string.Empty;

            var body = restRequest.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);

            if (body != null)
                stringBody = body.Value.ToString();

            int requestTime = GetLocalTime() + GetDeltaTime();
            restRequest.AddHeader("X-Ovh-Timestamp", requestTime.ToString(CultureInfo.InvariantCulture));
            restRequest.AddHeader("X-Ovh-Consumer", ConsumerKey);
            restRequest.AddHeader("X-Ovh-Signature", GenerateSignature(ConsumerKey, restRequest.Method.ToString(), _restClient.BuildUri(restRequest).AbsoluteUri, stringBody, requestTime));
        }

        /// <summary>
        /// Gets the delta time.
        /// </summary>
        /// <returns>
        /// Return Int32
        /// </returns>
        private int GetDeltaTime()
        {
            if (_deltaTime.HasValue)
                return _deltaTime.Value;

            var request = InitBaseRequest("auth/time");
            var response = _restClient.Execute(request);

            if (response == null || response.StatusCode != HttpStatusCode.OK)
                return 0;

            int serverTime;
            if (int.TryParse(response.Content, out serverTime))
            {
                _deltaTime = serverTime - GetLocalTime();
                return _deltaTime.Value;
            }

            return 0;
        }

        /// <summary>
        /// Gets the local time.
        /// </summary>
        /// <returns>
        /// Return Int32
        /// </returns>
        private static int GetLocalTime()
        {
            return ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

        /// <summary>
        /// Initializes the base request.
        /// </summary>
        /// <param name="requestedUrl">The requested URL.</param>
        /// <param name="method">The method.</param>
        /// <returns>
        /// Return Rest request
        /// </returns>
        private RestRequest InitBaseRequest(string requestedUrl, Method method = Method.GET)
        {
            var restRequest = new RestRequest(requestedUrl)
            {
                JsonSerializer = new RestSharpJsonNetSerializer(),
                Method = method
            };
            restRequest.AddHeader("Content-type", "application/json; charset=utf-8");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("X-Ovh-Application", ApplicationKey);

            return restRequest;
        }

        /// <summary>
        /// Generates the signature.
        /// </summary>
        /// <param name="ck">The ck.</param>
        /// <param name="method">The method.</param>
        /// <param name="query">The query.</param>
        /// <param name="body">The body.</param>
        /// <param name="tstamp">The tstamp.</param>
        /// <returns>
        /// Return String
        /// </returns>
        private string GenerateSignature(string ck, string method, string query, string body, int tstamp)
        {
            var computeHash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(ApplicationSecret + "+" + ck + "+" + method + "+" + query + "+" + body + "+" + tstamp));

            var sb = new StringBuilder();
            foreach (var ch in computeHash)
            {
                sb.Append(ch.ToString("x2"));
            }

            return "$1$" + sb;
        }
    }
}
