using Newtonsoft.Json;
using ShopMate.ModelDto;
using ShopMate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebErrorLogging.Utilities;

namespace ShopMate.Controllers
{
    public class DeliveryNoteController : BaseController
    {

        string userId = Env.GetUserInfo("name");
        // GET: DeliveryNote
        public ActionResult Index()
        {
            return View();
        }

        //Get: DeliveryNote/GetGrid
        public ActionResult GetGrid()
        {
            var tak = db.DeliveryNotes.ToArray();

            var result = from c in tak
                         select new string[]
                {
                    c.Id.ToString(), Convert.ToString(c.Id),

                    Convert.ToString(c.invoiceNo),
                    Convert.ToString(c.OrderNo),

                    Convert.ToString(c.ddate),
                    Convert.ToString(c.CustomerUserId)
                };

            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        //Get: DeliveryNote/DeliveredGetGrid
        public ActionResult DeliveredGetGrid()
        {
            var tak = db.DeliveryNotes.Where(i => i.delivered == true).ToArray();

            var result = from c in tak
                         select new string[]
{
                c.Id.ToString(), Convert.ToString(c.Id),

                Convert.ToString(c.invoiceNo),
                Convert.ToString(c.OrderNo),
                Convert.ToString(c.ddate),
                Convert.ToString(c.CustomerUserId)
};

            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        //Get: DeliveryNote/DeliveredGetGrid
        public ActionResult UnDeliveredGetGrid()
        {
            var tak = db.DeliveryNotes.Where(i => i.delivered == false).ToArray();

            var result = from c in tak
                         select new string[]
{
                c.Id.ToString(), Convert.ToString(c.Id),
                Convert.ToString(c.invoiceNo),
                Convert.ToString(c.OrderNo),
                Convert.ToString(c.ddate),
                Convert.ToString(c.CustomerUserId)
};

            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        // GET: /DeliveryNote/ModelBindIndex
        public ActionResult ModelBindIndex()
        {
            return View();
        }

        // GET: DeliveryNote/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryNote ObjDeliveryNote = db.DeliveryNotes.Find(id);
            if (ObjDeliveryNote == null)
            {
                return HttpNotFound();
            }
            return View(ObjDeliveryNote);
        }

        // GET: DeliveryNote/Create
        public ActionResult Create()
        {
            ViewBag.CustomerUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Customer"), "Id", "FullName");
            ViewBag.ordernumber = new SelectList(db.Orders, "Id", "UserName");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: DeliveryNote/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Create(DeliveryNote objDeliveryNote, DNoteMaterial[] dnotematerial)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! DNote  Is Not Complete!";
            try
            {
                if (ModelState.IsValid)
                {


                    db.DeliveryNotes.Add(objDeliveryNote);

                    foreach (var quoteItems in dnotematerial)
                    {
                        DNoteMaterial Items = new DNoteMaterial();

                        Items.ProductId = quoteItems.ProductId;
                        Items.Quantity = quoteItems.Quantity;
                        Items.Description = quoteItems.Description;
                        Items.DNoteId = objDeliveryNote.Id;
                        //    Items.DeliveryNote = objDeliveryNote;

                        db.DNoteMaterials.Add(Items);
                    }
                    var customer = db.Users.FirstOrDefault(i => i.Id == objDeliveryNote.CustomerUserId);
                    var selectedInvoice = db.InformalInvoices.FirstOrDefault(i => i.Id == objDeliveryNote.invoiceNo);
                    var selectedInvoice2 = db.Invoices.FirstOrDefault(i => i.Id == objDeliveryNote.invoiceNo);
                    if (customer.vatNumber == "" || customer.vatNumber == null)
                    {
                        selectedInvoice.DNote = true;
                        db.Entry(selectedInvoice).State = EntityState.Modified;
                    }
                    else
                    {
                        selectedInvoice2.DNote = true;
                        db.Entry(selectedInvoice2).State = EntityState.Modified;
                    }

                    //selectedInvoice.DNote = true;
                    //db.Entry(selectedInvoice).State = EntityState.Modified;
                    db.SaveChanges(userId);
                    //db.SaveChanges();

                    //result = "Success! Delivery Note Created";
                    //return Json(result, JsonRequestBehavior.AllowGet);

                    sb.Append("Submitted");
                    result = "Submitted";
                    List<DispatchReturn> retVal = new List<DispatchReturn>();
                    //retVal.Add(new SaleReturn { msg = "Submitted", value = invoiceId });
                    retVal.Add(new DispatchReturn { msg = "Submitted", value = objDeliveryNote.Id });
                    //return Json(returval, JsonRequestBehavior.AllowGet);
                    return Json(retVal, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    foreach (var key in this.ViewData.ModelState.Keys)
                    {
                        foreach (var err in this.ViewData.ModelState[key].Errors)
                        {
                            sb.Append(err.ErrorMessage + "<br/>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }

            return Content(sb.ToString());
        }

        public class DispatchReturn
        {
            public string msg { get; set; }
            public int value { get; set; }

        }

        // GET: DeliveryNote/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CustomerUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Customer"), "Id", "UserName");
            DeliveryNote ObjDeliveryNote = db.DeliveryNotes.Find(id);
            if (ObjDeliveryNote == null)
            {
                return HttpNotFound();
            }

            return View(ObjDeliveryNote);
        }

        // POST: DeliveryNote/Edit/5
        [HttpPost]
        public ActionResult Edit(DeliveryNote ObjDeliveryNote)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjDeliveryNote).State = EntityState.Modified;
                    db.SaveChanges();

                    sb.Append("Sumitted");
                    return Content(sb.ToString());
                }
                else
                {
                    foreach (var key in this.ViewData.ModelState.Keys)
                    {
                        foreach (var err in this.ViewData.ModelState[key].Errors)
                        {
                            sb.Append(err.ErrorMessage + "<br/>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }


            return Content(sb.ToString());
        }

        // GET: DeliveryNote/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryNote ObjDeliveryNote = db.DeliveryNotes.Find(id);
            if (ObjDeliveryNote == null)
            {
                return HttpNotFound();
            }
            return View(ObjDeliveryNote);
        }

        // POST: DeliveryNote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {

                DeliveryNote ObjDeliveryNote = db.DeliveryNotes.Find(id);
                db.DeliveryNotes.Remove(ObjDeliveryNote);
                db.SaveChanges();

                sb.Append("Sumitted");
                return Content(sb.ToString());

            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }

            return Content(sb.ToString());
        }

        // GET: /DeliveryNote/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            DeliveryNote ObjDeliveryNote = db.DeliveryNotes.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;

            }

            return View(ObjDeliveryNote);
        }

        // POST: /DeliveryNote/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(DeliveryNote ObjDeliveryNote)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjDeliveryNote).State = EntityState.Modified;
                    db.SaveChanges();

                    sb.Append("Sumitted");
                    return Content(sb.ToString());
                }
                else
                {
                    foreach (var key in this.ViewData.ModelState.Keys)
                    {
                        foreach (var err in this.ViewData.ModelState[key].Errors)
                        {
                            sb.Append(err.ErrorMessage + "<br/>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }

            return Content(sb.ToString());

        }
        public ActionResult GetDeliveryInvoices(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool Isformal = true;
            List<Invoice> LstInvoice = new List<Invoice>();
            List<InformalInvoice> LstInformalInvoice = new List<InformalInvoice>();
            var vatNumber = db.Users.FirstOrDefault(i => i.Id == id).vatNumber;
            if (vatNumber == null)
            {
                Isformal = false;
            }
            var ObjInvoices = db.Invoices.Where(i => i.CustomerId == (id)).ToArray();
            var ObjInformalInvoices = db.InformalInvoices.Where(i => i.CustomerId == id && i.DNote == false).ToArray();
            if (ObjInformalInvoices == null && ObjInvoices == null)
            {
                return HttpNotFound();
            }
            if (Isformal)
            {
                foreach (var item in ObjInvoices)
                {
                    Invoice SInvoice = new Invoice();
                    SInvoice.Id = item.Id;
                    LstInvoice.Add(SInvoice);

                }
                return Json(new { salesorders = LstInvoice }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var item in ObjInformalInvoices)
                {
                    InformalInvoice SIInvoice = new InformalInvoice();
                    SIInvoice.Id = item.Id;
                    LstInformalInvoice.Add(SIInvoice);

                }
                return Json(new { salesorders = LstInformalInvoice }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDeliveryNoteItems(string id)
        {
            int Id = 0;
            int CustomerId = 0;
            bool IsFormal = true;
            try
            {
                if (id == null)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }
                else
                {
                    string[] broken_str = id.Split(',');
                    Id = int.Parse(broken_str[0]);
                    CustomerId = int.Parse(broken_str[1].Trim());
                }
                var vatNumber = db.Users.FirstOrDefault(i => i.Id == CustomerId).vatNumber;
                if (vatNumber == null)
                {
                    IsFormal = false;
                }
                if (IsFormal)
                {

                }
                var ObjInvoicetems = db.InvoiceItemss.Where(i => i.InvoiceId == (Id)).ToArray();
                var ObjInformalInvoicetems = db.InvoiceItemss.Where(i => i.InformalInvoiceId == Id).ToArray();
                List<DNoteMaterial> objDNoteMAterial = new List<DNoteMaterial>();
                if (IsFormal)
                {
                    foreach (var item in ObjInvoicetems)
                    {
                        DNoteMaterial InvoiceItem = new DNoteMaterial();
                        InvoiceItem.ProductId = item.ProductId;
                        //InvoiceItem. = item.ProductId;
                        InvoiceItem.Description = db.Products.FirstOrDefault(i => i.Id == item.ProductId).Name + " " + db.Products.FirstOrDefault(i => i.Id == item.ProductId).ProductDescription;
                        InvoiceItem.Quantity = item.Quantity;
                        //InvoiceItem.Name = db.Products.FirstOrDefault(i => i.Id == item.ProductId).Name;
                        objDNoteMAterial.Add(InvoiceItem);
                    }
                }
                else
                {
                    foreach (var items in ObjInformalInvoicetems)
                    {
                        DNoteMaterial InvoiceItem = new DNoteMaterial();
                        InvoiceItem.ProductId = items.ProductId;
                        InvoiceItem.Description = db.Products.FirstOrDefault(i => i.Id == items.ProductId).Name + " " + db.Products.FirstOrDefault(i => i.Id == items.ProductId).ProductDescription;
                        InvoiceItem.Quantity = items.Quantity;
                        objDNoteMAterial.Add(InvoiceItem);
                    }
                }

                if (ObjInvoicetems == null)
                {
                    return HttpNotFound();
                }
                ////var result = JsonConvert.SerializeObject(objDNoteMAterial, Formatting.Indented,
                ////               new JsonSerializerSettings
                ////               {
                ////                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                ////               });

                //////return Json(result, JsonRequestBehavior.AllowGet);
                ////return Json(new { InvoiceItems = objDNoteMAterial }, JsonRequestBehavior.AllowGet);
                var result = JsonConvert.SerializeObject(objDNoteMAterial, Formatting.Indented,
                              new JsonSerializerSettings
                              {
                                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                              });

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult print(int id)
        {
            DeliveryNote dnote = db.DeliveryNotes.Find(id);
            var DnoteItems = db.DNoteMaterials.Where(q => q.DNoteId == id).ToArray();

            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
            //var jobcard = db.JobCards.FirstOrDefault(i => i.Id == id && i.WarehouseId == warehouse);
            var user = db.Users.FirstOrDefault(i => i.Id == dnote.CustomerUserId);

            if (dnote == null)
            {
                return HttpNotFound();
            }

            DNoteDto dto = new DNoteDto();

            dto.invoiceNo = dnote.invoiceNo;
            dto.OrderNo = dnote.OrderNo;
            dto.CustomerUser = user.UserName;
            dto.delivered = dnote.delivered;
            dto.CompanyAddress = invoiceFormat.AddressInfo;
            dto.CompanyContact = invoiceFormat.OtherInfo;
            dto.CompanyName = invoiceFormat.CompanyName;
            dto.Id = dnote.Id;
            dto.Logo = invoiceFormat.Logo;
            dto.ToInfo = user.Address + "<br/> " + user.Mobile + "<br/> " + user.About;
            dto.Collector = dnote.CollectedBy;
            dto.CollectorId = dnote.CollectorId;
            dto.VehicleRegNo = dnote.CollectingVehicleRegNo;
            //dto.CustomerMinerNo = user.MinePermitNumber;
            List<DNoteMaterialDto> itemsList = new List<DNoteMaterialDto>();

            foreach (var items in DnoteItems)
            {
                DNoteMaterialDto itemDto = new DNoteMaterialDto();
                itemDto.Description = items.Description;
                itemDto.Quantity = items.Quantity;
                itemDto.Name = db.Products.FirstOrDefault(i => i.Id == items.ProductId).Name;

                itemsList.Add(itemDto);
            }

            dto.items = itemsList;

            return View(dto);

        }
        private SIContext db = new SIContext();


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
