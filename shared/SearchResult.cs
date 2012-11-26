using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class SearchResult
    {
        public int CustID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string PurchaseDate { get; set; }
        public string PoNumber { get; set; }

        public double SubTotal { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }

        public int TotalNumberOfPieces { get; set; }
        public double TotalWeight { get; set; }

        public string CustomerInformation { get; set; }
        public List<string> columns;
        public List<List<string>> rows;
    }
}
