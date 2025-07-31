using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpTools.HTTP.Architecture
{
    public interface IRequestHandler
    {
        /// <summary>
        ///     Initiate an HTTP GET Request that returns the contents of the response in a custom HTTP Response object.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="timeoutInMs"></param>
        /// <returns></returns>
        public Task<HttpResponse> GetAsync(string endpoint, IDictionary<string, object> data, int timeoutInMs = -1);

        /// <summary>
        ///     Initiate an HTTP GET Request that returns the contents of the response as a deserialized JSON object.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="timeoutInMs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T?> GetAsync<T>(string endpoint, IDictionary<string, object> data, int timeoutInMs = -1);

        /// <summary>
        ///     Initiate an HTTP POST Request that returns the contents of the response in a custom HTTP Response object.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="timeoutInMs"></param>
        /// <returns></returns>
        public Task<HttpResponse> PostAsync(string endpoint, IDictionary<string, object> data, int timeoutInMs = -1);

        /// <summary>
        ///     Initiate an HTTP POST Request that returns the contents of the Response as a deserialized JSON object.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="timeoutInMs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T?> PostAsync<T>(string endpoint, IDictionary<string, object> data, int timeoutInMs = -1);

        /// <summary>
        ///     Initiate an HTTP PUT Request that returns the contents of the response in a custom HTTP Response object.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="timeoutInMs"></param>
        /// <returns></returns>
        public Task<HttpResponse> PutAsync(string endpoint, IDictionary<string, object> data, int timeoutInMs = -1);

        /// <summary>
        ///     Initiate an HTTP PUT Request that returns the contents of the Response as a deserialized JSON object.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="timeoutInMs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T?> PutAsync<T>(string endpoint, IDictionary<string, object> data, int timeoutInMs = -1);

        /// <summary>
        ///     Initiate an HTTP DELETE Request that returns the contents of the response in a custom HTTP Response object.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="timeoutInMs"></param>
        /// <returns></returns>
        public Task<HttpResponse> DeleteAsync(string endpoint, IDictionary<string, object> data, int timeoutInMs = -1);

        /// <summary>
        ///      Initiate an HTTP DELETE Request that returns the contents of the Response as a deserialized JSON object.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="timeoutInMs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T?> DeleteAsync<T>(string endpoint, IDictionary<string, object> data, int timeoutInMs = -1);
    }
}