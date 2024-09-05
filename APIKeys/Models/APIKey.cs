using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIKeys.Models
{
    /// <summary>
    /// Simple API Key class
    /// </summary>
    public class APIKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Encrypted API Key
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// DateTime in UTC the API Key was created
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// DateTime in UTC the API Key will expire
        /// </summary>
        public DateTime ExpiresDateTime { get; set; }

        /// <summary>
        /// UserId the API Key is associated with
        /// </summary>
        public int AssignedToUserId { get; set; }
    }
}
