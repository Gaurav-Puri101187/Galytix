using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Galytix.Infra
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(
            RequestDelegate next,
            ILogger<GlobalExceptionHandler> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                HandleException(httpContext, ex);
            }
        }

        private void HandleException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
