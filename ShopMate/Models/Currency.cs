using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopMate.Models
{
    [TrackChanges]
    public class Currency
    {  
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("Currency")]
        public string Name { get; set; }
        public string CurrencySymbol { get; set; }
        public int WarehouseId { get; set; }
    }
}