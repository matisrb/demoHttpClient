using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TestHttpClientApi.Services.Command;

namespace TestHttpClientApi.Services
{
    public class ProcessService : BackgroundService
    {
        private Channel<CommandTest> _channel;
        public ProcessService(Channel<CommandTest> channel)
        {
            _channel = channel;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!_channel.Reader.Completion.IsCompleted)
            {
                var command = await _channel.Reader.ReadAsync();

                var pp = command;
            }
        }
    }
}
