using FoodShop.Manager.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FoodShop.Manager.Api.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    await RewriteResponseAsync(context,
                                                (int)HttpStatusCode.Unauthorized,
                                                WsConstants.UnauthorizedErrorType,
                                                WsConstants.UnauthorizedErrorMessage);
                }
                else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    await RewriteResponseAsync(context,
                                                (int)HttpStatusCode.Forbidden,
                                                WsConstants.ForbiddenErrorType,
                                                WsConstants.ForbiddenErrorMessage);
                }
            }
            catch
            {
                await RewriteResponseAsync(context,
                    (int)HttpStatusCode.InternalServerError,
                    WsConstants.GeneralErrorType,
                    WsConstants.GeneralErrorMessage);
            }
        }

        private async Task RewriteResponseAsync(HttpContext context, int status, string code, string message)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                return;
            }

            var error = new Dictionary<string, object>
                {
                    {"ErrorCode", code},
                    {"Message", message}
                };

            context.Response.Clear();
            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }
    }
}
