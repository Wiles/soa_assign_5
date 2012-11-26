using ClientSite.AspTools;
using ClientSite.Logic;
using shared;
using shared.FormData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ClientSite.Pages
{
    public partial class QueryPage : System.Web.UI.Page
    {
        private enum PageType
        {
            Search,
            Insert,
            Update
        }

        private PageType RequestType = PageType.Search;

        /// <summary>
        /// This is the content that is set for subsequent pages to later read.
        /// 
        /// This content is the response from the search query to the RESTFul search service
        /// </summary>
        public string SearchContent = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["type"] == null)
                {
                    throw new KeyNotFoundException();
                }

                var type = Request.QueryString["type"].ToLower();
                switch (type)
                {
                    case "search":
                        RequestType = PageType.Search;
                        break;
                    case "insert":
                        RequestType = PageType.Insert;
                        break;
                    case "update":
                        RequestType = PageType.Update;
                        break;
                }
            }
            catch (KeyNotFoundException)
            {
                // Ignore...
            }

            if (!Page.IsPostBack)
            {
                // Only perform on first page load
                SetupPage(RequestType);
            }
        }

        private void SetupPage(PageType type)
        {
            switch (type)
            {
                case PageType.Search:
                    {
                        headerLabel.Text = "Search for some data!";

                        customerRadio.Visible = false;
                        productRadio.Visible = false;
                        cartRadio.Visible = false;
                        orderRadio.Visible = false;

                        custId.ReadOnly = true;
                        prodId.ReadOnly = true;
                        orderId.ReadOnly = true;
                        orderCustId.ReadOnly = true;
                        cartOrderId.ReadOnly = true;
                        cartProdId.ReadOnly = true;
                        generatePurchaseOrder.Visible = true;
                        break;
                    }
                case PageType.Insert:
                    {
                        headerLabel.Text = "Insert some data!";

                        custId.ReadOnly = true;
                        prodId.ReadOnly = true;
                        orderId.ReadOnly = true;
                        orderCustId.ReadOnly = true;
                        cartOrderId.ReadOnly = true;
                        cartProdId.ReadOnly = true;
                        break;
                    }
                case PageType.Update:
                    {
                        headerLabel.Text = "Update some data!";
                        // Everything is accessible on update page...
                        break;
                    }
            }

            if (RequestType != PageType.Search)
            {
                customerRadio.Checked = true;
            }
        }

        protected void goBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }

        protected void execute_Click(object sender, EventArgs e)
        {
            ClientErrors.Text = "Client Errors<br>...";
            ServerErrors.Text = "Server Errors<br>...";

            // Populate data for service call
            var data = new ServerServiceRequest();

            // Customers
            try
            {
                data.Customer_CustID = (String.IsNullOrWhiteSpace(custId.Text)) ? null : (int?)int.Parse(custId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Customer custID");
            }

            data.Customer_FirstName = (String.IsNullOrWhiteSpace(firstname.Text)) ? null : firstname.Text;
            data.Customer_LastName = (String.IsNullOrWhiteSpace(lastname.Text)) ? null : lastname.Text;
            data.Customer_PhoneNumber = (String.IsNullOrWhiteSpace(phonenumber.Text)) ? null : phonenumber.Text;

            // Orders
            try
            {
                data.Order_CustID = (String.IsNullOrWhiteSpace(orderCustId.Text)) ? null : (int?)int.Parse(orderCustId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into the Order custID");
            }

            try
            {
                data.Order_OrderID = (String.IsNullOrWhiteSpace(orderId.Text)) ? null : (int?)int.Parse(orderId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Order orderID");
            }

            data.Order_OrderDate = (String.IsNullOrWhiteSpace(orderDate.Text)) ? null : orderDate.Text;
            data.Order_PoNumber = (String.IsNullOrWhiteSpace(poNumber.Text)) ? null : poNumber.Text;

            // Products
            try
            {
                data.Product_ProdID = (String.IsNullOrWhiteSpace(prodId.Text)) ? null : (int?)int.Parse(prodId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Product prodID");
            }

            try
            {
                data.Product_Price = (String.IsNullOrWhiteSpace(price.Text)) ? null : (double?)double.Parse(price.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Product price");
            }

            try
            {
                data.Product_ProdWeight = (String.IsNullOrWhiteSpace(prodWeight.Text)) ? null : (int?)int.Parse(prodWeight.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Product prodWeight");
            }

            data.Product_InStock = (soldOut.Checked) ? (byte)0 : (byte)1;
            data.Product_ProdName = prodName.Text;

            // Carts
            try
            {
                data.Cart_OrderID = (String.IsNullOrWhiteSpace(cartOrderId.Text)) ? null : (int?)int.Parse(cartOrderId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Cart orderID");
            }

            try
            {
                data.Cart_ProdID = (String.IsNullOrWhiteSpace(cartProdId.Text)) ? null : (int?)int.Parse(cartProdId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Cart prodID");
            }

            try
            {
                data.Cart_Quantity = (String.IsNullOrWhiteSpace(quantity.Text)) ? null : (int?)int.Parse(quantity.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Cart quantity");
            }

            // Bail out, if we have errors
            if (HasClientErrors())
            {
                return;
            }

            // Make sure that the user has not selected both customers and products
            var hasCustomerField = (data.Customer_CustID != null &&
                data.Customer_FirstName != null &&
                data.Customer_LastName != null &&
                data.Customer_PhoneNumber != null);

            var hasProductField = (data.Product_ProdID != null &&
                data.Product_ProdName != null &&
                data.Product_ProdWeight != null &&
                data.Product_Price != null
                // Ignore the instock field...
            );

            if (hasCustomerField && hasProductField)
            {
                AppendClientError("Cannot have <b>both</b> customer and product sections filled");
                return;
            }

            if (generatePurchaseOrder.Checked)
            {
                if (data.Customer_CustID == null && data.Customer_LastName == null &&
                    data.Customer_FirstName == null && data.Order_OrderID == null &&
                    data.Order_PoNumber == null && data.Order_OrderDate == null
                    )
                {
                    AppendClientError(@"At least one of the following fields must be filled: Customer custID, Customer lastName, <br>
Customer firstName, Order orderID, Order poNumber or Order orderDate when 'Generate P.O.' is checked");
                    return;
                }
            }

            switch (RequestType)
            {
                case PageType.Search:
                    {
                        var url = new Uri(ClientConfiguration.ServerUrl.ToString() + data.ToUrl());
                        var client = HttpWebRequest.Create(url);
                        client.GetRequestStream().Close();

                        using (var responseStream = client.GetResponse().GetResponseStream())
                        using (var reader = new StreamReader(responseStream))
                        {
                            SearchContent = reader.ReadToEnd();

                            if (generatePurchaseOrder.Checked)
                            {
                                Server.Transfer("/Page/PurchaseOrderPage.aspx");
                            }
                            else
                            {
                                Server.Transfer("/Page/SearchResultsPage.aspx");
                            }
                        }

                        break;
                    }
                case PageType.Insert:
                    {
                        var url = ClientConfiguration.ServerUrl;
                        var client = HttpWebRequest.Create(url);
                        client.Method = "POST";

                        var js = new JavaScriptSerializer();
                        var json = js.Serialize(data);
                        var requestContent = Encoding.ASCII.GetBytes(json);

                        client.ContentLength = requestContent.Length;

                        var requestStream = client.GetRequestStream();
                        requestStream.Write(requestContent, 0, requestContent.Length);
                        requestStream.Close();

                        using (var responseStream = client.GetResponse().GetResponseStream())
                        using (var reader = new StreamReader(responseStream))
                        {
                            var responseContent = reader.ReadToEnd();

                            try
                            {
                                var success = js.Deserialize<JsonSuccess>(responseContent);
                                WebFormTools.MessageBoxShow(this, success.Message);
                            }
                            catch (Exception)
                            {
                                var error = js.Deserialize<JsonError>(responseContent);
                                AppendServerError(error.Message);
                            }
                        }

                        break;
                    }
                case PageType.Update:
                    {
                        var url = ClientConfiguration.ServerUrl;
                        var client = HttpWebRequest.Create(url);
                        client.Method = "PUT";

                        var js = new JavaScriptSerializer();
                        var json = js.Serialize(data);
                        var requestContent = Encoding.ASCII.GetBytes(json);

                        client.ContentLength = requestContent.Length;

                        using (Stream requestStream = client.GetRequestStream())
                        {
                            requestStream.Write(requestContent, 0, requestContent.Length);
                        }

                        using (var responseStream = client.GetResponse().GetResponseStream())
                        using (var reader = new StreamReader(responseStream))
                        {
                            var responseContent = reader.ReadToEnd();

                            try
                            {
                                var success = js.Deserialize<JsonSuccess>(responseContent);
                                WebFormTools.MessageBoxShow(this, success.Message);
                            }
                            catch (Exception)
                            {
                                var error = js.Deserialize<JsonError>(responseContent);
                                AppendServerError(error.Message);
                            }
                        }

                        break;
                    }
            }
        }

        protected void exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://www.google.ca");
        }

        protected void generatePurchaseOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (generatePurchaseOrder.Checked)
            {
                DisableAllInputs();
                orderId.ReadOnly = false;
                custId.ReadOnly = false;
                lastname.ReadOnly = false;
                firstname.ReadOnly = false;
                poNumber.ReadOnly = false;
                orderDate.ReadOnly = false;
            }
            else
            {
                SetupPage(RequestType);
            }
        }

        protected void customerRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabledRows();
        }

        protected void productRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabledRows();
        }

        protected void orderRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabledRows();
        }

        protected void cartRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabledRows();
        }

        private void DisableAllInputs()
        {
            foreach (var form in Controls)
            {
                if (form is HtmlForm)
                {
                    foreach (var control in ((HtmlForm)form).Controls)
                    {
                        if (control is TextBox)
                        {
                            var textbox = (TextBox)control;
                            textbox.ReadOnly = true;
                        }
                    }
                }
            }

            soldOut.Enabled = false;
        }

        private void UpdateEnabledRows()
        {
            if (this.RequestType == PageType.Search)
            {
                return;
            }

            if (customerRadio.Checked)
            {
                DisableAllInputs();
                custId.ReadOnly = false;
                firstname.ReadOnly = false;
                lastname.ReadOnly = false;
                phonenumber.ReadOnly = false;
            }
            else if (productRadio.Checked)
            {
                DisableAllInputs();
                prodId.ReadOnly = false;
                prodName.ReadOnly = false;
                price.ReadOnly = false;
                prodWeight.ReadOnly = false;
                soldOut.Enabled = true;
            }
            else if (orderRadio.Checked)
            {
                DisableAllInputs();
                orderCustId.ReadOnly = false;
                orderId.ReadOnly = false;
                poNumber.ReadOnly = false;
                orderDate.ReadOnly = false;
            }
            else if (cartRadio.Checked)
            {
                DisableAllInputs();
                cartOrderId.ReadOnly = false;
                cartProdId.ReadOnly = false;
                quantity.ReadOnly = false;
            }
        }


        private bool HasClientErrors()
        {
            return ClientErrors.Text.Contains('-');
        }

        private void AppendClientError(string message)
        {
            ClientErrors.Text += "<br>- " + message;
        }

        private void AppendServerError(string message)
        {
            ServerErrors.Text += "<br>- " + message;
        }
    }
}