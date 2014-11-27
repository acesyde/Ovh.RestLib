using Newtonsoft.Json;

namespace Ovh.RestLib.Models.Request
{
    /// <summary>
    /// o authentication request
    /// </summary>
    internal class OAuthRequest
    {
        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [JsonProperty("accessRules")]
        public AccessRules[] AccessRules { get; set; }
        /// <summary>
        /// Gets or sets the redirection.
        /// </summary>
        /// <value>
        /// The redirection.
        /// </value>
        [JsonProperty("redirection")]
        public string Redirection { get; set; }
    }

    /// <summary>
    /// Access rules
    /// </summary>
    internal class AccessRules
    {
        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        [JsonProperty("method")]
        public string Method { get; set; }
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [JsonProperty("path")]
        public string Path { get; set; }
    }
}
