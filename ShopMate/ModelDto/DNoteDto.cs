using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopMate.ModelDto
{
    public class DNoteDto
    {

        public int Id { get; set; }


        public List<DNoteMaterialDto> items { get; set; }


        public int? invoiceNo { get; set; }

        public string OrderNo { get; set; }


        public string CustomerUser { get; set; }

        public bool delivered { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyName { get; set; }
        public Nullable<DateTime> ddate { get; set; }
        public string ToInfo { get; set; }
        public string Logo { get; set; }
        public string Collector { get; set; }
        public string CollectorId { get; set; }
        public string VehicleRegNo { get; set; }
        public int InvoiceId { get; set; }
        public string CustomerMinerNo { get; set; }

    }
    public class DNoteMaterialDto
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public class InternalDNoteDto
    {

        public string WarehouseFrom { get; set; }

        public string WarehouseTo { get; set; }


        public int OrderNumber { get; set; }

        public DateTime Date { get; set; }
        public string CompanyAddress { get; set; }
        public string Warehouse { get; set; }
        public string receivedby { get; set; }

        public List<InternalDNoteMaterialDto> items { get; set; }

    }
    public class InternalDNoteMaterialDto
    {
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}