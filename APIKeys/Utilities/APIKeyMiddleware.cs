using APIKeys.Data;
using APIKeys.Models;
using System.Net;

namespace APIKeys.Utilities
{
    public class APIKeyMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public APIKeyMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }   

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var apiKeyIsValid = false;
            var apiKeyIsExpired = false;

            // make sure there is an X-API-KEY header
            if (httpContext.Request.Headers.ContainsKey("X-API-KEY"))
            {
                // use the global ServiceCollection to get the DBContext that this method needs
                var apiKeysDB = httpContext.RequestServices.GetService(typeof(APIKeysDB)) as APIKeysDB;

                if (apiKeysDB != null)
                {
                    // grap the X-API-KEY value
                    var apiKey = httpContext.Request.Headers["X-API-KEY"];

                    // search the DB for a matching API Key
                    // NOTE: if the application uses Authentication (Identity, OAuth2.0, etc), filter on the user id also
                    var matchingApiKey = apiKeysDB.APIKeys.FirstOrDefault(f => f.Key == apiKey);

                    // Found a matching key
                    if (matchingApiKey != null)
                    {
                        // make sure the key is not expired
                        if (matchingApiKey.ExpiresDateTime < DateTime.UtcNow)
					    {
                            // If API Key is expired, we will notify the user
                            apiKeyIsExpired = true;
                        }
                        else
                        {
                            // API Key is valid
                            apiKeyIsValid = true;
                        }
                    }
                }
            }

            // if the API Key is invalid, return a 401
            if (!apiKeyIsValid)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                httpContext.Response.ContentType = "text/plain";

                if (apiKeyIsExpired)
                {
                    await httpContext.Response.WriteAsync("API Key Expired");
                }
                else
                {
                    await httpContext.Response.WriteAsync("Unauthorized");
                }
            }

            await _requestDelegate(httpContext);
        }
    }
}
