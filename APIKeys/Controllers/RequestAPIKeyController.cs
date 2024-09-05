using APIKeys.Data;
using APIKeys.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIKeys.Controllers
{
    /// <summary>
    /// This controller is not protected by an API Key because it is used to request an API Key
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RequestAPIKeyController : Controller
    {
        private readonly APIKeysDB _apiKeysDB;

        public RequestAPIKeyController(APIKeysDB apiKeysDB) 
        { 
            _apiKeysDB = apiKeysDB;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            // the API Key should be unique... we'll just use a GUID
            var apiKey = Guid.NewGuid().ToString();

            var newAPIKey = new APIKey()
            {
                Key = apiKey,
                CreatedDateTime = DateTime.UtcNow,
                ExpiresDateTime = DateTime.UtcNow.AddDays(90)

                // to request an APIKey, the user should already be authenticated (via Identity, OAuth2.0, etc)
                // In a real world application, use the Authorization infrastructure to lookup a User Id and assign it here
                // TODO: AssignedUserId = IHttpContextAccessor.HttpContext.User.Identity.Name;
            };

            _apiKeysDB.Add(newAPIKey);
            await _apiKeysDB.SaveChangesAsync();

            return apiKey;
        }
    }
}
