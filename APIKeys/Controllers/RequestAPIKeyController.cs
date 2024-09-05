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

        [HttpGet(Name = "RequestNewAPIKey")]
        public async Task<string> RequestNewAPIKey()
        {
            var apiKey = Guid.NewGuid().ToString();

            var newAPIKey = new APIKey()
            {
                Key = apiKey,
                CreatedDateTime = DateTime.UtcNow,
                ExpiresDateTime = DateTime.UtcNow.AddDays(90)
                // to request an APIKey, the user should be authenticated (via Identity, OAuth2.0, etc); grab the User Id and assign it here
                // TODO: something like: AssignedUserId = IHttpContextAccessor.HttpContext.User.Identity.Name;
            };

            _apiKeysDB.Add(newAPIKey);
            await _apiKeysDB.SaveChangesAsync();

            return apiKey;
        }
    }
}
