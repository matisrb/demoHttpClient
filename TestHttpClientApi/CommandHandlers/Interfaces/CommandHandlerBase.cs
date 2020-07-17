using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHttpClientApi.Commands.Interfaces;

namespace TestHttpClientApi.CommandHandlers.Interfaces
{
    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand> 
        where TCommand : ICommand
    {
        readonly ILogger _logger;
        public CommandHandlerBase(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> HandleAsync(TCommand command)
        {
            _logger.Information($"Handler started {GetType()} handling command: {command.GetType()}");
            
            var result = await OnHandleBase(command);

            _logger.Information($"Handler {GetType()} ended handling command: {command.GetType()}");

            return result;
        }

        protected abstract Task<string> OnHandleBase(TCommand command);
    }
}
