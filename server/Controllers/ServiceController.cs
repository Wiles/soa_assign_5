using server.Models;
using server.Sql;
using shared;
using shared.FormData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

                var context = new SoaDataContext();
                var query = from customer in context.Customers
                            from order in context.Orders
                                where customer.custID == order.custID
                            from cart in context.Carts
                                where cart.orderID == order.orderID
                            from product in context.Products
                                where product.prodID == cart.prodID
                            select new { Customer = customer, Cart = cart, Order = order, Product = product };

                var results = query.ToArray();

                int resultIndex = 0;
                foreach (var item in results)
                {
                    Logger.GetInstance().Write("Item {0}: {1}", resultIndex, item);
                    resultIndex++;
                }

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

                var delete = ServerServiceRequest.FromTableQueries(queries.Select(q => q.ToTableColumnValue()).ToList());

                new DatabaseUpdate(delete).Delete();

                return Json(new JsonSuccess("OK", "Successfully Deleted"));
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                return Json(new JsonError(ex), JsonRequestBehavior.AllowGet);
            }
        }

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
                return Json(new JsonError("Failure to insert"));
            }
        }

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
