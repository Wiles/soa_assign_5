﻿using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using server.Models;
using server.Sql;
using shared;
using shared.FormData;

namespace server.Controllers
{
    public class ServiceController : Controller
    {
        /// <summary>
        /// Handler for a get (search) request.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The JSON encoded request results</returns>
        public JsonResult Get(string[] values)
        {
            try
            {
                if (values == null)
                {
                    throw new Exception("No table/column/values given for GET");
                }

                values = values[0].Split('/');

                bool purchaseOrder = false;
                if (values[0].Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    purchaseOrder = true;
                    // Remove the purchase order item
                    values = values.Skip(1).ToArray();
                }
                else if (values[0].Equals("false", StringComparison.CurrentCultureIgnoreCase))
                {
                    purchaseOrder = false;
                    // Remove the purchase order item
                    values = values.Skip(1).ToArray();
                }

                var queries = TableQuery.ListQueriesFromPath(values);
                var data = ServerServiceRequest.FromTableQueries(queries.Select(q => q.ToTableColumnValue()).ToList());

                var searcher = new DatabaseSearch(data, purchaseOrder);
                var results = searcher.Search();

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                return Json(new JsonError(ex), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Handler for a delete request
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The JSON encoded delete request results</returns>
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
                var data = ServerServiceRequest.FromTableQueries(queries.Select(q => q.ToTableColumnValue()).ToList());

                new DatabaseUpdate(data).Delete();

                return Json(new JsonSuccess("OK", "Successfully Deleted"));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                return Json(new JsonError(ex), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Handler for a post (insert) request
        /// </summary>
        /// <returns>The JSON encoded post response</returns>
        [System.Web.Mvc.HttpPost]
        public JsonResult Post()
        {
            var input = Request.InputStream;
            input.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(input).ReadToEnd();

            if (String.IsNullOrWhiteSpace(json))
            {
                return Json(new JsonError("Request body must not be null"));
            }

            try
            {
                var js = new JavaScriptSerializer();
                var insert = js.Deserialize<ServerServiceRequest>(json);

                new DatabaseUpdate(insert).Insert();

                return Json(new JsonSuccess("OK", "Successfully Inserted"));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                return Json(new JsonError("Failure to insert: " + ex.Message));
            }
        }

        /// <summary>
        /// Handler for a put (update) request
        /// </summary>
        /// <returns>The JSON encoded put response</returns>
        [System.Web.Mvc.HttpPut]
        public JsonResult Put()
        {
            var input = Request.InputStream;
            input.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(input).ReadToEnd();

            if (json == null)
            {
                return Json(new JsonError("Request body must not be null"));
            }

            try
            {
                var js = new JavaScriptSerializer();
                var update = js.Deserialize<ServerServiceRequest>(json);

                new DatabaseUpdate(update).Update();

                return Json(new JsonSuccess("OK", "Successfully Updated"));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                return Json(new JsonError("Failure to update: " + ex.Message));
            }
        }

    }
}
