using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopMate.Models
{
    public class finishedItem
    {
        [DisplayName(" ID")]
        public int Id { get; set; }
       
        [DisplayName("Description")]
        public string description { get; set; }
        [DisplayName("Cut Sheet")]
        public string cutsheet { get; set; }
        [Required]
        [DisplayName("Quantity")]
        public Decimal Quantity { get; set; }
      
        [DisplayName("Finished Quantity")]
        public int? Qty { get; set; }

        [DisplayName("Unit Price")]
        public Decimal unitprice { get; set; }
        [DisplayName("Total Price")]
        public Decimal Total { get; set; }
        public FinishedGoods finishedgoods { get; set; }

        [DisplayName("Inventory Type")]
        public int? InventoryTypeId { get; set; }
        public virtual InventoryType InventoryType_InventoryTypeId { get; set; }
        [DisplayName("Raw Material Id")]
        public int ProductId { get; set; }
        public virtual Product Product_ProductId { get; set; }
        public int ProductStockId { get; set; }
        public int TransactionId { get; set; }
        public DateTime dateadded { get; set; }
        public int? WarehouseId { get; set; }
        public virtual Warehouse Warehouse_WarehouseId { get; set; }
    }
}