using server.Sql;
using shared.FormData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Models
{
    public class DatabaseUpdate
    {
        private ServerServiceRequest Data;
        public DatabaseUpdate(ServerServiceRequest data)
        {
            this.Data = data;
        }

        public void Insert()
        {
            var context = new SoaDataContext();
            Customer customer = null;
            if (Data.Customer_LastName != null)
            {
                customer = new Customer();
                customer.firstName = Data.Customer_FirstName;
                customer.lastName = Data.Customer_LastName;
                customer.phoneNumber = Data.Customer_PhoneNumber;

                context.Customers.InsertOnSubmit(customer);
            }

            context.SubmitChanges();
            
            Order order = null;
            if (Data.Order_OrderDate != null)
            {
                order = new Order();

                if (Data.Order_CustID == null)
                {
                    throw new Exception("Please enter a CustID into the order table");
                }

                order.custID = (int)Data.Order_CustID;
                order.orderDate = Data.Order_OrderDate;
                order.poNumber = Data.Order_PoNumber;

                context.Orders.InsertOnSubmit(order);
            }

            context.SubmitChanges();


            Product product = null;
            if (Data.Product_Price != null)
            {
                product = new Product();
                product.prodName = Data.Product_ProdName;
                product.price = (Data.Product_Price != null) ? (double)Data.Product_Price : 0;
                product.prodWeight = (Data.Product_ProdWeight != null) ? (double)Data.Product_ProdWeight : 0;
                product.inStock = (Data.Product_InStock != null) ? (byte)Data.Product_InStock : (byte)0;

                context.Products.InsertOnSubmit(product);
            }

            context.SubmitChanges();
            
            if (Data.Cart_Quantity != null)
            {
                var cart = new Cart();
                if (order != null)
                {
                    cart.orderID = order.orderID;
                }

                if (product != null)
                {
                    cart.prodID = product.prodID;
                }

                cart.quantity = (Data.Cart_Quantity != null) ? (int)Data.Cart_Quantity : 0;
            }

            context.SubmitChanges();
        }

        public void Update()
        {
            var context = new SoaDataContext();
            if (Data.Customer_CustID != null)
            {
                var customer = (from c in context.Customers
                    where c.custID == Data.Customer_CustID && c.deleted == (byte)0
                        select c).FirstOrDefault();


                if (customer == null)
                {
                    throw new Exception(String.Format("Customer with ID {0} does not exist", Data.Customer_CustID));
                }
                else
                {
                    // Update with new fields
                    customer.firstName = Data.Customer_FirstName;
                    customer.lastName = Data.Customer_LastName;
                    customer.firstName = Data.Customer_FirstName;
                    customer.phoneNumber = Data.Customer_PhoneNumber;
                }
            }

            if (Data.Order_OrderID != null)
            {
                var order = (from c in context.Orders
                             where c.orderID == Data.Order_OrderID && c.deleted == (byte)0
                                select c).FirstOrDefault();


                if (order == null)
                {
                    throw new Exception(String.Format("Order with ID {0} does not exist", Data.Order_OrderID));
                }
                else
                {
                    // Update with new fields
                    order.poNumber = Data.Order_PoNumber;
                    order.orderDate = Data.Order_OrderDate;
                    order.custID = (int)Data.Order_CustID;
                }
            }

            if (Data.Product_ProdID != null)
            {
                var product = (from c in context.Products
                               where c.prodID == Data.Product_ProdID && c.deleted == (byte)0
                                select c).FirstOrDefault();


                if (product == null)
                {
                    throw new Exception(String.Format("Product with ID {0} does not exist", Data.Product_ProdID));
                }
                else
                {
                    // Update with new fields
                    product.prodName = Data.Product_ProdName;
                    product.prodWeight = (Data.Product_ProdWeight != null) ? (double)Data.Product_ProdWeight : 0;
                    product.price = (Data.Product_Price != null) ? (double)Data.Product_Price : 0;
                    product.inStock = (Data.Product_InStock != null) ? (byte)Data.Product_InStock : (byte)0;
                }
            }

            if (Data.Cart_OrderID != null && Data.Cart_ProdID != null)
            {
                var cart = (from c in context.Carts
                            where c.orderID == Data.Cart_OrderID && c.prodID == Data.Cart_ProdID && c.deleted == (byte)0
                                select c).FirstOrDefault();


                if (cart == null)
                {
                    throw new Exception(String.Format("Cart with ProdID {0} and OrderID {1} does not exist", 
                        Data.Cart_ProdID, Data.Cart_OrderID));
                }
                else
                {
                    // Update with new fields
                    cart.quantity = (Data.Cart_Quantity != null) ? (int)Data.Cart_Quantity : 0;
                }
            }

            context.SubmitChanges();
        }

        public void Delete()
        {
            var context = new SoaDataContext();
            if (Data.Customer_CustID != null)
            {
                var customer = (from c in context.Customers
                                where c.custID == Data.Customer_CustID && c.deleted == (byte)0
                                select c).FirstOrDefault();


                if (customer == null)
                {
                    throw new Exception(String.Format("Customer with ID {0} does not exist", Data.Customer_CustID));
                }
                else
                {
                    // Soft delete
                    customer.deleted = (byte)1;
                }

                // Delete all the data for the customer
                var orders = (from o in context.Orders
                              where o.custID == customer.custID && o.deleted == (byte)0
                             select o).ToArray();

                foreach (var order in orders)
                {
                    // Delete all the carts for the orders
                    foreach (var cart in order.Carts)
                    {
                        cart.deleted = (byte)1;
                    }

                    order.deleted = (byte)1;
                }
            }

            if (Data.Order_OrderID != null)
            {
                var order = (from c in context.Orders
                             where c.orderID == Data.Order_OrderID && c.deleted == (byte)0
                             select c).FirstOrDefault();


                if (order == null)
                {
                    throw new Exception(String.Format("Order with ID {0} does not exist", Data.Order_OrderID));
                }
                else
                {
                    // Soft delete
                    order.deleted = (byte)1;
                }

                // Delete all the carts for the orders
                foreach (var cart in order.Carts)
                {
                    cart.deleted = (byte)1;
                }
            }

            if (Data.Product_ProdID != null)
            {
                var product = (from c in context.Products
                               where c.prodID == Data.Product_ProdID && c.deleted == (byte)0
                               select c).FirstOrDefault();


                if (product == null)
                {
                    throw new Exception(String.Format("Product with ID {0} does not exist", Data.Product_ProdID));
                }
                else
                {
                    // Soft delete
                    product.deleted = (byte)1;
                }

                // Delete from carts from product
                foreach (var cart in product.Carts)
                {
                    cart.deleted = (byte)1;
                }
            }

            if (Data.Cart_OrderID != null && Data.Cart_ProdID != null)
            {
                var cart = (from c in context.Carts
                            where c.orderID == Data.Cart_OrderID && c.prodID == Data.Cart_ProdID && c.deleted == (byte)0
                            select c).FirstOrDefault();


                if (cart == null)
                {
                    throw new Exception(String.Format("Cart with ProdID {0} and OrderID {1} does not exist",
                        Data.Cart_ProdID, Data.Cart_OrderID));
                }
                else
                {
                    // Soft delete
                    cart.deleted = (byte)1;
                }
            }

            context.SubmitChanges();
        }
    }
}