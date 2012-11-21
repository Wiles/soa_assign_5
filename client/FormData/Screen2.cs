using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSite.FormData
{
    public class Screen2Data
    {
        public int CustId;
        public string Firstname;
        public string Lastname;
        public string phoneNumber;

        public int ProdId;
        public string ProdName;
        public string Price;
        public int ProdWeight;

        public int OrderId;
        public int OrderCustId;
        public int PoNumber;
        public string OrderDate;

        public int CartOrderId;
        public int CartProdId;
        public int Quantity;
    }
}