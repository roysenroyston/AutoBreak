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
    public class QuotationController : Controller
    {
        string userId = Env.GetUserInfo("name");
        // GET: Quotation
        public ActionResult Index()
        {
            return View();
        }

        // GET Quotation/GetGrid
        public ActionResult GetGrid()
        {
            var tak = db.Quotations.ToArray();

            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(db.Users.FirstOrDefault(i => i.Id == (c.AddedBy)).FullName),
            Convert.ToString(db.Users.FirstOrDefault(i => i.Id == (c.customerId)).FullName),
            Convert.ToString(c.IssueDate),
            Convert.ToString(c.SubTotal),
            Convert.ToString(c.VAT),
            Convert.ToString(c.Total),
            // Convert.ToString(c.Remarks),
            Convert.ToString(c.ValidUntil),

          Convert.ToString(c.approved)
            //Convert.ToString(c.ModifiedBy)


            };

            //return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        // GET: /Quotation/ModelBindIndex
        public ActionResult ModelBindIndex()
        {
            return View();
        }

        // GET: Quotation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Quotation ObjQuotation = db.Quotations.Find(id);
            if (ObjQuotation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Quotations, "Id", "Name", ObjQuotation.Id);

            IEnumerable<QuotationItems> quotation = db.QuotationItems.Where(qt => qt.QuotationId.Equals(ObjQuotation.Id));
            ObjQuotation.items = quotation;

                       var quotationItems = db.QuotationItems.Where(q => q.QuotationId == id).ToArray();
            var CustVat = db.Users.FirstOrDefault(n => n.Id == ObjQuotation.customerId).vatNumber;
            var Quotation = db.Quotations.FirstOrDefault(i => i.Id == id);
            string currencySymbol;
            int cury = db.Quotations.FirstOrDefault(i => i.Id == id).CurrencyId;
            User user = new User();
            user = db.Users.FirstOrDefault(i => i.Id == ObjQuotation.AddedBy);
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
            //var jobcard = db.JobCards.FirstOrDefault(i => i.Id == id && i.WarehouseId == warehouse);
            //var user = db.Users.FirstOrDefault(i => i.Id == ObjQuotation.AddedBy);
            var customer = db.Users.FirstOrDefault(i => i.Id == ObjQuotation.customerId);

            var setting = db.Settings.Where(i => i.sGroup == "Quotation").ToArray();
            if (cury == 4)
            {
                currencySymbol = db.Settings.FirstOrDefault(i => i.sGroup == "USD").sValue; /*db.Currencies.FirstOrDefault(i => i.Id == Quotation.CurrencyId).CurrencySymbol;*/

            }
            else
            {
                currencySymbol = db.Settings.FirstOrDefault(i => i.sGroup == "Quotation").sValue; /*db.Currencies.FirstOrDefault(i => i.Id == Quotation.CurrencyId).CurrencySymbol;*/

            }

            if (ObjQuotation == null)
            {
                return HttpNotFound();
            }

            QuotationDto dto = new QuotationDto();
            //  dto.AddedBy = user.UserName;
            dto.IssueDate = ObjQuotation.IssueDate;

            dto.SubTotal = ObjQuotation.SubTotal;
            dto.Total = ObjQuotation.Total;
            dto.VAT = ObjQuotation.VAT;
            dto.customer = customer.FullName;
            dto.ValidUntil = ObjQuotation.ValidUntil;
            dto.CompanyVat = invoiceFormat.VatNumber;
            //dto.BP = ObjQuotation.
            dto.Id =(int) id;
            dto.Logo = Env.GetSiteRoot() + "/Uploads/" + invoiceFormat.Logo;
            dto.CompanyAddress = invoiceFormat.AddressInfo;
            dto.CompanyContact = invoiceFormat.OtherInfo;
            dto.CompanyName = invoiceFormat.CompanyName;
            dto.ToInfo = customer.Address + customer.Mobile;
            dto.Remarks = ObjQuotation.Remarks;
            //dto.CurrencySymbol = setting.FirstOrDefault(i => i.sKey == "CurrencySymbol").sValue;
            //dto.CurrencySymbol = db.Currencies.FirstOrDefault(i => i.Id == Quotation.CurrencyId).CurrencySymbol;
            dto.CurrencySymbol = currencySymbol;
            dto.CustomerVat = CustVat;

            List<QuotationItemsDto> itemsList = new List<QuotationItemsDto>();

            foreach (var items in quotationItems)
            {

               
                QuotationItemsDto itemDto = new QuotationItemsDto();
               
                    var totalwithoutvat = (items.UnitPrice * items.Quantity);
                    itemDto.Description = items.Description;
                    itemDto.Quantity = items.Quantity;
                    itemDto.UnitVat = (items.TotalPrice - totalwithoutvat);
                    itemDto.UnitVat = System.Math.Round(itemDto.UnitVat, 2);
                    itemDto.UnitPrice = items.UnitPrice;
                    itemDto.TotalPrice = System.Math.Round(items.UnitPrice * items.Quantity, 2);
                    itemDto.Name = db.Products.FirstOrDefault(i => i.Id == items.ProductId).Name;
                    itemDto.ProductId = items.ProductId;

                    itemsList.Add(itemDto);
          
                
            }

            dto.items = itemsList;

            return View(dto);

        }


        public class Cart
        {
           // public int Id { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public int UnitPrice { get; set; }
            public int UnitVat { get; set; }
            public int TotalPrice { get; set; }

        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Details1(int? Id , /*List<Cart>productss*/ QuotationItems[] productss)
        {
            Quotation objquotation = db.Quotations.Find(Id);
            string retVal = "";
            try { 
            Quotation ObjQuotation = new Quotation();

            ObjQuotation.customerId = objquotation.customerId;
            ObjQuotation.SubTotal = 0;
            ObjQuotation.Total = 0;
            ObjQuotation.VAT = 0;
            ObjQuotation.WarehouseId = objquotation.WarehouseId;
            ObjQuotation.Remarks = objquotation.Remarks;
            ObjQuotation.approved = objquotation.approved;
            ObjQuotation.IssueDate = DateTime.Now;
            ObjQuotation.AddedBy = objquotation.AddedBy;
            ObjQuotation.CurrencyId = objquotation.customerId;
            db.Quotations.Add(ObjQuotation);
            db.SaveChanges(userId);

                // var myId = db.Products.FirstOrDefault(i => i.Name = q)

                foreach (var item in productss)
                {
                    // var productId = db.WarehouseStocks.FirstOrDefault(i => i.Product_ProductId.Name == item.Name).ProductId;
                    if (item.UnitPrice != 0 && item.Quantity != 0)
                    {

                        QuotationItems objQuotationItems = new QuotationItems();
                        objQuotationItems.ProductId = item.ProductId;
                        objQuotationItems.Description = db.Products.FirstOrDefault(i => i.Id == item.ProductId).Name;
                        objQuotationItems.Quantity = item.Quantity;
                        objQuotationItems.UnitPrice = item.UnitPrice;
                        objQuotationItems.TotalPrice = objQuotationItems.UnitPrice * objQuotationItems.Quantity;
                        objQuotationItems.QuotationId = ObjQuotation.Id;
                        objQuotationItems.TaxId = item.TaxId;

                        if (objQuotationItems.TaxId == 2)
                        {
                            objQuotationItems.unitTax = (decimal)0.145 * objQuotationItems.TotalPrice;
                        }
                        else
                        {
                            objQuotationItems.unitTax = (decimal)0.00;
                        }
                        db.QuotationItems.Add(objQuotationItems);
                        db.SaveChanges(userId);

                        ObjQuotation.SubTotal = ObjQuotation.SubTotal + objQuotationItems.TotalPrice;
                        ObjQuotation.VAT = ObjQuotation.VAT + objQuotationItems.unitTax;
                        ObjQuotation.Total = ObjQuotation.SubTotal + ObjQuotation.VAT;
                        db.Entry(ObjQuotation).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            retVal = "Succes";
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                retVal = "error:" + ex.Message;
            }

            return Json(retVal, JsonRequestBehavior.AllowGet);
            //return View();
            //return View();
        }
        public ActionResult pGetDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ObjQuotation = db.Quotations.Where(i => i.customerId == (id) && i.approved == false).ToArray();
            if (ObjQuotation == null)
            {
                return HttpNotFound();
            }
            return Json(new { salesorders = ObjQuotation }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetQuotationItems(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var ObjQuotationtems = db.QuotationItems.Where(i => i.QuotationId == (id)).ToArray();
                var objQty = db.Quotations.FirstOrDefault(m => m.Id == (id));

                List<Quotation> lstQuotation = new List<Quotation>();
                Quotation materials = new Quotation();
                materials.Id = objQty.Id;
                materials.Remarks = objQty.Remarks;
                //materials.FinishedGoodsQuantity = objQty.FinishedGoodsQuantity;
                //lstQuotation.Add(materials);


                List<QuotationItems> lstQuotationItem = new List<QuotationItems>();
                foreach (var item in ObjQuotationtems)
                {
                    QuotationItems QuotationItem = new QuotationItems();
                    QuotationItem.ProductId = item.ProductId;
                    QuotationItem.Quantity = item.Quantity;
                    QuotationItem.Description = item.Description;
                    QuotationItem.UnitPrice = item.UnitPrice;
                    QuotationItem.TotalPrice = item.TotalPrice;
                    QuotationItem.TaxId = item.TaxId;
                    QuotationItem.unitTax = item.unitTax * item.Quantity;
                    QuotationItem.QuotationId = item.QuotationId;
                    //QuotationItem.totalAmountWithTax = item.TotalAmountWithTax;
                    //QuotationItem.saleOrderId = item.SaleOrderId;
                    QuotationItem.Name = db.Products.FirstOrDefault(i => i.Id == item.ProductId).Name;
                    lstQuotationItem.Add(QuotationItem);
                }
                if (ObjQuotationtems == null)
                {
                    return HttpNotFound();
                }
                // var result = JsonConvert.SerializeObject(new { data = lstQuotationItem, ngoni = db.Quotations.FirstOrDefault(i => i.Id == id) }, Formatting.Indented,
                var result = JsonConvert.SerializeObject(lstQuotationItem, Formatting.Indented,
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

        // GET: Quotation/Create
        public ActionResult Create()
        {
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "Name");
            ViewBag.ParentId = new SelectList(db.Quotations, "Id", "Name");
            ViewBag.PaymentModes = new SelectList(db.Currencies, "id", "Name");
            ViewBag.userId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "SaleMan"), "Id", "UserName");
            //ViewBag.CustomerUserId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.CustomerUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Customer"), "Id", "FullName");
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "Id", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            ViewBag.PaymentMethods = new SelectList(db.InvoicePaymentMethods, "id", "Name");
            //ViewBag.customerId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Customer"), "Id", "UserName");
            return View();
        }

        // POST: /Quotation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        //public ActionResult Create(Quotation ObjQuotation, QuotationItems[] QuotationItems)
        //public ActionResult NewInvoice(int customerid, int? orderNo, string vatReg, int ProjectNumber, decimal subtotal, decimal vat, decimal total, decimal payment, decimal balance, int wareId, int PaymentMethodId, InvoiceMaterials[] invoicemat, int? salesrepid)


        public ActionResult Create(int customerId, decimal SubTotal, decimal Total, decimal VAT, int wareId, bool approved, string Remarks, QuotationItems[] QuotationItems, int CurrencyId)
        {
            // public ActionResult Create(string customername, string jobNo, string address, string Description, string OrderNumber, decimal sandries, decimal totalbfvat, decimal VAT, decimal TotalAmountWithTax, QuotationItems[] QuotationItems, QuotationMaterials[] QuotationMaterials)

            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! Quotation  Is Not Complete!";
            Quotation ObjQuotation = new Quotation();
            try
            {
                if (ModelState.IsValid)
                {
                    ObjQuotation.customerId = customerId;
                    ObjQuotation.SubTotal = SubTotal;
                    ObjQuotation.Total = Total;
                    ObjQuotation.VAT = VAT;
                    ObjQuotation.WarehouseId = wareId;
                   ObjQuotation.Remarks = Remarks;
                    ObjQuotation.approved = approved;
                    ObjQuotation.IssueDate = DateTime.Now;
                    ObjQuotation.AddedBy = AddedBy;
                    ObjQuotation.CurrencyId = CurrencyId;




                    db.Quotations.Add(ObjQuotation);

                    foreach (var quoteItems in QuotationItems)
                    {
                        QuotationItems objQuotationItems = new QuotationItems();
                        objQuotationItems.ProductId = quoteItems.ProductId;
                        objQuotationItems.Quantity = quoteItems.Quantity;
                        objQuotationItems.Description = quoteItems.Description;
                        objQuotationItems.TotalPrice = quoteItems.TotalPrice;
                        objQuotationItems.QuotationId = ObjQuotation.Id;
                        objQuotationItems.TaxId = db.Products.FirstOrDefault(i => i.Id == quoteItems.ProductId).TaxId;
                        objQuotationItems.UnitPrice = quoteItems.UnitPrice;
                        objQuotationItems.unitTax = quoteItems.unitTax;
                       

                       
                        db.QuotationItems.Add(objQuotationItems);
                        db.SaveChanges(userId);

                        //ObjQuotation.VAT = ob
                    }

                    db.SaveChanges(userId);

                    result = "Submitted";
                    return Json(result, JsonRequestBehavior.AllowGet);
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

        // GET: Quotation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Quotation ObjQuotation = db.Quotations.Find(id);
            if (ObjQuotation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Quotations, "Id", "Name", ObjQuotation.Id);

            IEnumerable<QuotationItems> quotation = db.QuotationItems.Where(qt => qt.QuotationId.Equals(ObjQuotation.Id));
            ObjQuotation.items = quotation;
            return View(ObjQuotation);
        }

        // POST: Quotation/Edit/5        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Quotation objQuotation)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
            try
            {
                if (ModelState.IsValid)
                {

                    objQuotation.ModifiedBy = AddedBy;
                    objQuotation.ValidUntil = DateTime.Now;
                    db.Entry(objQuotation).State = EntityState.Modified;
                    db.SaveChanges(userId);

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

        // GET: Quotation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation ObjQuotation = db.Quotations.Find(id);
            if (ObjQuotation == null)
            {
                return HttpNotFound();
            }
            return View(ObjQuotation);
        }

        // POST: /Quotation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {

                Quotation ObjQuotation = db.Quotations.Find(id);
                db.Quotations.Remove(ObjQuotation);
                db.SaveChanges(userId);

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

        // GET: /Quotation/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            Quotation ObjQuotation = db.Quotations.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;
                ViewBag.ParentId = new SelectList(db.Quotations, "Id", "MenuText", ObjQuotation.Id);

            }

            return View(ObjQuotation);
        }

        // POST: /Quotation/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(Quotation ObjQuotation)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjQuotation).State = EntityState.Modified;
                    db.SaveChanges(userId);

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
   
        public ActionResult print(int id)
        {

           
            Quotation ObjQuotation = db.Quotations.Find(id);
            var quotationItems = db.QuotationItems.Where(q => q.QuotationId == id).ToArray();
            var CustVat = db.Users.FirstOrDefault(n => n.Id == ObjQuotation.customerId).vatNumber;
            var Quotation = db.Quotations.FirstOrDefault(i => i.Id == id);
            string currencySymbol;
            int cury = db.Quotations.FirstOrDefault(i => i.Id == id).CurrencyId;
            User user = new User();
            user = db.Users.FirstOrDefault(i => i.Id == ObjQuotation.AddedBy);
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
            //var jobcard = db.JobCards.FirstOrDefault(i => i.Id == id && i.WarehouseId == warehouse);
            //var user = db.Users.FirstOrDefault(i => i.Id == ObjQuotation.AddedBy);
            var customer = db.Users.FirstOrDefault(i => i.Id == ObjQuotation.customerId);
          
            var setting = db.Settings.Where(i => i.sGroup == "Quotation").ToArray();
            if ( cury == 4)
            {
                currencySymbol = db.Settings.FirstOrDefault(i => i.sGroup == "USD").sValue; /*db.Currencies.FirstOrDefault(i => i.Id == Quotation.CurrencyId).CurrencySymbol;*/

            }
            else
            {
            currencySymbol = db.Settings.FirstOrDefault(i => i.sGroup == "Quotation").sValue; /*db.Currencies.FirstOrDefault(i => i.Id == Quotation.CurrencyId).CurrencySymbol;*/

            }

            if (ObjQuotation == null)
            {
                return HttpNotFound();
            }

            QuotationDto dto = new QuotationDto();
            //  dto.AddedBy = user.UserName;
            dto.IssueDate = ObjQuotation.IssueDate;

            dto.SubTotal = ObjQuotation.SubTotal;
            dto.Total = ObjQuotation.Total;
            dto.VAT = ObjQuotation.VAT;
            dto.customer = customer.FullName;
            dto.ValidUntil = ObjQuotation.ValidUntil;
            dto.CompanyVat = invoiceFormat.VatNumber;
            //dto.BP = ObjQuotation.
            dto.Id = id;
            dto.Logo = Env.GetSiteRoot() + "/Uploads/" + invoiceFormat.Logo;
            dto.CompanyAddress = invoiceFormat.AddressInfo;
            dto.CompanyContact = invoiceFormat.OtherInfo;
            dto.CompanyName = invoiceFormat.CompanyName;
            dto.ToInfo = customer.Address  + customer.Mobile;
           // dto.Remarks = ObjQuotation.Remarks;
            //dto.CurrencySymbol = setting.FirstOrDefault(i => i.sKey == "CurrencySymbol").sValue;
            //dto.CurrencySymbol = db.Currencies.FirstOrDefault(i => i.Id == Quotation.CurrencyId).CurrencySymbol;
            dto.CurrencySymbol = currencySymbol;
            dto.CustomerVat = CustVat;

            List<QuotationItemsDto> itemsList = new List<QuotationItemsDto>();

            foreach (var items in quotationItems)
            {
                
                QuotationItemsDto itemDto = new QuotationItemsDto();
                var totalwithoutvat = (items.UnitPrice * items.Quantity);
                itemDto.Description = items.Description;
                itemDto.Quantity = items.Quantity;
                itemDto.UnitVat =items.unitTax;
                //itemDto.UnitVat = System.Math.Round(itemDto.UnitVat, 2);
                itemDto.UnitPrice = items.UnitPrice;
                itemDto.TotalPrice = System.Math.Round(items.UnitPrice* items.Quantity,2);               
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