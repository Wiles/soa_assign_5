using ClientSite.Logic;
using ClientSite.Sql;
using shared;
using shared.FormData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientSite.Pages
{
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Populate the carts drop down
                    var context = new SoaDataContext();
                    var labels = context.Carts.Where(c => c.deleted == (byte)0)
                        .Select(c => String.Format("{0},{1}", c.orderID, c.prodID)).ToList();

                    CartDropDown.Items.Clear();
                    foreach (var label in labels)
                    {
                        CartDropDown.Items.Add(label);
                    }

                    SetupDeleteButtons();

                    if (Request.QueryString["response"] != null)
                    {
                        ServerResponseText.Text = Request.QueryString["response"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                throw;
            }
        }

        private void SetupDeleteButtons()
        {
            try
            {
                CustomerDelete.Enabled = CustomerDropDown.Items.Count != 0;
                OrderDelete.Enabled = OrderDropDown.Items.Count != 0;
                ProductDelete.Enabled = ProductDropDown.Items.Count != 0;
                CartDelete.Enabled = CartDropDown.Items.Count != 0;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                throw;
            }
        }

        protected void CustomerDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var data = new ServerServiceRequest();
                data.Customer_CustID = int.Parse(CustomerDropDown.SelectedItem.Value);

                IssueServiceDelete(data);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                throw new Exception("Failure to process Customer Drop Down");
            }
        }

        protected void OrderDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var data = new ServerServiceRequest();
                data.Order_OrderID = int.Parse(OrderDropDown.SelectedItem.Value);

                IssueServiceDelete(data);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                throw new Exception("Failure to process Order Drop Down");
            }
        }

        protected void ProductDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var data = new ServerServiceRequest();
                data.Product_ProdID = int.Parse(ProductDropDown.SelectedItem.Value);

                IssueServiceDelete(data);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                throw new Exception("Failure to process Product Drop Down");
            }
        }

        protected void CartDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var data = new ServerServiceRequest();
                var split = CartDropDown.SelectedItem.Text.Split(',');
                data.Cart_OrderID = int.Parse(split[0]);
                data.Cart_ProdID = int.Parse(split[1]);

                IssueServiceDelete(data);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
                throw new Exception("Failure to process Cart Drop Down");
            }
        }

        private void IssueServiceDelete(ServerServiceRequest data)
        {
            var js = new JavaScriptSerializer();

            var url = new Uri(ClientConfiguration.ServerUrl.ToString() + data.ToUrl());
            var client = HttpWebRequest.Create(url);
            client.Method = "DELETE";
            client.GetRequestStream().Close();

            using (var responseStream = client.GetResponse().GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                var content = reader.ReadToEnd();

                try
                {
                    var result = js.Deserialize<JsonSuccess>(content);
                    var responseText = "Server Response: -" + result.Message;
                    Response.Redirect("/Pages/DeletePage.aspx?response=" + responseText);
                }
                catch (Exception)
                {
                    var error = js.Deserialize<JsonError>(content);
                    var responseText = "Server Response: -" + error.Message;
                    Response.Redirect("/Pages/DeletePage.aspx?response=" + responseText);
                }
            }
        }

        protected void GoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }

        protected void Exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://www.google.ca");
        }

        protected void CustomerDropDown_DataBound(object sender, EventArgs e)
        {
            SetupDeleteButtons();
        }

        protected void OrderDropDown_DataBound(object sender, EventArgs e)
        {
            SetupDeleteButtons();
        }

        protected void ProductDropDown_DataBound(object sender, EventArgs e)
        {
            SetupDeleteButtons();
        }
    }
}