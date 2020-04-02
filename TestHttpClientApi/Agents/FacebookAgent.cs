using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestHttpClientApi.Agents
{
    public class FacebookAgent : IFacebookAgent
    {
        HttpClient _httpClient;

        public FacebookAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<TResponse> GetAsync<TResponse>(string address, string query = null) where TResponse : class
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PostAsync<TRequest, TResponse>(string address, TRequest request, string query = null)
            where TRequest : class
            where TResponse : class
        {
            throw new NotImplementedException();
        }
    }
}
