using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace server.Controllers
{
    public class ServiceController : Controller
    {

        // GET: /Service/.*?/
        public string Index(string value)
        {
            return Index(new string[] {value});
        }

        //
        // GET: /Service/.*?/*
        public string Index(string[] values)
        {
            return String.Join(",", values);
        }

    }
}
