using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestHttpClientApi.Agents
{
    public interface IWeatherForecastService
    {
        Task<int> GetBelgradeData();
    }
}
