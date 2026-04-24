using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShopMate.Models
{
    [TrackChanges]
    public class StockShippingOrder
    {

        [DisplayName("S.No")]
        public int Id { get; set; }
        [Required]
        [DisplayName("From")]
        public int? WarehouseFrom { get; set; }
        [Required]
        [DisplayName("To")]
        public int? WarehouseTo { get; set; }
        [Required]
        [DisplayName("Date Added")]
        public DateTime DateAdded { get; set; }
        [Required]
        [DisplayName("Date Modified")]
        public DateTime DateModified { get; set; }
        [DisplayName("Added By")]
        public Nullable<int> AddedBy { get; set; }
        [Required]
        [DisplayName("Warehouse")]
        public int WarehouseId { get; set; }
        [DisplayName("Is Dispatched")]
        public bool IsDispatched { get; set; }
        [DisplayName("Is Received")]
        public bool IsReceived { get; set; }
        [DisplayName("Modified By")]
        public int? ModifiedBy { get; set; }
        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        [Required]
        [DisplayName("Warehouse")]
        public int Warehouse { get; set; }
        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

    }
}