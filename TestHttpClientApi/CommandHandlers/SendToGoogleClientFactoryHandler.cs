using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestHttpClientApi.CommandHandlers.Interfaces;
using TestHttpClientApi.Commands;
using TestHttpClientApi.Common;

namespace TestHttpClientApi.CommandHandlers
{
    public sealed class SendToGoogleClientFactoryHandler : CommandHandlerTest<SendToGoogleCommand> 
    {
        readonly IHttpClientFactory _httpClientFactory;

        public SendToGoogleClientFactoryHandler(IHttpClientFactory httpClientFactory, ILogger logger) 
            : base(logger)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task<string> OnHandleTest(SendToGoogleCommand command)
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
    }
}
