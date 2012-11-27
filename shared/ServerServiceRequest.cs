using System;
using System.Collections.Generic;
using System.Text;

namespace shared.FormData
{
    public class ServerServiceRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerServiceRequest" /> class.
        /// </summary>
        public ServerServiceRequest()
        {
        }

        /// <summary>
        /// Gets or sets the customer_ cust ID.
        /// </summary>
        /// <value>
        /// The customer_ cust ID.
        /// </value>
        public int? Customer_CustID{ get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer_.
        /// </summary>
        /// <value>
        /// The first name of the customer_.
        /// </value>
        public string Customer_FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer_.
        /// </summary>
        /// <value>
        /// The last name of the customer_.
        /// </value>
        public string Customer_LastName { get; set; }

        /// <summary>
        /// Gets or sets the customer_ phone number.
        /// </summary>
        /// <value>
        /// The customer_ phone number.
        /// </value>
        public string Customer_PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the product_ prod ID.
        /// </summary>
        /// <value>
        /// The product_ prod ID.
        /// </value>
        public int? Product_ProdID{ get; set; }

        /// <summary>
        /// Gets or sets the name of the product_ prod.
        /// </summary>
        /// <value>
        /// The name of the product_ prod.
        /// </value>
        public string Product_ProdName { get; set; }

        /// <summary>
        /// Gets or sets the product_ price.
        /// </summary>
        /// <value>
        /// The product_ price.
        /// </value>
        public double? Product_Price { get; set; }

        /// <summary>
        /// Gets or sets the product_ prod weight.
        /// </summary>
        /// <value>
        /// The product_ prod weight.
        /// </value>
        public double? Product_ProdWeight { get; set; }

        /// <summary>
        /// Gets or sets the product_ in stock.
        /// </summary>
        /// <value>
        /// The product_ in stock.
        /// </value>
        public byte? Product_InStock { get; set; }

        /// <summary>
        /// Gets or sets the order_ order ID.
        /// </summary>
        /// <value>
        /// The order_ order ID.
        /// </value>
        public int? Order_OrderID{ get; set; }

        /// <summary>
        /// Gets or sets the order_ cust ID.
        /// </summary>
        /// <value>
        /// The order_ cust ID.
        /// </value>
        public int? Order_CustID { get; set; }

        /// <summary>
        /// Gets or sets the order_ po number.
        /// </summary>
        /// <value>
        /// The order_ po number.
        /// </value>
        public string Order_PoNumber { get; set; }

        /// <summary>
        /// Gets or sets the order_ order date.
        /// </summary>
        /// <value>
        /// The order_ order date.
        /// </value>
        public string Order_OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the cart_ order ID.
        /// </summary>
        /// <value>
        /// The cart_ order ID.
        /// </value>
        public int? Cart_OrderID{ get; set; }

        /// <summary>
        /// Gets or sets the cart_ prod ID.
        /// </summary>
        /// <value>
        /// The cart_ prod ID.
        /// </value>
        public int? Cart_ProdID { get; set; }

        /// <summary>
        /// Gets or sets the cart_ quantity.
        /// </summary>
        /// <value>
        /// The cart_ quantity.
        /// </value>
        public int? Cart_Quantity { get; set; }

        /// <summary>
        /// Froms the table queries.
        /// </summary>
        /// <param name="tcvs">The TCVS.</param>
        /// <returns>The service request</returns>
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
                    var type = data.GetType();
                    var property = type.GetProperty(name);
                    var propertyType = property.PropertyType;

                    if (propertyType == typeof(string))
                    {
                        var value = (tcv.Value != null) ? tcv.Value.ToString() : "";
                        property.SetValue(data, value, null);
                    }
                    else if (propertyType == typeof(int))
                    {
                        property.SetValue(data, int.Parse(tcv.Value), null);
                    }
                    else if (propertyType == typeof(byte))
                    {
                        property.SetValue(data, byte.Parse(tcv.Value), null);
                    }
                    else if (propertyType == typeof(double))
                    {
                        property.SetValue(data, double.Parse(tcv.Value), null);
                    }
                    else if (propertyType == typeof(int?))
                    {
                        int? value = (tcv.Value != null) ? int.Parse(tcv.Value.ToString()) : 0;
                        property.SetValue(data, value, null);
                    }
                    else if (propertyType == typeof(byte?))
                    {
                        byte? value = (tcv.Value != null) ? byte.Parse(tcv.Value.ToString()) : (byte)0;
                        property.SetValue(data, value, null);
                    }
                    else if (propertyType == typeof(double?))
                    {
                        double? value = (tcv.Value != null) ? double.Parse(tcv.Value.ToString()) : 0;
                        property.SetValue(data, value, null);
                    }
                    else if (propertyType == typeof(bool))
                    {
                        property.SetValue(data, bool.Parse(tcv.Value), null);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// To the URL.
        /// </summary>
        /// <param name="withGeneratePurchaseOrder">if set to <c>true</c> [with generate purchase order].</param>
        /// <returns>The url</returns>
        public string ToUrl(bool withGeneratePurchaseOrder = false)
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
            var suffix = (withGeneratePurchaseOrder) ? (withGeneratePurchaseOrder.ToString() + '/') : "";
            return suffix + sb.ToString().TrimEnd('/');
        }

        /// <summary>
        /// Self DocumentingMethodNameThatTellsYouEverythingAboutItBecauseItsUltraAmazingWeShouldAllWriteLikeThisBecauseItsGood
        /// </summary>
        /// <returns>Is this at least one non-null property in this object</returns>
        public bool HasOneOrMoreFieldsWithAValue()
        {
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (!IgnoreProperty(prop.Name))
                {
                    if (prop.GetValue(this, null) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Ignores the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static bool IgnoreProperty(string name)
        {
            return name.Equals("GenPurchaseOrder", StringComparison.CurrentCultureIgnoreCase);
        }
    }

    /// <summary>
    /// Represents a table column value.
    /// </summary>
    public class TableColumnValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumnValue" /> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        public TableColumnValue(string table, string column, string value)
        {
            this.Table = table;
            this.Column = column;
            this.Value = value;
        }

        /// <summary>
        /// The table
        /// </summary>
        public string Table;

        /// <summary>
        /// The column
        /// </summary>
        public string Column;

        /// <summary>
        /// The value
        /// </summary>
        public string Value;
    }
}