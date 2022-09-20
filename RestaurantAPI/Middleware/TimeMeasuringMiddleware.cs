using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Exceptions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RestaurantAPI.Middleware
{
    public class TimeMeasuringMiddleware : IMiddleware
    {
        private readonly ILogger<TimeMeasuringMiddleware> _logger;
        private const int _maxMilisecs = 4000;

        public TimeMeasuringMiddleware(ILogger<TimeMeasuringMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await next.Invoke(context);
            stopwatch.Stop();

            if(stopwatch.ElapsedMilliseconds < _maxMilisecs)
            {
                var mssg = $"Request [{context.Request.Method}] at {context.Request.Path}: elapsed {stopwatch.ElapsedMilliseconds.ToString()} ms";
                _logger.LogInformation(mssg);
            }
        }
    }
}
