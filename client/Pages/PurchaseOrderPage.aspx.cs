using shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientSite.Pages
{
    public partial class PurchaseOrderPage : System.Web.UI.Page
    {
        public SearchResult SearchResult;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Setup print button
                Print.Attributes.Add("onClick", "window.print()");


                // Read content from previous query page
                QueryPage sourcepage = (QueryPage)Context.Handler;
                SearchResult = sourcepage.SearchResult;

                // TODO: Parse this content and fill the page
            }
        }
    }
}