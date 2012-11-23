using server.Models;
using server.Sql;
using shared;
using shared.FormData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace server.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/.*?/*
        public JsonResult Get(string[] values)
        {
            try
            {
                if (values == null)
                {
                    throw new Exception("No table/column/values given for GET");
                }

                values = values[0].Split('/');


                var queries = TableQuery.ListQueriesFromPath(values);

                throw new Exception("Failure to return JsonResult");
            }
            catch (Exception ex)
            {
                return Json(new JsonError(ex), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(string[] values)
        {
            try
            {
                if (values == null)
                {
                    throw new Exception("No table/column/values given for DELETE");
                }

                values = values[0].Split('/');


                var queries = TableQuery.ListQueriesFromPath(values);

                throw new Exception("Failure to return JsonResult");
            }
            catch (Exception ex)
            {
                return Json(new JsonError(ex), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Post([FromBody] string json)
        {
            try
            {
                var js = new JavaScriptSerializer();
                var insert = js.Deserialize<Screen2Data>(json);

                new DatabaseUpdate(insert).Insert();

                return Json(new JsonSuccess("OK", "Successfully Inserted"));
            }
            catch (Exception)
            {
                return Json(new JsonError("Failure to insert"));
            }
        }

        public JsonResult Put([FromBody] string json)
        {
            try
            {
                var js = new JavaScriptSerializer();
                var update = js.Deserialize<Screen2Data>(json);

                new DatabaseUpdate(update).Update();

                return Json(new JsonSuccess("OK", "Successfully Updated"));
            }
            catch (Exception)
            {
                return Json(new JsonError("Failure to update"));
            }
        }

    }
}
