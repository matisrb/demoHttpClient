using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using TestHttpClientApi.Agents;
using TestHttpClientApi.Common;
using TestHttpClientApi.HttpHandlers;

namespace TestHttpClientApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();            

            #region Register types

            services.AddTransient<LoggingHandler>();

            services.AddTransient<IAppCustom, CustomApp>();

            #endregion

            //Adds the System.Net.Http.IHttpClientFactory and related services to the Microsoft.Extensions.DependencyInjection.IServiceCollection.

            #region Basic Client Usage

            services.AddHttpClient();

            #endregion

            #region Named Client usage         

            services.AddHttpClient(ApiConstants.GoogleClient, client =>
            {
                client.BaseAddress = new Uri("https://www.google.com");
            });

            services.AddHttpClient(ApiConstants.FacebookClient, client =>
            {
                client.BaseAddress = new Uri("https://www.facebook.com");
            });

            #endregion

            #region Typed Client usage            

            services.AddHttpClient<IAffirmationsService, AffirmationService>(client =>
            {
                client.BaseAddress = new Uri("https://www.affirmations.dev/");
            });

            #endregion

            #region Typed Client usage - handlers 

            services.AddHttpClient<IAffirmationsService, AffirmationService>(client =>
            {
                client.BaseAddress = new Uri("https://www.affirmations.dev/");
            }).AddHttpMessageHandler<LoggingHandler>();

            #endregion

            #region Typed Client usage - Polly retry

            services.AddHttpClient<IAffirmationsService, AffirmationService>(client =>
            {
                client.BaseAddress = new Uri("https://www.affirmations.dev/");
            })//.AddTransientHttpErrorPolicy(x => x.RetryAsync(3))
              .AddPolicyHandler(GetRetryPolicy())
              .AddTransientHttpErrorPolicy(x => x.CircuitBreakerAsync(5, TimeSpan.FromSeconds(25)));

            #endregion

            #region Typed Client usage - Polly TimeOut

            //var shortTimeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
            //var longTimeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

            //var registry = services.AddPolicyRegistry();

            //registry.Add("regular", shortTimeout);
            //registry.Add("long", longTimeout);

            //services.AddHttpClient<IAffirmationsService, AffirmationService>(client =>
            //{
            //    client.BaseAddress = new Uri("https://www.affirmations.dev/");
            //}).AddPolicyHandlerFromRegistry("long");

            //////////
            //var timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
            //var extendedTimeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

            //services.AddHttpClient<IAffirmationsService, AffirmationService>(client =>
            //{
            //    client.BaseAddress = new Uri("https://www.affirmations.dev/");
            //}).AddPolicyHandler(request => request.Method == HttpMethod.Get ? timeout : extendedTimeout);

            #endregion

            #region Lifetime management

            //services.AddHttpClient(ApiConstants.GoogleClient, client =>
            //{
            //    client.BaseAddress = new Uri("https://www.google.com");
            //}).SetHandlerLifetime(TimeSpan.FromMinutes(5));

            #endregion


            #region Forecast service

            //services.AddHttpClient<IWeatherForecastService, WeatherForecastService>(client =>
            //{
            //    client.BaseAddress = new Uri("http://api.weatherbit.io/v2.0/current");
            //}).AddHttpMessageHandler<DummyHandler>()
            //  .SetHandlerLifetime(TimeSpan.FromMinutes(10));

            #endregion
        }

        #region Helper Methods

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .OrResult(message => message.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        #endregion

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }       
    }
}
