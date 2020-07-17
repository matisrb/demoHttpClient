using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHttpClientApi.Commands.Interfaces;

namespace TestHttpClientApi.CommandHandlers.Interfaces
{
    public abstract class CommandHandlerTest<TCommand> : CommandHandlerBase<TCommand>
        where TCommand : ICommand
    {
        readonly ILogger _logger;

        public CommandHandlerTest(ILogger logger)
            :base(logger)
        {
            _logger = logger;
        }

        protected abstract Task<string> OnHandleTest(TCommand command);

        protected override async Task<string> OnHandleBase(TCommand command)
        {
            _logger.Information($"LOG from CommandHandlerTest");

            var result = await OnHandleTest(command);

            return result;
        }

    }
}
