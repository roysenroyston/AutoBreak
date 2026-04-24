using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopMate.ModelDto
{
    public class GRVDto
    {
        public int Id { get; set; }
        public int? supplier { get; set; }
        public string receivedby { get; set; }
        public int OrderNumber { get; set; }
        
        public Nullable<DateTime> purchasedate { get; set; }
        public List<GRVMaterialsDto> GRVMaterials { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string Warehouse { get; set; }
        public string SupplierInfo { get; set; }
        //public string DispatchAt { get; set; }
        public string companayname { get; set; }
        public List<GRVMaterialsDto> Items { get; set; }
    }

    public class GRVMaterialsDto
    {
        public int Id { get; set; }
        public Decimal Quantity { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
        public String Product { get; set; }
        public string Product_ProductId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal TotalPrice { get; set; }
        public List<GRVMaterialsDto> GRVMaterials { get; set; }

    }
}