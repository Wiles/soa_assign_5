using ClientSite.AspTools;
using ClientSite.Logic;
using shared;
using shared.FormData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

        public SearchResult SearchResult;

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
                SetupPage(RequestType, true);
            }
        }

        private void SetupPage(PageType type, bool first = false)
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

                        GeneratePurchaseOrder.Visible = true;
                        break;
                    }
                case PageType.Insert:
                    {
                        headerLabel.Text = "Insert some data!";

                        CustId.ReadOnly = true;
                        ProdId.ReadOnly = true;
                        OrderId.ReadOnly = true;
                        break;
                    }
                case PageType.Update:
                    {
                        headerLabel.Text = "Update some data!";
                        // Everything is accessible on update page...
                        break;
                    }
            }

            if (RequestType != PageType.Search && first)
            {
                customerRadio.Checked = true;
                UpdateEnabledRows();
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
            PopulateFields(data);

            // Bail out, if we have errors
            if (HasClientErrors())
            {
                return;
            }

            // Make sure that the user has not selected both customers and products
            var hasCustomerField = (data.Customer_CustID != null ||
                data.Customer_FirstName != null ||
                data.Customer_LastName != null ||
                data.Customer_PhoneNumber != null);

            var hasProductField = (data.Product_ProdID != null ||
                data.Product_ProdName != null ||
                data.Product_ProdWeight != null ||
                data.Product_Price != null
                // Ignore the instock field...
            );

            if (hasCustomerField && hasProductField)
            {
                AppendClientError("Cannot have <b>both</b> customer and product fields filled");
                return;
            }

            if (GeneratePurchaseOrder.Checked)
            {
                // Different fields are mandatory when generate P.O. is checked
                if (data.Customer_CustID == null && data.Customer_LastName == null &&
                    data.Customer_FirstName == null && data.Order_OrderID == null &&
                    data.Order_PoNumber == null && data.Order_OrderDate == null)
                {
                    AppendClientError(@"At least one of the following fields must be filled: Customer custID, Customer lastName, <br>
Customer firstName, Order orderID, Order poNumber or Order orderDate when 'Generate P.O.' is checked");
                    return;
                }
            }

            if (!data.HasOneOrMoreFieldsWithAValue())
            {
                AppendClientError("Please fill in at least one of the fields");
                return;
            }

            if (RequestType == PageType.Insert)
            {
                if (!EnforceRequiredInsertFieldsFilled(data))
                {
                    return;
                }
            }

            // Check maximum lengths, because Linq to SQL generates crappy messages on the server end
            DateTime parsed;
            if (data.Customer_PhoneNumber != null && !Regex.Match(data.Customer_PhoneNumber, @"^\d{3}-\d{3}-\d{4}$").Success)
            {
                AppendClientError("Please use the following format on phone numbers (xxx-xxx-xxxx)");
                return;
            }
            else if (data.Order_OrderDate != null && !DateTime.TryParseExact(data.Order_OrderDate, "MM-dd-yy",
                CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed))
            {
                AppendClientError("Please use the following format for order dates (MM-DD-YY) and proper dates");
                return;
            }

            switch (RequestType)
            {
                case PageType.Search:
                    {
                        var purchaseOrder = GeneratePurchaseOrder.Checked;

                        var url = new Uri(ClientConfiguration.ServerUrl.ToString() + data.ToUrl(purchaseOrder));
                        var client = HttpWebRequest.Create(url);
                        client.Method = "GET";

                        using (var responseStream = client.GetResponse().GetResponseStream())
                        using (var reader = new StreamReader(responseStream))
                        {
                            SearchContent = reader.ReadToEnd();

                            try
                            {
                                var js = new JavaScriptSerializer();
                                var result = js.Deserialize<SearchResult>(SearchContent);

                                SearchResult = result;

                                if (purchaseOrder)
                                {
                                    Server.Transfer("/Pages/PurchaseOrderPage.aspx");
                                }
                                else
                                {
                                    Server.Transfer("/Pages/SearchResultsPage.aspx");
                                }
                            }
                            catch (Exception)
                            {
                                // Check if we have a Json Error
                                var js = new JavaScriptSerializer();
                                var error = js.Deserialize<JsonError>(SearchContent);

                                AppendServerError(error.Message);
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

        private void PopulateFields(ServerServiceRequest data)
        {

            // Customers
            try
            {
                data.Customer_CustID = (String.IsNullOrWhiteSpace(CustId.Text)) ? null : (int?)int.Parse(CustId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Customer custID");
            }

            data.Customer_FirstName = (String.IsNullOrWhiteSpace(FirstName.Text)) ? null : FirstName.Text;
            data.Customer_LastName = (String.IsNullOrWhiteSpace(LastName.Text)) ? null : LastName.Text;
            data.Customer_PhoneNumber = (String.IsNullOrWhiteSpace(Phonenumber.Text)) ? null : Phonenumber.Text;

            // Orders
            try
            {
                data.Order_CustID = (String.IsNullOrWhiteSpace(OrderCustId.Text)) ? null : (int?)int.Parse(OrderCustId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into the Order custID");
            }

            try
            {
                data.Order_OrderID = (String.IsNullOrWhiteSpace(OrderId.Text)) ? null : (int?)int.Parse(OrderId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Order orderID");
            }

            data.Order_OrderDate = (String.IsNullOrWhiteSpace(OrderDate.Text)) ? null : OrderDate.Text;
            data.Order_PoNumber = (String.IsNullOrWhiteSpace(PoNumber.Text)) ? null : PoNumber.Text;

            // Products
            try
            {
                data.Product_ProdID = (String.IsNullOrWhiteSpace(ProdId.Text)) ? null : (int?)int.Parse(ProdId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Product prodID");
            }

            try
            {
                data.Product_Price = (String.IsNullOrWhiteSpace(Price.Text)) ? null : (double?)double.Parse(Price.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Product price");
            }

            try
            {
                data.Product_ProdWeight = (String.IsNullOrWhiteSpace(ProdWeight.Text)) ? null : (int?)int.Parse(ProdWeight.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Product prodWeight");
            }

            if (RequestType != PageType.Search)
            {
                data.Product_InStock = (SoldOut.Checked) ? (byte)0 : (byte)1;
            }
            else
            {
                // Ignore the instock field when performing search
                data.Product_InStock = null;
            }

            data.Product_ProdName = (String.IsNullOrWhiteSpace(ProdName.Text)) ? null : ProdName.Text;

            // Carts
            try
            {
                data.Cart_OrderID = (String.IsNullOrWhiteSpace(CartOrderId.Text)) ? null : (int?)int.Parse(CartOrderId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Cart orderID");
            }

            try
            {
                data.Cart_ProdID = (String.IsNullOrWhiteSpace(CartProdId.Text)) ? null : (int?)int.Parse(CartProdId.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Cart prodID");
            }

            try
            {
                data.Cart_Quantity = (String.IsNullOrWhiteSpace(Quantity.Text)) ? null : (int?)int.Parse(Quantity.Text);
            }
            catch (Exception)
            {
                AppendClientError("Please enter a number into Cart quantity");
            }
        }

        private bool EnforceRequiredInsertFieldsFilled(ServerServiceRequest data)
        {
            bool missingMandatoryField = false;
            if (customerRadio.Checked)
            {
                if (data.Customer_LastName == null)
                {
                    AppendClientError("Customer must have a lastName");
                    missingMandatoryField = true;
                }

                if (data.Customer_PhoneNumber == null)
                {
                    AppendClientError("Customer must have a phoneNumber");
                    missingMandatoryField = true;
                }
            }
            else if (productRadio.Checked)
            {
                if (data.Product_ProdName == null)
                {
                    AppendClientError("Product must have a prodName");
                    missingMandatoryField = true;
                }

                if (data.Product_Price == null)
                {
                    AppendClientError("Product must have a price");
                    missingMandatoryField = true;
                }

                if (data.Product_ProdWeight == null)
                {
                    AppendClientError("Product must have a prodWeight");
                    missingMandatoryField = true;
                }
            }
            else if (orderRadio.Checked)
            {
                if (data.Order_CustID == null)
                {
                    AppendClientError("Order must have a custID");
                    missingMandatoryField = true;
                }

                if (data.Order_OrderDate == null)
                {
                    AppendClientError("Order must have a orderDate");
                    missingMandatoryField = true;
                }
            }
            else if (cartRadio.Checked)
            {
                if (data.Cart_OrderID == null)
                {
                    AppendClientError("Cart must have a orderID");
                    missingMandatoryField = true;
                }

                if (data.Cart_ProdID == null)
                {
                    AppendClientError("Cart must have a prodID");
                    missingMandatoryField = true;
                }

                if (data.Cart_Quantity == null)
                {
                    AppendClientError("Cart must have a quantity");
                    missingMandatoryField = true;
                }
            }

            return !missingMandatoryField;
        }

        protected void exit_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.HomePage);
        }

        protected void generatePurchaseOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (GeneratePurchaseOrder.Checked)
            {
                DisableAllInputs();
                OrderId.ReadOnly = false;
                CustId.ReadOnly = false;
                LastName.ReadOnly = false;
                FirstName.ReadOnly = false;
                PoNumber.ReadOnly = false;
                OrderDate.ReadOnly = false;
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

            SoldOut.Enabled = false;
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
                CustId.ReadOnly = false;
                FirstName.ReadOnly = false;
                LastName.ReadOnly = false;
                Phonenumber.ReadOnly = false;
            }
            else if (productRadio.Checked)
            {
                DisableAllInputs();
                ProdId.ReadOnly = false;
                ProdName.ReadOnly = false;
                Price.ReadOnly = false;
                ProdWeight.ReadOnly = false;
                SoldOut.Enabled = true;
            }
            else if (orderRadio.Checked)
            {
                DisableAllInputs();
                OrderCustId.ReadOnly = false;
                OrderId.ReadOnly = false;
                PoNumber.ReadOnly = false;
                OrderDate.ReadOnly = false;
            }
            else if (cartRadio.Checked)
            {
                DisableAllInputs();
                CartOrderId.ReadOnly = false;
                CartProdId.ReadOnly = false;
                Quantity.ReadOnly = false;
            }

            SetupPage(RequestType);
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