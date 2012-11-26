using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ClientSite.Logic
{
    public class ClientConfiguration
    {
        public static Uri ServerUrl
        {
            get
            {
                return new Uri(ConfigurationSettings.AppSettings["ServerHostUrl"].ToString());
            }
        }
    }
}