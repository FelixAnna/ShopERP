using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FoodShop.Manager.Api.Middlewares
{
    public class AddRequestIdToHttpContext
    {
        private readonly RequestDelegate _next;

        public AddRequestIdToHttpContext(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["slaRequestId"] = requestId;

            await _next(context);
        }
    }
}
