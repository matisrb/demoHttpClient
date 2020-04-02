using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TestHttpClientApi.Common;

namespace TestHttpClientApi.HttpHanders
{
    public class LoggingHandler : DelegatingHandler
    {
        readonly ILogger _logger;
        readonly IAppCustom _appCustom;

        public LoggingHandler(ILoggerFactory loggerFactory,
                              IAppCustom appCustom)
        {
            _logger = loggerFactory.CreateLogger(nameof(LoggingHandler));

            _appCustom = appCustom;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            int pp = _appCustom.CustomNumber;

            var requestUri = request.RequestUri.ToString();

            _logger.LogInformation($"Log from handler. request uri: {requestUri}");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
