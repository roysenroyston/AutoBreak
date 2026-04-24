using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopMate.Models
{
    [TrackChanges]
    public class Stores
    {
        [DisplayName(" ID")]
        public int Id { get; set; }
     
        [StringLength(100)]
        [DisplayName("Name")]
        public string Name { get; set; }
       
        [DisplayName("Total Amount")]
        public Decimal totalprice { get; set; }

        [DisplayName("Purchase Price")]
        public Decimal PurchasePrice { get; set; }
        [DisplayName("Remaining Quantity")]
        [DefaultValue(0.00)]
        public Decimal RemainingQuantity { get; set; }

        [Required]
        [DisplayName("Vendor Name")]
        public int? VendorUserId { get; set; }

        public virtual User User_VendorUserId { get; set; }
        [DisplayName("Product Category")]
        public string ProductCategory { get; set; }
        [SkipTracking]
        [StringLength(100)]
        [DisplayName("Bar Code")]
        public string BarCode { get; set; }

        [Required]
        [DisplayName("Warehouse")]
        public int WarehouseId { get; set; }
        [DisplayName("Purchase Date")]
        public Nullable<DateTime> purchasedate { get; set; }
        [DisplayName("Added By")]
        public Nullable<int> AddedBy { get; set; }
        public IEnumerable<StoresMaterials> StoresMaterials { get; set; }
    }
}