using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestHttpClientApi.Agents
{
    public interface IFacebookAgent
    {
        Task<TResponse> GetAsync<TResponse>(string address, string query = null)
          where TResponse : class;

        Task<TResponse> PostAsync<TRequest, TResponse>(string address, TRequest request, string query = null)
            where TResponse : class
            where TRequest : class;
    }
}
