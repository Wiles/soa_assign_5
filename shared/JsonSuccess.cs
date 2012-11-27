using System;

namespace shared
{
    [Serializable]
    public class JsonSuccess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSuccess" /> class.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public JsonSuccess(string status, string message)
        {
            this.Status = status;
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }
    }
}
