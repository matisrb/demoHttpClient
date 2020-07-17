using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHttpClientApi.Commands.Interfaces;

namespace TestHttpClientApi.CommandHandlers.Interfaces
{
    public interface ICommandHandler<TCommand> 
        where TCommand : ICommand
    {
        public Task<string> HandleAsync(TCommand command);
    }
}
