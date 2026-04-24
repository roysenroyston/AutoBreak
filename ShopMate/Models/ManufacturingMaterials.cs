using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopMate.Models
{
    public class ManufacturingMaterial
    {
        [DisplayName("S.No")]
        public int Id { get; set; }

        [DisplayName("Finished Product")]
        public int FinishedGoodsId { get; set; }
        [DisplayName("Finished Product")]
        public string FinishedGoodsName { get; set; }
        [DisplayName("Finished Product")]
        public int FinishedGoodsQuantity { get; set; }
        [DisplayName("Cut Sheet")]
        public string CutSheet { get; set; }
        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        public bool Approved { get; set; }
        public decimal TotalAmount { get; set; }
        [DisplayName("Added By")]
        public Nullable<int> AddedBy { get; set; }
        [DisplayName("Date Added")]
        public Nullable<DateTime> DateAdded { get; set; }
        
        public IEnumerable<Manufacturing> Manufacturings { get; set; }
    }
}