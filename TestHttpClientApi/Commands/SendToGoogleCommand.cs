using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHttpClientApi.Commands.Interfaces;

namespace TestHttpClientApi.Commands
{
    public sealed class SendToGoogleCommand : ICommand
    {
        public string Messages { get; set; }
    }
}
