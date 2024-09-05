using APIKeys.Data;
using System.Net;

namespace APIKeys.Utilities
{
    /// <summary>
    /// This class is Middleware that inspects the HTTP Request for an API Key. If a valid API Key is not found, it returns a 401.
    /// NOTE: for larger applications, caching should be implemented to improve performance.
    /// </summary>
    public class APIKeyMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public APIKeyMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }   

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // assume the API Key is invalid
            var apiKeyStatus = APIKeyChecker.APIKeyStatus.Invalid;

            // make sure there is an X-API-KEY header
            if (httpContext.Request.Headers.ContainsKey("X-API-KEY"))
            {
                // use the global ServiceCollection to get the DBContext that this method needs
                var apiKeysDB = httpContext.RequestServices.GetService(typeof(APIKeysDB)) as APIKeysDB;

                // grap the X-API-KEY value from the header. We've already confirmed that it exists.
                var apiKey = httpContext.Request.Headers["X-API-KEY"];

                // determine the API Key status
                apiKeyStatus = APIKeyChecker.CheckAPIKeyStatus(apiKey, apiKeysDB);
            }

            // if the API Key is invalid, return a 401 with an "Unauthorized" message
            if (apiKeyStatus == APIKeyChecker.APIKeyStatus.Invalid)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                httpContext.Response.ContentType = "text/plain";
                await httpContext.Response.WriteAsync("Unauthorized");
                return;
            }
            // if the API Key is expired, return a 401 with an "API Key Expired" message
            else if (apiKeyStatus == APIKeyChecker.APIKeyStatus.Expired)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                httpContext.Response.ContentType = "text/plain";
                await httpContext.Response.WriteAsync("API Key Expired");
                return;
            }

            // continue processing the request
            await _requestDelegate(httpContext);
        }
    }
}
