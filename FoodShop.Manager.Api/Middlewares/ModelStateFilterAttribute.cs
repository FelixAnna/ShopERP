using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodShop.Manager.Api.Middlewares
{
    /// <summary>
    /// Valid Model State, transfer to exception if it is invalid
    /// </summary>
    public class ModelStateFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger _log;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log"></param>
        public ModelStateFilterAttribute(ILogger<ModelStateFilterAttribute> log)
        {
            _log = log;
        }

        /// <summary>
        /// Valid Model State, transfer to exception if it is invalid
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            // If the binding models are invalid, bail
            var modelState = actionContext.ModelState;

            if (!modelState.IsValid)
            {
                var sb = modelState // filter for modelstate errors & accumulate error details
                    .Where(kvp => kvp.Value.Errors.Any())
                    .Aggregate(new StringBuilder("Request contains one or more parameter errors"),
                        (sbAgg, kvp) => sbAgg.AppendFormat("\t{0} Message: {1}",
                            kvp.Key,
                            string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage)))
                    );

                _log.LogWarning(sb.ToString());

                var error = new Dictionary<string, object>
                    {
                        { "error", "invalid_parameter"},
                        { "error_description", "There is a problem with your configuration, please contact support team" },
                        { "param_name", "" }
                    };
                actionContext.Result = new BadRequestObjectResult(error);
            }
        }
    }
}
