using ClientSite.Logic;
using shared;
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
        public SearchResult SearchResult;

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

        protected void Print_Click(object sender, EventArgs e)
        {
        }

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