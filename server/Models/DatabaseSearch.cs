using server.Sql;
using shared;
using shared.FormData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Models
{
    public class DatabaseSearch
    {
        private readonly ServerServiceRequest Data;
        private readonly bool PurchaseOrder;

        public DatabaseSearch(ServerServiceRequest serviceRequest, bool purchaseOrder = false)
        {
            this.Data = serviceRequest;
            this.PurchaseOrder = purchaseOrder;
        }

        public SearchResult Search()
        {
            var context = new SoaDataContext();
            var query = from customer in context.Customers
                        where customer.deleted == (byte)0
                        from order in context.Orders
                        where customer.custID == order.custID && order.deleted == (byte)0
                        from cart in context.Carts
                        where cart.orderID == order.orderID && cart.deleted == (byte)0
                        from product in context.Products
                        where product.prodID == cart.prodID && product.deleted == (byte)0
                        select new { Customer = customer, Cart = cart, Order = order, Product = product };


            // TODO: Replace with reflection


            // Customer fields

            if (Data.Customer_CustID != null)
            {
                query = query.Where(q => q.Customer.custID == Data.Customer_CustID);
            }

            if (Data.Customer_FirstName != null)
            {
                query = query.Where(q => q.Customer.firstName == Data.Customer_FirstName);
            }

            if (Data.Customer_LastName != null)
            {
                query = query.Where(q => q.Customer.lastName == Data.Customer_LastName);
            }

            if (Data.Customer_PhoneNumber != null)
            {
                query = query.Where(q => q.Customer.phoneNumber == Data.Customer_PhoneNumber);
            }


            // Order fields

            if (Data.Order_CustID != null)
            {
                query = query.Where(q => q.Order.custID == Data.Order_CustID);
            }

            if (Data.Order_OrderID != null)
            {
                query = query.Where(q => q.Order.orderID == Data.Order_OrderID);
            }

            if (Data.Order_OrderDate != null)
            {
                query = query.Where(q => q.Order.orderDate == Data.Order_OrderDate);
            }

            if (Data.Order_PoNumber != null)
            {
                query = query.Where(q => q.Order.poNumber == Data.Order_PoNumber);
            }


            // Product fields

            if (Data.Product_ProdID != null)
            {
                query = query.Where(q => q.Product.prodID == Data.Product_ProdID);
            }

            if (Data.Product_ProdName != null)
            {
                query = query.Where(q => q.Product.prodName == Data.Product_ProdName);
            }

            if (Data.Product_ProdWeight != null)
            {
                query = query.Where(q => q.Product.prodWeight == Data.Product_ProdWeight);
            }

            if (Data.Product_Price != null)
            {
                query = query.Where(q => q.Product.price == Data.Product_Price);
            }

            if (Data.Product_InStock != null)
            {
                query = query.Where(q => q.Product.inStock == Data.Product_InStock);
            }


            // Cart fields

            if (Data.Cart_OrderID != null)
            {
                query = query.Where(q => q.Cart.orderID == Data.Cart_OrderID);
            }

            if (Data.Cart_ProdID != null)
            {
                query = query.Where(q => q.Cart.prodID == Data.Cart_ProdID);
            }

            if (Data.Cart_Quantity != null)
            {
                query = query.Where(q => q.Cart.quantity == Data.Cart_Quantity);
            }


            // Get the results

            var results = query.ToArray();

            int resultIndex = 0;
            foreach (var item in results)
            {
                Logger.GetInstance().Write("Item {0}: {1}", resultIndex, item);
                resultIndex++;
            }

            var searchResult = new SearchResult();

            return searchResult;
        }
    }
}