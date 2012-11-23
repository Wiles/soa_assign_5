using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace shared.FormData
{
    public class Screen2Data
    {
        public int? Customer_CustID{ get; set; }
        public string Customer_Firstname { get; set; }
        public string Customer_Lastname { get; set; }
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

        public string ToUrl()
        {
            var sb = new StringBuilder();
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(this);
                if (value != null)
                {
                    var fullname = prop.Name[0].ToString().ToLower() + prop.Name.Substring(1);
                    var table = fullname.Split('_')[0];
                    var column = fullname.Split('_')[1];
                    sb.Append(table);
                    sb.Append("/");
                    sb.Append(column);
                    sb.Append("/");
                    sb.Append(value.ToString());
                }
            }

            return sb.ToString();
        }
    }
}