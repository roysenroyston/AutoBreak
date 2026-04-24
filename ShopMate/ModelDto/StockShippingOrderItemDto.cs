using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopMate.ModelDto
{
    public class StockShippingOrderItemDto
    {
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        //public List<StockShippingOrderItemsDto> StockShippingOrderItems { get; set; }
        public Decimal Quantity { get; set; }

        public string CompanyAddress { get; set; }
        public string Warehouse { get; set; }
        public string receivedby { get; set; }
        public List<StockShippingOrderItemDto> Items { get; set; }

    }

    //public class StockShippingOrderItemDto
    //{
    //    public int? StockShippingOrderId { get; set; }
    //    public int? ProductId { get; set; }
    //    public Decimal Quantity { get; set; }
    //}
}