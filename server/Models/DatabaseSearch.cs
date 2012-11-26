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
            if (PurchaseOrder)
            {
                return PurchaseOrderSearch();
            }
            else
            {
                return PerformNormalSearch();
            }
        }

        private SearchResult PurchaseOrderSearch()
        {
            var context = new SoaDataContext();
            var query = (from customer in context.Customers
                         where customer.deleted == (byte)0
                         from order in context.Orders
                         where customer.custID == order.custID && order.deleted == (byte)0
                         from cart in context.Carts
                         where cart.orderID == order.orderID && cart.deleted == (byte)0
                         from product in context.Products
                         where product.prodID == cart.prodID && product.deleted == (byte)0
                         select new { Customer = customer, Cart = cart, Order = order, Product = product }).Distinct();

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

            // Order fields

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

            var searchResult = new SearchResult();

            var results = query.Distinct().Select(q => CreateServerServiceRequest(q.Customer, q.Order, q.Product, q.Cart)).ToList();

            searchResult.Columns.AddRange(new string[]
            {
                "Product_ProdID",
                "Product_ProdName",
                "Cart_Quantity",
                "Product_Price",
                "Product_ProdWeight"
            });

            searchResult.Rows = results;

            try
            {
                var cust = query.Select(c => c.Customer).FirstOrDefault();

                searchResult.CustID = cust.custID;
                searchResult.FirstName = cust.firstName;
                searchResult.LastName = cust.lastName;
                searchResult.PhoneNumber = cust.phoneNumber;

                var order = query.Select(c => c.Order).FirstOrDefault();
                searchResult.PoNumber = order.poNumber;
                searchResult.PurchaseDate = order.orderDate;

                var subTotal = query.Where(q => q.Product.inStock == (byte)1).Select(q => q.Product.price * q.Cart.quantity).Sum();
                var tax = subTotal * 0.13;
                var total = subTotal + tax;
                searchResult.SubTotal = subTotal;
                searchResult.Tax = tax;
                searchResult.Total = total;

                var totalPieces = query.Where(q => q.Product.inStock == (byte)1).Select(q => q.Cart.quantity).Sum();
                var totalWeight = query.Where(q => q.Product.inStock == (byte)1).Select(q => q.Product.prodWeight * q.Cart.quantity).Sum();
                searchResult.TotalNumberOfPieces = totalPieces;
                searchResult.TotalWeight = totalWeight;
            }
            catch (Exception)
            {
                throw new Exception("No data found for purchase order");
            }

            return searchResult;
        }

        private SearchResult PerformNormalSearch()
        {
            var context = new SoaDataContext();
            var query = (from customer in context.Customers
                         where customer.deleted == (byte)0
                         from order in context.Orders
                         where customer.custID == order.custID && order.deleted == (byte)0
                         from cart in context.Carts
                         where cart.orderID == order.orderID && cart.deleted == (byte)0
                         from product in context.Products
                         where product.prodID == cart.prodID && product.deleted == (byte)0
                         select new { Customer = customer, Cart = cart, Order = order, Product = product }).Distinct();


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
            var hasCustomerField = (Data.Customer_CustID != null &&
                Data.Customer_FirstName != null &&
                Data.Customer_LastName != null &&
                Data.Customer_PhoneNumber != null);

            var hasProductField = (Data.Product_ProdID != null &&
                Data.Product_ProdName != null &&
                Data.Product_ProdWeight != null &&
                Data.Product_Price != null &&
                Data.Product_InStock != null);

            var hasOrderField = (Data.Order_CustID != null &&
                Data.Order_OrderDate != null &&
                Data.Order_OrderID != null &&
                Data.Order_PoNumber != null);

            var hasCartField = (Data.Cart_OrderID != null &&
                Data.Cart_ProdID != null &&
                Data.Cart_Quantity != null);

            var showOnlyCustomer = hasCustomerField && !hasProductField && !hasOrderField && !hasCartField;
            var showOnlyProduct = !hasCustomerField && hasProductField && !hasOrderField && !hasCartField;
            var showOrderAndCustomer = !hasProductField && hasOrderField && !hasCartField;
            var showProductAndOrderAndCart = (hasProductField && hasOrderField) || hasCartField;

            var searchResult = new SearchResult();

            var customerFields = new string[]
            {
                "Customer_CustID",
                "Customer_FirstName",
                "Customer_LastName",
                "Customer_PhoneNumber"
            };

            var productFields = new string[]
            {
                "Product_ProdID",
                "Product_ProdName",
                "Product_ProdWeight",
                "Product_Price",
                "Product_InStock"
            };

            var orderFields = new string[]
            {
                "Order_OrderID",
                "Order_CustID",
                "Order_OrderDate",
                "Order_PoNumber"
            };

            var cartFields = new string[]
            {
                "Cart_ProdID",
                "Cart_OrderID",
                "Cart_Quantity"
            };

            if (showOnlyCustomer)
            {
                searchResult.Columns.AddRange(customerFields);
            }
            else if (showOnlyProduct)
            {
                searchResult.Columns.AddRange(productFields);
            }
            else if (showOrderAndCustomer)
            {
                searchResult.Columns.AddRange(customerFields);
                searchResult.Columns.AddRange(orderFields);
                searchResult.Columns.AddRange(cartFields);
            }
            else if (showProductAndOrderAndCart)
            {
                searchResult.Columns.AddRange(customerFields);
                searchResult.Columns.AddRange(productFields);
                searchResult.Columns.AddRange(orderFields);
                searchResult.Columns.AddRange(cartFields);
            }
            else
            {
                searchResult.Columns.AddRange(customerFields);
                searchResult.Columns.AddRange(orderFields);
                searchResult.Columns.AddRange(productFields);
                searchResult.Columns.AddRange(cartFields);
            }

            var results = query.Select(q => CreateServerServiceRequest(q.Customer, q.Order, q.Product, q.Cart)).ToList();

            searchResult.Rows = results;

            return searchResult;
        }

        private static ServerServiceRequest CreateServerServiceRequest(Customer customer, Order order, Product product, Cart cart)
        {
            var data = new ServerServiceRequest();

            data.Customer_CustID = customer.custID;
            data.Customer_FirstName = customer.firstName;
            data.Customer_LastName = customer.lastName;
            data.Customer_PhoneNumber = customer.phoneNumber;

            data.Order_CustID = order.custID;
            data.Order_OrderID = order.orderID;
            data.Order_OrderDate = order.orderDate;
            data.Order_PoNumber = order.poNumber;

            data.Product_ProdID = product.prodID;
            data.Product_ProdName = product.prodName;
            data.Product_ProdWeight = product.prodWeight;
            data.Product_Price = product.price;
            data.Product_InStock = product.inStock;

            data.Cart_OrderID = cart.orderID;
            data.Cart_ProdID = cart.prodID;
            data.Cart_Quantity = cart.quantity;

            return data;
        }
    }
}