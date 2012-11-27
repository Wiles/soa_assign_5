using System;
using System.Web.UI;

namespace ClientSite
{
    public partial class _Default : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the search control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void search_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/QueryPage.aspx?type=search");
        }

        /// <summary>
        /// Handles the Click event of the insert control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void insert_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/QueryPage.aspx?type=insert");
        }

        /// <summary>
        /// Handles the Click event of the update control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void update_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/QueryPage.aspx?type=update");
        }

        /// <summary>
        /// Handles the Click event of the delete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void delete_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/DeletePage.aspx");
        }

        /// <summary>
        /// Handles the Click event of the exit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://www.google.ca");
        }
    }
}