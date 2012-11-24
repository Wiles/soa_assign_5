using ClientSite.AspTools;
using shared.FormData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientSite.Pages
{
    public partial class InsertPage : System.Web.UI.Page
    {
        private const string Url = "http://127.0.0.1/service/";
        private enum PageType
        {
            Search,
            Insert,
            Update,
            Delete
        }

        private PageType RequestType = PageType.Search;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
                    case "delete":
                        RequestType = PageType.Delete;
                        break;
                }
            }
            catch (KeyNotFoundException)
            {
                // Ignore...
            }

            SetupPage(RequestType);
        }

        private void SetupPage(PageType type)
        {
            switch (type)
            {
                case PageType.Search:
                    {
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
                        // Everything accessible on update page...
                        break;
                    }
                case PageType.Delete:
                    {
                        foreach (var control in this.Controls)
                        {
                            if (control is TextBox)
                            {
                                var textbox = (TextBox)control;
                                textbox.ReadOnly = true;
                            }
                        }

                        custId.ReadOnly = false;
                        prodId.ReadOnly = false;
                        orderId.ReadOnly = false;
                        orderCustId.ReadOnly = false;
                        cartOrderId.ReadOnly = false;
                        cartProdId.ReadOnly = false;

                        break;
                    }
            }
        }

        protected void goBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void execute_Click(object sender, EventArgs e)
        {
            var data = new Screen2Data();
            // Populate data

            try
            {
                data.Customer_CustID = (String.IsNullOrWhiteSpace(custId.Text)) ? null : (int?)int.Parse(custId.Text);
            }
            catch (Exception)
            {
                WebFormTools.MessageBoxShow(this, "Please enter a number into Customer custID");
                return;
            }

            data.Customer_Firstname = (String.IsNullOrWhiteSpace(firstname.Text)) ? null : firstname.Text;
            data.Customer_Lastname = (String.IsNullOrWhiteSpace(lastname.Text)) ? null : lastname.Text;
            data.Customer_PhoneNumber = (String.IsNullOrWhiteSpace(phonenumber.Text)) ? null : phonenumber.Text;
            
            try
            {
                data.Order_CustID = (String.IsNullOrWhiteSpace(orderCustId.Text)) ? null : (int?)int.Parse(orderCustId.Text);
            }
            catch (Exception)
            {
                WebFormTools.MessageBoxShow(this, "Please enter a number into the Order custID");
                return;
            }

            try
            {
                data.Order_OrderID = (String.IsNullOrWhiteSpace(orderId.Text)) ? null : (int?)int.Parse(orderId.Text);
            }
            catch (Exception)
            {
                WebFormTools.MessageBoxShow(this, "Please enter a number into Order orderID");
                return;
            }

            data.Order_OrderDate = (String.IsNullOrWhiteSpace(orderDate.Text)) ? null : orderDate.Text;
            data.Order_PoNumber = (String.IsNullOrWhiteSpace(poNumber.Text)) ? null : poNumber.Text;

            try
            {
                data.Product_ProdID = (String.IsNullOrWhiteSpace(prodId.Text)) ? null : (int?)int.Parse(prodId.Text);
            }
            catch (Exception)
            {
                WebFormTools.MessageBoxShow(this, "Please enter a number into Product prodID");
                return;
            }

            try
            {
                data.Product_Price = (String.IsNullOrWhiteSpace(price.Text)) ? null : (double?)double.Parse(price.Text);
            }
            catch (Exception)
            {
                WebFormTools.MessageBoxShow(this, "Please enter a number into Product price");
                return;
            }

            try
            {
                data.Product_ProdWeight = (String.IsNullOrWhiteSpace(prodWeight.Text)) ? null : (int?)int.Parse(prodWeight.Text);
            }
            catch (Exception)
            {
                WebFormTools.MessageBoxShow(this, "Please enter a number into Product prodWeight");
                return;
            }

            data.Product_InStock = (soldOut.Checked) ? (byte)0 : (byte)1;
            data.Product_ProdName = prodName.Text;

            try
            {
                data.Cart_OrderID = (String.IsNullOrWhiteSpace(cartOrderId.Text)) ? null : (int?)int.Parse(cartOrderId.Text);
            }
            catch (Exception)
            {
                WebFormTools.MessageBoxShow(this, "Please enter a number into Cart orderID");
                return;
            }

            try
            {
                data.Cart_ProdID = (String.IsNullOrWhiteSpace(cartProdId.Text)) ? null : (int?)int.Parse(cartProdId.Text);
            }
            catch (Exception)
            {
                WebFormTools.MessageBoxShow(this, "Please enter a number into Cart prodID");
                return;
            }

            try
            {
                data.Cart_Quantity = (String.IsNullOrWhiteSpace(quantity.Text)) ? null : (int?)int.Parse(quantity.Text);
            }
            catch (Exception)
            {
                WebFormTools.MessageBoxShow(this, "Please enter a number into Cart quantity");
                return;
            }

            var hasCustomerField = (data.Customer_CustID != null &&
                data.Customer_Firstname != null &&
                data.Customer_Lastname != null &&
                data.Customer_PhoneNumber != null);

            var hasProductField = (data.Product_ProdID != null &&
                data.Product_ProdName != null &&
                data.Product_ProdWeight != null &&
                data.Product_Price != null
                // Ignore the instock field...
            );

            if (hasCustomerField && hasProductField)
            {
                WebFormTools.MessageBoxShow(this, "Error: Cannot have customer and product sections filled");
                return;
            }

            switch (RequestType)
            {
                case PageType.Search:
                    {
                        var client = new HttpClient();
                        var task = client.GetAsync(new Uri(Url + data.ToUrl()));
                        task.RunSynchronously();
                        
                        var result = task.Result.Content;
                        break;
                    }
                case PageType.Insert:
                    {
                        var js = new JavaScriptSerializer();
                        var json = js.Serialize(data);
                        var client = new HttpClient();
                        var content = new ByteArrayContent(Encoding.ASCII.GetBytes(json));
                        var task = client.PostAsync(new Uri(Url), content);
                        task.RunSynchronously();
                        
                        var result = task.Result.Content;
                        break;
                    }
                case PageType.Update:
                    {
                        var js = new JavaScriptSerializer();
                        var json = js.Serialize(data);
                        var client = new HttpClient();
                        var content = new ByteArrayContent(Encoding.ASCII.GetBytes(json));
                        var task = client.PutAsync(new Uri(Url), content);
                        task.RunSynchronously();

                        var result = task.Result.Content;
                        break;
                    }
                case PageType.Delete:
                    {
                        var js = new JavaScriptSerializer();
                        var json = js.Serialize(data);
                        var client = new HttpClient();
                        var task = client.DeleteAsync(new Uri(Url + data.ToUrl()));
                        task.RunSynchronously();

                        var result = task.Result.Content;
                        break;
                    }
            }
        }

        protected void exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }

        protected void generatePurchaseOrder_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}