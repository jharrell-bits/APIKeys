using APIKeys.Data;

namespace APIKeys.Utilities
{
    /// <summary>
    /// This class exposes methods to calculate the API Key's status
    /// </summary>
    public static class APIKeyChecker
    {        
        /// <summary>
        /// List of possible statuses for an API Key
        /// </summary>
        public enum APIKeyStatus
        {
            Invalid = 0,
            Expired = 1,
            Valid = 2
        }

        /// <summary>
        /// Check the API Key
        /// </summary>
        /// <param name="apiKey">value of the API Key to test</param>
        /// <param name="apiKeysDB">APIKeysDB DbContext used to check the API Key</param>
        /// <returns>Status of the API Key</returns>
        public static APIKeyStatus CheckAPIKeyStatus(string? apiKey, APIKeysDB? apiKeysDB)
        {
            var apiKeyStatus = APIKeyStatus.Invalid;

            if (apiKeysDB != null && !string.IsNullOrEmpty(apiKey))
            {
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
                        apiKeyStatus = APIKeyStatus.Expired;
                    }
                    else
                    {
                        // API Key is valid
                        apiKeyStatus = APIKeyStatus.Valid;
                    }
                }
            }

            return apiKeyStatus;
        }
    }
}
