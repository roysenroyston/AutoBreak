using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShopMate.Models
{
    [TrackChanges]
    public class Expense
    {
        [DisplayName("S.No")] 
        public int Id { get; set; }

        [Required]
        [StringLength(200)] 
        [DisplayName("Remarks")] 
        public string Remarks { get; set; }
        [Required]
        [DisplayName("Amount")] 
        public Decimal Amount { get; set; }
        [DisplayName("SubTotal")]
        public Decimal SubTotal { get; set; }
        [DisplayName("Added By")] 
        public Nullable<int> AddedBy { get; set; }
        [DisplayName("Date Added")] 
        public Nullable<DateTime> DateAdded { get; set; }
        [Required]
        [DisplayName("Warehouse")] 
        public int WarehouseId { get; set; }
        [DisplayName("Expense")]
        public int? ExpenseId { get; set; }
        public virtual LedgerAccount LedgerAccount_LedgerAccountId { get; set; }

        [DisplayName("Invoice Number")]
        public string InvoiceNumber { get; set; }
        [DisplayName("Vat Amount")]
        public decimal TaxAmount { get; set; }
        [DisplayName("Vat Number")]
        public string VatNumber { get; set; }
        [DisplayName("Invoice Date")]
        public string InvoiceDate { get; set; }
        [DisplayName("Vendor Name")]
        public int? Vendorname { get; set; }
        public virtual User User_VendorUserId { get; set; }
        public virtual User User_UserFullName { get; set; }
        //public int? VendorUserId { get; set; }
       // public virtual User User_VendorUserId { get; set; }



    }
}
