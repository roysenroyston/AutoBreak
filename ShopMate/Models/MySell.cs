using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopMate.Models
{
    public class MySell
    {
        public string currency { get; set; }
        public string date { get; set; }
        public int id { get; set; }
        public int invoiceId { get; set; }
        public int online { get; set; }
        public List<SellProduct> products { get; set; }
        public string subtotal { get; set; }
        public double tax { get; set; }
        public string time { get; set; }
        public int userId { get; set; }
        public string paymentMethod { get; set; }
        public string customer { get; set; }
    }
    public class SellProduct
    {
        public string barcode { get; set; }
        public int id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }

        public decimal priceRtgs { get; set; }
        public int prodId { get; set; }
        public decimal quantity { get; set; }
        public double tax { get; set; }

       // public decimal UnitPrice { get; set; }
    }
}