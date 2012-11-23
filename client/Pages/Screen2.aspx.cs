using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientSite.Pages
{
    public partial class InsertPage : System.Web.UI.Page
    {
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
            switch (RequestType)
            {
                case PageType.Search:
                    {

                        break;
                    }
                case PageType.Insert:
                    {

                        break;
                    }
                case PageType.Update:
                    {

                        break;
                    }
                case PageType.Delete:
                    {

                        break;
                    }
            }
        }

        protected void exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void generatePurchaseOrder_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}