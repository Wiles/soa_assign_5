using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientSite
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void search_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pages/Screen2.aspx?type=search");
        }

        protected void insert_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pages/Screen2.aspx?type=insert");
        }

        protected void update_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pages/Screen2.aspx?type=update");
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pages/Screen2.aspx?type=delete");
        }

        protected void exit_Click(object sender, EventArgs e)
        {

        }
    }
}