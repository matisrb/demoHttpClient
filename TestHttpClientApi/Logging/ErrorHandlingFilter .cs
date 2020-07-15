using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Serilog;

namespace TestHttpClientApi.Logging
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        readonly Serilog.ILogger _logger;
        //private readonly TelemetryClient _telemetryClient;

        public ErrorHandlingFilter(IWebHostEnvironment hostingEnvironment,
            Serilog.ILogger logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }      

        public override void OnException(ExceptionContext context)
        {
            var actionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            Type controllerType = actionDescriptor.ControllerTypeInfo;

            var controllerBase = typeof(ControllerBase);
            var controller = typeof(Controller);

            // Api's implements ControllerBase but not Controller
            if (controllerType.IsSubclassOf(controllerBase) && !controllerType.IsSubclassOf(controller))
            {
                var errorId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

                var appName = _hostingEnvironment.ApplicationName;
                var envName = _hostingEnvironment.EnvironmentName;

                _logger.Error(context.Exception, $"Some error occured. ErrorId: {errorId}. {context.Exception.Message}" +
                                                 $"EnvironmentName: {envName}" +
                                                 $"ApplicationName: {appName}");

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new JsonResult(new CustomErrorResponse
                {
                    ErrorId = errorId,
                    Message = $"Some error occured. ErrorId: {errorId}"
                });
            }

            base.OnException(context);
        }
    }
}
