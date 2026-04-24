using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMate.Models
{
    [TrackChanges]
    public class Sale
    {
        [DisplayName("S.No")] 
        public int Id { get; set; }
        [Required]
        [DisplayName("Customer User")] 
        public int? CustomerUserId { get; set; }
        [DisplayName("Invoice Id")]
        public int? InvoiceId { get; set; }
        public virtual User User_CustomerUserId { get; set; }
        [Required]
        [DisplayName("Quantity")] 
        public Decimal Quantity { get; set; }
        [DisplayName("Returned Quantity")]
        public Decimal ReturnedQuantity { get; set; }
        [DisplayName("RemainingQuantity")]
        public int RemainingQuantity { get; set; }
        [Required]
        [DisplayName("Sale Price")] 
        public Decimal SalePrice { get; set; }
        [Required]
        [SkipTracking]
        [DisplayName("Payment Mode")]
        public int? PaymentModeId { get; set; }
        public virtual PaymentMode PaymentMode_PaymentModeId { get; set; }
        [Required]
        [DisplayName("Total Amount")] 
        public Decimal TotalAmount { get; set; }

        [DisplayName("Total Amount With Tax")]
        public Nullable<Decimal> TotalAmountWithTax { get; set; }
        
        [Required]
        [DisplayName("Paid Amount")] 
        public Decimal PaidAmount { get; set; }
        [Required]
        [DisplayName("Product")]
        [Index("IX_RecieptProduct", 1, IsUnique = true)]
        public int? ProductId { get; set; }
        public virtual Product Product_ProductId { get; set; }
        [DisplayName("Date Added")] 
        public Nullable<DateTime> DateAdded { get; set; }
        [SkipTracking]
        [DisplayName("Modified By")] 
        public Nullable<int> ModifiedBy { get; set; }
        [SkipTracking]
        [DisplayName("Date Modied")] 
        public Nullable<DateTime> DateModied { get; set; }
        [DisplayName("Added By")] 
        public Nullable<int> AddedBy { get; set; }
        [Required]
        [DisplayName("Warehouse")] 
        public int WarehouseId { get; set; }
        [DisplayName("Inventory Type")]
        public int? InventoryTypeId { get; set; }
        [DefaultValue(0.00)]
        [DisplayName("Ecocash")]
        public decimal ecocash { get; set; }
        [DefaultValue(0.00)]
        [DisplayName("Telecash")]
        public decimal telecash { get; set; }
        [DefaultValue(0.00)]
        [DisplayName("One Money")]
        public decimal onemoney { get; set; }
        [DefaultValue(0.00)]
        [DisplayName("RTGS")]
        public decimal rtgs { get; set; }
        [SkipTracking]
        [DefaultValue(0.00)]
        [DisplayName("USD")]
        public decimal usd { get; set; }
        [DefaultValue(0.00)]
        [DisplayName("BOND ")]
        public decimal bond { get; set; }
        [SkipTracking]
        [DefaultValue(0.00)]
        [DisplayName("FCA")]
        public decimal fca { get; set; }
        public bool isFormalSale { get; set; }
        [DisplayName("Customer Info")]
        public string CustomerInfo { get; set; }
        [DefaultValue("")]
        [DisplayName("RecieptNumber")]
        [Index("IX_RecieptProduct", 2, IsUnique = true)]
        public int recieptNumber { get; set; }
        [DefaultValue("")]
        public string customerName { get; set; }


    }
}
