using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopMate.Models
{
    public class RawMaterialStock
    {
        [DisplayName("S.No")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Raw Material")]
        public int? RawMaterialsId { get; set; }
        public virtual RawMaterials RawMaterials_RawMaterialsId { get; set; }
        [Required]
        [DisplayName("Quantity")]
        public Decimal Quantity { get; set; }
        [DisplayName("Purchase Price")]
        public Decimal PurchasePrice { get; set; }
        [DisplayName("Total Purchase Amount")]
        public Decimal TotalPurchaseAmount { get; set; }
        [SkipTracking]
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Added By")]
        public Nullable<int> AddedBy { get; set; }
        [DisplayName("Date Added")]
        public Nullable<DateTime> DateAdded { get; set; }
        [Required]
        [DisplayName("Inventory Type")]
        public int? InventoryTypeId { get; set; }
        public virtual InventoryType InventoryType_InventoryTypeId { get; set; }
        [Required]
        [DisplayName("Warehouse")]
        public int WarehouseId { get; set; }
        [SkipTracking]
        [DisplayName("C G S T")]
        public Nullable<int> CGST { get; set; }
        [SkipTracking]
        [DisplayName("C G S T_ Rate")]
        public Nullable<Decimal> CGST_Rate { get; set; }
        [SkipTracking]
        [DisplayName("S G S T")]
        public Nullable<int> SGST { get; set; }
        [SkipTracking]
        [DisplayName("S G S T_ Rate")]
        public Nullable<Decimal> SGST_Rate { get; set; }
        [SkipTracking]
        [DisplayName("I G S T")]
        public Nullable<int> IGST { get; set; }
        [SkipTracking]
        [DisplayName("I G S T_ Rate")]
        public Nullable<Decimal> IGST_Rate { get; set; }
        [SkipTracking]
        [DisplayName("Tax")]
        public Nullable<int> TaxId { get; set; }
        [SkipTracking]
        [DisplayName("Other Tax Value")]
        public Nullable<Decimal> OtherTaxValue { get; set; }
        [DisplayName("Total Purchase Amount With Tax")]
        public Nullable<Decimal> TotalPurchaseAmountWithTax { get; set; }
        [DisplayName("Tax Amount")]
        public Nullable<Decimal> TaxAmount { get; set; }
    }
}