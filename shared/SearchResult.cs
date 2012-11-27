using System.Collections.Generic;
using shared.FormData;

namespace shared
{
    public class SearchResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult" /> class.
        /// </summary>
        public SearchResult()
        {
            Columns = new List<string>();
            Rows = new List<ServerServiceRequest>();
        }

        /// <summary>
        /// The columns
        /// </summary>
        public List<string> Columns;

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public List<ServerServiceRequest> Rows { get; set; }

        /// <summary>
        /// Gets or sets the cust ID.
        /// </summary>
        /// <value>
        /// The cust ID.
        /// </value>
        public int CustID { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the purchase date.
        /// </summary>
        /// <value>
        /// The purchase date.
        /// </value>
        public string PurchaseDate { get; set; }

        /// <summary>
        /// Gets or sets the po number.
        /// </summary>
        /// <value>
        /// The po number.
        /// </value>
        public string PoNumber { get; set; }

        /// <summary>
        /// Gets or sets the sub total.
        /// </summary>
        /// <value>
        /// The sub total.
        /// </value>
        public double SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the tax.
        /// </summary>
        /// <value>
        /// The tax.
        /// </value>
        public double Tax { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public double Total { get; set; }

        /// <summary>
        /// Gets or sets the total number of pieces.
        /// </summary>
        /// <value>
        /// The total number of pieces.
        /// </value>
        public int TotalNumberOfPieces { get; set; }

        /// <summary>
        /// Gets or sets the total weight.
        /// </summary>
        /// <value>
        /// The total weight.
        /// </value>
        public double TotalWeight { get; set; }
    }
}
