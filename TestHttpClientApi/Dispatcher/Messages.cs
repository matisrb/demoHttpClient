using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TestHttpClientApi.CommandHandlers.Interfaces;
using TestHttpClientApi.Commands.Interfaces;

namespace TestHttpClientApi.Dispatcher
{
    public sealed class Messages
    {
        readonly IServiceProvider _serviceProvider;

        public Messages(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<TResult> Dispatch<TResult>(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic resolvedHandler =_serviceProvider.GetService(handlerType);
            Task<TResult> result = resolvedHandler.HandleAsync((dynamic)command);

            return result;
        }
    }
}
