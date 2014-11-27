namespace Ovh.RestLib.Models.Response
{
    /// <summary>
    /// o authentication response
    /// </summary>
    internal class OAuthResponse
    {
        /// <summary>
        /// Gets or sets the validation URL.
        /// </summary>
        /// <value>
        /// The validation URL.
        /// </value>
        public string ValidationUrl { get; set; }
        /// <summary>
        /// Gets or sets the consumer key.
        /// </summary>
        /// <value>
        /// The consumer key.
        /// </value>
        public string ConsumerKey { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }
    }
}
