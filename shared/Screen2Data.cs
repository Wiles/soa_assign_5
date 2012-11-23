using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace shared.FormData
{
    public class Screen2Data
    {
        public int? Customer_CustId{ get; set; }
        public string Customer_Firstname { get; set; }
        public string Customer_Lastname { get; set; }
        public string Customer_PhoneNumber { get; set; }

        public int? Product_ProdId{ get; set; }
        public string Product_ProdName { get; set; }
        public string Product_Price { get; set; }
        public int? Product_ProdWeight { get; set; }

        public int? Order_OrderId{ get; set; }
        public int? Order_OrderCustId { get; set; }
        public int? Order_PoNumber { get; set; }
        public string Order_OrderDate { get; set; }

        public int? Cart_CartOrderId{ get; set; }
        public int? Cart_CartProdId { get; set; }
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