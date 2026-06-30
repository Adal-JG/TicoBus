using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicoBus.SI.Filters
{
    public class ApiKeyFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;

        public ApiKeyFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var apiKeyConfig = _configuration["ApiKey"];

            if (!context.HttpContext.Request.Headers.TryGetValue("X-API-KEY", out var apiKeyHeader))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    mensaje = "API Key no enviada."
                });
                return;
            }

            if (apiKeyHeader != apiKeyConfig)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    mensaje = "API Key inválida."
                });
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}