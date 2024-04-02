using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.Filters
{
    public class ApiExceptionFilter : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response
                .WriteAsJsonAsync(new { Errors = new { Detail = "Bad Request"}}, cancellationToken);

            return true;
        }
    }
}
