using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientSite.Pages
{
    public partial class SearchResultsPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Setup print button
                Print.Attributes.Add("onClick", "window.print()");
         
                
                // Read content from previous query page
                QueryPage sourcepage = (QueryPage)Context.Handler;
                var content = sourcepage.SearchContent;

                // TODO: Parse this content and fill the page
            }
        }

        protected void GoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/QueryPage.aspx?type=search");
        }

        protected void Print_Click(object sender, EventArgs e)
        {
        }

        protected void Exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://www.google.ca");
        }
    }
}