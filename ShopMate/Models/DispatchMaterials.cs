using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopMate.Models
{
    public class DispatchMaterials
    {
        [DisplayName(" ID")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Product")]
        public int? ProductId { get; set; }
        public virtual Product Product_ProductId { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Quantity")]
        public Decimal Quantity { get; set; }

        public int DispatchId { get; set; }
        public virtual Dispatch Dispatch_DispatchId { get; set; }
        public Nullable<int> AddedBy { get; set; }
        [DisplayName("Warehouse")]
        public int WarehouseId { get; set; }
        [DisplayName("Date Added")]
        public Nullable<DateTime> DateAdded { get; set; }
    }
}