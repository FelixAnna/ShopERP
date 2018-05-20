using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FoodShop.Manager.Api.Middlewares
{
    public class GlobalExceptionLogger
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionLogger(ILogger<GlobalExceptionLogger> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured");
                throw;
            }
        }
    }
}
