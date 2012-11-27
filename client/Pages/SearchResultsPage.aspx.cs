using System;
using System.Linq;
using System.Web.UI.WebControls;
using ClientSite.Logic;
using shared;

namespace ClientSite.Pages
{
    public partial class SearchResultsPage : System.Web.UI.Page
    {
        /// <summary>
        /// The search result
        /// </summary>
        public SearchResult SearchResult;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Setup print button
                    Print.Attributes.Add("onClick", "window.print()");


                    // Read content from previous query page
                    QueryPage sourcepage = (QueryPage)Context.Handler;
                    SearchResult = sourcepage.SearchResult;

                    if (SearchResult.Rows.Count == 0)
                    {
                        Information.Text = "No results found for query";
                    }
                    else
                    {
                        PopulateSearchResults();
                    }
                }
                catch (Exception ex)
                {
                    Information.Text = "Error: Corrupted data from web service found";
                    Logger.GetInstance().Write(ex);
                }
            }
        }

        /// <summary>
        /// Populates the search results.
        /// </summary>
        private void PopulateSearchResults()
        {
            var columns = SearchResult.Columns;

            ResultsTable.Rows.Clear();
            var headerRow = new TableRow();
            foreach (var column in columns)
            {
                var cell = new TableCell();
                if (column.Contains('_'))
                {
                    cell.Text = column.Split('_')[1];
                }
                else
                {
                    cell.Text = column;
                }
                headerRow.Cells.Add(cell);
            }
            ResultsTable.Rows.Add(headerRow);

            foreach (var searchResultRow in SearchResult.Rows)
            {
                var row = new TableRow();

                foreach (var column in columns)
                {
                    string strValue = "";
                    var value = searchResultRow.GetType().GetProperty(column)
                        .GetValue(searchResultRow, null);
                    if (value != null)
                    {
                        strValue = value.ToString();
                    }

                    var cell = new TableCell();
                    cell.Text = strValue;
                    row.Cells.Add(cell);
                }

                ResultsTable.Rows.Add(row);
            }
        }

        /// <summary>
        /// Handles the Click event of the GoBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void GoBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/Pages/QueryPage.aspx?type=search");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the Print control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Print_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Click event of the Exit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Exit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Constants.HomePage);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex);
            }
        }
    }
}