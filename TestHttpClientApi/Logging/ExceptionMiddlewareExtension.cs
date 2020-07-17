using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHttpClientApi.Logging
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder applicationBuilder, ILogger logger)
        {
            applicationBuilder.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorCtx = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorCtx != null)
                    {
                        var exc = errorCtx.Error;
                        var errorId = Activity.Current?.Id ?? context.TraceIdentifier;

                        logger.Error(exc, $"Some kind of error happend in the API. ErrorId: {errorId}");

                        var jsonResponse = JsonConvert.SerializeObject(new CustomErrorResponse
                        {
                            ErrorId = errorId,
                            Message = "Some kind of error happend in the API."
                        });

                        await context.Response.WriteAsync(jsonResponse, Encoding.UTF8);
                    }
                });
            });
        }
    }
}
