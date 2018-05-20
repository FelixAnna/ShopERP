using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Manager.Api.Middlewares
{
    public class AddUserIdentifierToHttpContext
    {
        private readonly RequestDelegate _next;

        public AddUserIdentifierToHttpContext(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Items["ImpersonationDetails"] = GetImpersonationDetails(context.User);

            await _next(context);
        }

        private string GetImpersonationDetails(ClaimsPrincipal user)
        {
            if (user == null || user.Claims == null || !user.Claims.Any() ||
                string.IsNullOrWhiteSpace(user.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Environment.NewLine + "(Authentication - User: anonymous)";
            }

            var sb = new StringBuilder(Environment.NewLine + "(Authentication - ");

            // "ipb" from ApplicationOAuthProvider.ImpersonationClaimType
            var claim = user.Claims.SingleOrDefault(c => c.Type == "ipb");
            if (claim != null)
            {
                sb.AppendFormat("Impersonator: {0}, ", claim.Value);
            }
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            sb.AppendFormat("User: {0}", userId);

            return sb + ")";
        }
    }
}
