using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ShopMate.Models
{
    public class WarehouseStockTran
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Product")]
        public int? ProductId { get; set; }
        public virtual Product Product_ProductId { get; set; }
        [Required]
        [DisplayName("Warehouse")]
        public int? WarehouseId { get; set; }
        public virtual Warehouse Warehouse_WarehouseId { get; set; }

        [Required]
        [DisplayName("Added By")]
        public int? UserId { get; set; }
        public virtual User User_UserId { get; set; }

        public string TransactionType { get; set; }

        public DateTime DateAdded { get; set; }

        [Required]
        [DisplayName("Inventory Type")]
        public int? InventoryTypeId { get; set; }
        public virtual InventoryType InventoryType_InventoryTypeId { get; set; }

    }
}