using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TestHttpClientApi.Services
{
    public class NotificationService : BackgroundService
    {
        private Channel<string> _channel;

        public NotificationService(Channel<string> channel)
        {
            _channel = channel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!_channel.Reader.Completion.IsCompleted)
            {
                string message = string.Empty;
                try
                {
                    message = await _channel.Reader.ReadAsync();
                }
                catch (Exception exc)
                {
                    var errro = exc;
                }
                

                //Send a message
                var pp = message;
            }
        }
    }
}
