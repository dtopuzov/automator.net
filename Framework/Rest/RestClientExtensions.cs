using System.Reflection;
using log4net;
using RestSharp;

namespace Framework.RestAPI
{
    /// <summary>
    /// Extension for RestSharp rest client.
    /// </summary>
    public static class RestClientExtensions
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Sends HTTP request and retrun the response.
        /// In addition it logs details for sended request.
        /// </summary>
        /// <param name="client">RestClient object.</param>
        /// <param name="request">Request object.</param>
        /// <returns>Response object.</returns>
        public static IRestResponse SendRequest(this RestClient client, IRestRequest request)
        {
            Log.Info("======================= Request =======================");
            Log.Info("Url: " + client.BaseUrl + request.Resource);
            Log.Info("Method: " + request.Method);
            if (client.DefaultParameters.Count > 0)
            {
                Log.Info("Client Params:");
                foreach (var param in client.DefaultParameters)
                {
                    Log.Info(param.Name + ":" + param.Value);
                }
            }

            if (request.Parameters.Count > 0)
            {
                Log.Info("Request Params:");
                foreach (var param in request.Parameters)
                {
                    Log.Info(param.Name + ":" + param.Value);
                }
            }

            var response = client.Execute(request);

            Log.Info("======================= Response ======================");
            Log.Info("Status Code: " + response.StatusCode);

            return response;
        }
    }
}
