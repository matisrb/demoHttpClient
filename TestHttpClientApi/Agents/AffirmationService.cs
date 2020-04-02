using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestHttpClientApi.Common;
using TestHttpClientApi.Models;

namespace TestHttpClientApi.Agents
{
    public class AffirmationService : IAffirmationsService
    {
        readonly HttpClient _httpClient;

        readonly IAppCustom _appCustom;

        public AffirmationService(HttpClient httpClient, IAppCustom appCustom)
        {
            _httpClient = httpClient;

            _appCustom = appCustom;
        }

        public async Task<string> GetAffirmation()
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get
            };

            int pp = _appCustom.CustomNumber;

            //var response = await _httpClient.GetAsync("");
            var response = await _httpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<AffirmationModel>();

            return result.Affirmation;
        }
    }
}
