using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestHttpClientApi.Agents
{
    public class WeatherForecastService : IWeatherForecastService
    {
        readonly HttpClient _httpClient;

        public WeatherForecastService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> GetBelgradeData()
        {
            var uriBuild = new UriBuilder(_httpClient.BaseAddress)
            {
                Query = $"key=11d124af2e604f87b0a30b82e41616d9&city=Belgrade,Rs"
            };

            var result = await _httpClient.GetAsync(uriBuild.Uri);

            var stringContent = await result.Content.ReadAsStringAsync();

            return stringContent.Length;
        }
    }
}
