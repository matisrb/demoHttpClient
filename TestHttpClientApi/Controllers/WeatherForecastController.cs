using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestHttpClientApi.Agents;
using TestHttpClientApi.Common;
using Serilog;
using TestHttpClientApi.Commands;
using TestHttpClientApi.Dispatcher;
using System.Threading.Channels;
using TestHttpClientApi.Services.Command;
using Greet;

namespace TestHttpClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {        
        readonly IAffirmationsService _affirmationsService;
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger _logger;
        readonly Messages _messages;

        public WeatherForecastController(IAffirmationsService affirmationsService,
                                         IHttpClientFactory httpClientFactory,
                                         ILogger logger,
                                         Messages messages)
        {
            _httpClientFactory = httpClientFactory;

            _affirmationsService = affirmationsService;

            _logger = logger;

            _messages = messages;
        }

        //0. Dummy
        [HttpGet("{abc}/dummy")]
        public async Task<string> GetDummy()
        {

            _logger.Information("Log Message from dummy request");
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
            _logger.Information("Log Message from factory/basic request");

            var ips = await Dns.GetHostAddressesAsync("www.google.com");

            #region exception test
            //Global exception handling testing purpose
            //int p = 0;
            //var pp = 1 / p;
            #endregion

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
            var command = new SendToGoogleCommand 
            {
                Messages = "some message Bla" 
            };

            var result = await _messages.Dispatch<string>(command);

            return result;              
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

        [HttpPost("sendMessage/{message}")]
        public async Task<int> SendMessage([FromServices]Channel<string> channel, [FromRoute]string message)
        {

            await channel.Writer.WriteAsync(message);

            return await Task.FromResult(1);
        }

        [HttpPost("sendMessageC/{cmd}")]
        public async Task<int> SendMessageC([FromServices] Channel<CommandTest> channel, [FromRoute] int cmd)
        {

            await channel.Writer.WriteAsync(new CommandTest { CommandType = cmd});

            return await Task.FromResult(1);
        }

        [HttpPost("sendMessageGrpc/{name}")]
        public async Task<string> SendMessageG([FromServices] GreetingService.GreetingServiceClient greetingServiceClient, 
                                               [FromRoute] string name)
        {

            GreetingResponse response;
            try
            {
                response = await greetingServiceClient.GreetAsync(new GreetingRequest
                {
                    Greeting = new Greeting
                    {
                        FirstName = name,
                        LastName = "Matic"
                    }
                });
            }
            catch (Exception exc)
            {
                throw;
            }    

            return await Task.FromResult(response.Result);
        }
    }
}
