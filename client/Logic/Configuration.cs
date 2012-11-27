using System;
using System.Configuration;

namespace ClientSite.Logic
{
    public class ClientConfiguration
    {
        /// <summary>
        /// Gets the server URL.
        /// </summary>
        /// <value>
        /// The server URL.
        /// </value>
        public static Uri ServerUrl
        {
            get
            {
                return new Uri(ConfigurationSettings.AppSettings["ServerHostUrl"].ToString());
            }
        }
    }
}