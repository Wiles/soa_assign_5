using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace shared.FormData
{
    
    public class ServerServiceRequest
    {
        public bool GenPurchaseOrder { get; set; }

        public int? Customer_CustID{ get; set; }
        public string Customer_FirstName { get; set; }
        public string Customer_LastName { get; set; }
        public string Customer_PhoneNumber { get; set; }

        public int? Product_ProdID{ get; set; }
        public string Product_ProdName { get; set; }
        public double? Product_Price { get; set; }
        public int? Product_ProdWeight { get; set; }
        public byte? Product_InStock { get; set; }

        public int? Order_OrderID{ get; set; }
        public int? Order_CustID { get; set; }
        public string Order_PoNumber { get; set; }
        public string Order_OrderDate { get; set; }

        public int? Cart_OrderID{ get; set; }
        public int? Cart_ProdID { get; set; }
        public int? Cart_Quantity { get; set; }

        public ServerServiceRequest()
        {
            GenPurchaseOrder = false;
        }

        public static ServerServiceRequest FromTableQueries(List<TableColumnValue> tcvs)
        {
            var data = new ServerServiceRequest();
            foreach (var tcv in tcvs)
            {

                var table = tcv.Table;
                var column = tcv.Column[0].ToString().ToUpper() + tcv.Column.Substring(1);
                var name = String.Format("{0}_{1}", table, column);

                if (!IgnoreProperty(name))
                {
                    var value = tcv.Value;

                    var type = data.GetType();
                    type.GetProperty(name).SetValue(data, value, null);
                }
            }

            return data;
        }

        public string ToUrl()
        {
            var sb = new StringBuilder();
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (!IgnoreProperty(prop.Name))
                {
                    var value = prop.GetValue(this, null);
                    if (value != null)
                    {
                        var name = prop.Name;
                        var table = name.Split('_')[0];
                        var column = name.Split('_')[1];
                        var columnName = column[0].ToString().ToLower() + column.Substring(1);
                        sb.Append(table);
                        sb.Append("/");
                        sb.Append(columnName);
                        sb.Append("/");
                        sb.Append(value.ToString());
                        sb.Append("/");
                    } 
                }
            }

            // Remove the last '/'
            return sb.ToString().TrimEnd('/');
        }

        private static bool IgnoreProperty(string name)
        {
            return name.Equals("GenPurchaseOrder", StringComparison.CurrentCultureIgnoreCase);
        }
    }

    public class TableColumnValue
    {
        public string Table;
        public string Column;
        public string Value;

        public TableColumnValue(string table, string column, string value)
        {
            this.Table = table;
            this.Column = column;
            this.Value = value;
        }
    }
}