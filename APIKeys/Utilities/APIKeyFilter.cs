using APIKeys.Data;
using APIKeys.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIKeys.Utilities
{
    /// <summary>
    /// This class is an ActionFilter that inspects the HTTP Request for an API Key. If a valid API Key is not found, it returns a 401.
    /// NOTE: for larger applications, caching should be implemented to improve performance
    /// </summary>
    public class APIKeyFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var apiKeyIsValid = false;
            var apiKeyIsExpired = false;

            // make sure there is an X-API-KEY header
            if (context.HttpContext.Request.Headers.ContainsKey("X-API-KEY"))
            {
                // use the global ServiceCollection to get the DBContext that this method needs
                var apiKeysDB = context.HttpContext.RequestServices.GetService(typeof(APIKeysDB)) as APIKeysDB;

                if (apiKeysDB != null)
                {
                    // grap the X-API-KEY value from the header, we already confirmed that it exists
                    var apiKey = context.HttpContext.Request.Headers["X-API-KEY"];

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
                if (apiKeyIsExpired)
                {
                    context.Result = new Http401Result("API Key Expired.");
                }
                else
                {
                    context.Result = new Http401Result("Unauthorized");
                }

                return;
            }

            // Otherwise, continue processing the request
            await next();
        }
    }
}
