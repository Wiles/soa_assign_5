using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace server
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoute(
                "Get",
                "service/{*values}",
                new { controller = "Service", action = "Get" },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );

            RouteTable.Routes.MapRoute(
                "Post",
                "service",
                new { controller = "Service", action = "Post" },
                new { httpMethod = new HttpMethodConstraint("POST") }
            );

            RouteTable.Routes.MapRoute(
                "Put",
                "service",
                new { controller = "Service", action = "Put" },
                new { httpMethod = new HttpMethodConstraint("PUT") }
            );

            RouteTable.Routes.MapRoute(
                "Delete",
                "service/{*values}",
                new { controller = "Service", action = "Delete" },
                new { httpMethod = new HttpMethodConstraint("DELETE") }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}