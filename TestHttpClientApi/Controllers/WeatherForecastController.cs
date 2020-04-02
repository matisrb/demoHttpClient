using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestHttpClientApi.Agents;
using TestHttpClientApi.Common;

namespace TestHttpClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {        
        private readonly IAffirmationsService _affirmationsService;

        readonly IHttpClientFactory _httpClientFactory;

        public WeatherForecastController(IAffirmationsService affirmationsService,
                                         IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _affirmationsService = affirmationsService;
        }

        //0. Dummy
        [HttpGet("{abc}/dummy")]
        public async Task<string> GetDummy()
        {
            var ips = await Dns.GetHostAddressesAsync("www.google.com");

            var sw = Stopwatch.StartNew();

            int length = 20;
            for (int i = 0; i < length; i++)
            {
                using (var httpClinet = new HttpClient())
                {
                    var response = await httpClinet.GetAsync("https://www.google.com");
                }
            }

            sw.Stop();

            return $"Elapsed {Math.Round(sw.ElapsedMilliseconds/1000.0, 2, MidpointRounding.AwayFromZero)}s {Environment.NewLine}" +
                   $"for {length} requests {Environment.NewLine}" +
                   $"IP : {ips.FirstOrDefault()}";
        }

        //1. Factory Basic usage
        [HttpGet("{abc}/factory/basic")]
        public async Task<string> GetBasicFactory()
        {
            var ips = await Dns.GetHostAddressesAsync("www.google.com");

            var sw = Stopwatch.StartNew();

            int length = 20;
            for (int i = 0; i < length; i++)
            {
                var client = _httpClientFactory.CreateClient();
                var result = await client.GetAsync("https://www.google.com");
            }

            sw.Stop();

            return $"Elapsed {Math.Round(sw.ElapsedMilliseconds / 1000.0, 2, MidpointRounding.AwayFromZero)}s {Environment.NewLine}" +
                   $"for {length} requests {Environment.NewLine}" +
                   $"IP: {ips.FirstOrDefault()}";
        }

        //2. Factory Named clients
        [HttpGet("{abc}/factory/named")]
        public async Task<string> GetFactory()
        {
            var ips = await Dns.GetHostAddressesAsync("www.google.com");

            var sw = Stopwatch.StartNew();

            int lenght = 10;
            for (int i = 0; i < lenght; i++)
            {
                var client = _httpClientFactory.CreateClient(ApiConstants.GoogleClient);

                var result = await client.GetAsync("");
            }

            sw.Stop();

            return $"Elapsed {Math.Round(sw.ElapsedMilliseconds / 1000.0, 2, MidpointRounding.AwayFromZero)}s {Environment.NewLine}" +
                   $"for {lenght} requests {Environment.NewLine}" +
                   $"IP: {ips.FirstOrDefault()}";
        }        

        //3. Factory Typed clients
        [HttpGet("{abc}/factory/typed/affirmation")]
        public async Task<string> GetAffirmaition()
        {
            var sw = Stopwatch.StartNew();

            var ips = await Dns.GetHostAddressesAsync("www.affirmations.dev");

            var result = await _affirmationsService.GetAffirmation();

            sw.Stop();

            return $"Affirmation: {Environment.NewLine}" +
                   $"{result} {Environment.NewLine}" +
                   $"elapsed time: {Math.Round(sw.ElapsedMilliseconds/1000.0, 2, MidpointRounding.AwayFromZero) }s {Environment.NewLine}" +
                   $"IP: {ips.FirstOrDefault()}";
        }
      
    }
}
