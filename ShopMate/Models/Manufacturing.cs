using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopMate.Models
{

    public class Manufacturing
    {
        [DisplayName("S.No")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Quantity")]
        public Decimal Quantity { get; set; }
        [DisplayName("Unit Price")]
        public Decimal UnitPrice { get; set; }
        [DisplayName("Unit Total")]
        public Decimal UnitTotal { get; set; }
        public string CutSheet { get; set; }
        [Required]
        [DisplayName("Raw Material")]
        public int? RawMaterialsId { get; set; }
        public virtual RawMaterials RawMaterials_RawMaterialsId { get; set; }
        [DisplayName("Raw Material Name")]
        public string RawMaterialsname { get; set; }
        [DisplayName("Date Added")]
        public Nullable<DateTime> DateAdded { get; set; }
        [DisplayName("Authorised By")]
        public Nullable<int> AddedBy { get; set; }
        [Required]
        [DisplayName("Warehouse")]
        public int WarehouseId { get; set; }
        [DisplayName("Is Removed")]
        public bool IsRemoved { get; set; }
        //[Required]
        [DisplayName("Inventory Type")]
        public int? InventoryTypeId { get; set; }
        public Decimal Remaining { get; set; }
        public virtual InventoryType InventoryType_InventoryTypeId { get; set; }

        public ManufacturingMaterial ManufacturingMaterial { get; set; }

        [DisplayName("ManufacturingMaterial Id")]
        public int? ManufacturingMaterialId { get; set; }
        public virtual ManufacturingMaterial ManufacturingMaterial_ManufacturingMaterialId { get; set; }

        public IEnumerable<ManufacturingMaterial> manufacturings { get; set; }


    }
}