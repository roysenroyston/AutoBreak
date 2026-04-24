using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShopMate.Models
{
    [TrackChanges]
    public class ProductStock
    {
        [DisplayName("S.No")]
        public int Id { get; set; }
    //    [Required]
        [DisplayName("Product")]
        public int? ProductId { get; set; }
        public virtual Product Product_ProductId { get; set; }
     //   [Required]
        [DisplayName("Quantity")]
        public Decimal Quantity { get; set; }
        [DisplayName("Returned Quantity")]
        public Decimal ReturnedQuantity { get; set; }
        //[Required]
        [DisplayName("Purchase Price")]
        public Decimal PurchasePrice { get; set; }
        //[Required]
        [DisplayName("Total Purchase Amount")]
        public Decimal TotalPurchaseAmount { get; set; }
        //[Required]
        [DisplayName("Total Sale Amount")]
        public Decimal TotalSaleAmount { get; set; }
        //[Required]
        [DisplayName("Total Sale Amount With Tax")]
        public Decimal TotalSaleAmountWithTax { get; set; }
        [DisplayName("Discount")]
        public Nullable<Decimal> Discount { get; set; }
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
        [SkipTracking]
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Added By")]
        public Nullable<int> AddedBy { get; set; }
        [DisplayName("Date Added")]
        public Nullable<DateTime> DateAdded { get; set; }
        [DisplayName("Modified By")]
        public Nullable<int> ModifiedBy { get; set; }
        [DisplayName("Date Modied")]
        public Nullable<DateTime> DateModied { get; set; }
       // [Required]
        [DisplayName("Inventory Type")]
        public int? InventoryTypeId { get; set; }
        public virtual InventoryType InventoryType_InventoryTypeId { get; set; }
        //[Required]
        [DisplayName("Unit Selling Price")]
        public Decimal SalePrice { get; set; }
        //[Required]
        [SkipTracking]
        [DisplayName("Tax Amount for entered Quanties")]
        public Decimal TaxAmount { get; set; }
        //[Required]
        [DisplayName("Total Profit For the quanties entered")]
        public Decimal Profit { get; set; }
        //[Required]
        [DisplayName("Profit With Tax for quantities entered")]
        public Decimal ProfitWithTax { get; set; }
     //   [Required]
        [DisplayName("Warehouse")]
        public int WarehouseId { get; set; }

        public bool IsFormal { get; set; }
        public Decimal RemainingQuantity { get; set; }
    }
}
