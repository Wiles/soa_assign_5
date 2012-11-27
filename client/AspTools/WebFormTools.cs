using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientSite.AspTools
{
    public class WebFormTools
    {
        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="message">The message.</param>
        public static void MessageBoxShow(Page page, string message)
        {
            var ltr = new Literal();
            ltr.Text = @"<script type='text/javascript'> alert('" + message + "') </script>";
            page.Controls.Add(ltr);
        }
    }
}