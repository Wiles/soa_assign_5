using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace server.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Default1/

        public string Index(string[] values)
        {
            return String.Join(",", values);
        }

    }
}
