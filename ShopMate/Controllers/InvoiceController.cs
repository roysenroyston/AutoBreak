using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ShopMate.Models;
using ShopMate.ModelDto;
using WebErrorLogging.Utilities;
using System.Web.Services;
using System.Security.Claims;
using System.Threading;

namespace ShopMate.Controllers
{
    public class InvoiceController : BaseController
    {
        string userId = Env.GetUserInfo("name");
        // GET: /Invoice/ for purchase
        public ActionResult Index()
        {
            return View();
        }


        //new invoice
        //new invoice
        public ActionResult NewInvoice()
        {

            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "Name");
            ViewBag.CustomerUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Customer"), "Id", "FullName");
            ViewBag.CustomerVATReg = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Customer"), "Id", "CustomerVatReg");
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "Id", "Name");
            ViewBag.salesrepId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "SalesRep"), "Id", "UserName");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            ViewBag.PaymentMethods = new SelectList(db.InvoicePaymentMethods, "id", "Name");
            ViewBag.InvoicePaymentsMethod = new SelectList(db.InvoicePaymentMethods, "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewInvoice(int customerid, int? orderNo, string vatReg, int ProjectNumber,  decimal subtotal, decimal currencysubtotal, decimal vat, decimal currencyvat, List<CurrencyAmount> currencyAmounts, decimal total, decimal currencytotal, decimal payment, decimal balance, int wareId, int PaymentMethodId, InvoiceMaterials[] invoicemat, int? salesrepid, string remarks, int InvoicePaymentsMethod, int CurrencyId)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            // int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            DateTime today = DateTime.Now;
            // Get the claims values
            int warehouse = Int16.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.Actor)
                   .Select(c => c.Value).SingleOrDefault());
            var roysen = db.PaymentModes.Where(kk => kk.Name == db.Currencies.Where(dm => dm.Id == CurrencyId).FirstOrDefault().Name).FirstOrDefault().Id;
            //foreach (var item in invoicemat)
            //{
            //    var warehousestock1 = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == item.ProductId && i.WarehouseId == warehouse);
            //    if (item.quantity > warehousestock1.RemainingQuantity)
            //    {
            //        //sb.Append("Hazviko");
            //        return Content(sb.ToString());
            //    }

            //}
            foreach (var ngoni in invoicemat)
            {
               
                var warehousestock = db.WarehouseStocks.FirstOrDefault(i=>i.ProductId == ngoni.ProductId && i.WarehouseId == warehouse);
               
                {
                    var selectedSaleOrder = db.Quotations.FirstOrDefault(i => i.Id == orderNo);
                    bool IsFormalInvoice = true;
                    if (db.Users.FirstOrDefault(i => i.Id == (customerid)).vatNumber == null || db.Users.FirstOrDefault(i => i.Id == (customerid)).vatNumber == "")
                    {
                        IsFormalInvoice = false;

                    }
                    int invoiceId = 0;
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string returval = "Invoice failed to process ";
                    int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
                    //var PaymentMode = "";
                    try

                    {
                       

                        if (ModelState.IsValid)
                        {

                            Invoice ObjInvoice = new Invoice();
                            InformalInvoice ObjInformalInvoices = new InformalInvoice();
                    
                            if (IsFormalInvoice == true)
                            {
                                ObjInvoice.UserId = customerid;
                                ObjInvoice.orderNumber = orderNo;
                                // ObjInvoice.InvoiceNo = jobNo;
                                ObjInvoice.IsBilled = false;
                                ObjInvoice.CustomerVatReg = vatReg;
                                ObjInvoice.ProjectNumber = ProjectNumber;
                                ObjInvoice.subtotal = subtotal;
                                ObjInvoice.vat = vat;
                                ObjInvoice.total = total;
                                ObjInvoice.currencysubtotal = currencysubtotal;
                                ObjInvoice.currencyvat = currencyvat;
                                ObjInvoice.currencytotal = currencytotal;
                                ObjInvoice.payment = payment;
                                ObjInvoice.balance = balance;
                                ObjInvoice.CurrencyId = CurrencyId;
                                ObjInvoice.IsPurchaseOrSale = "Sale";
                                ObjInvoice.WarehouseId = warehouse;
                                ObjInvoice.DispatchAt = wareId;
                                ObjInvoice.DateAdded = DateTime.Now;
                                ObjInvoice.DateModied = DateTime.Now;
                                ObjInvoice.AddedBy = AddedBy;
                                ObjInvoice.InvoiceNo = ObjInvoice.Id;
                                ObjInvoice.salesrepId = salesrepid;
                                ObjInvoice.Remarks = remarks;
                                ObjInvoice.CustomerId = customerid;
                                ObjInvoice.InvoicePaymentMethodId = InvoicePaymentsMethod;
                                var duein = db.InvoicePaymentMethods.FirstOrDefault(i => i.Id == (ObjInvoice.InvoicePaymentMethodId)).DueIn;

                                DateTime answer = today.AddDays(duein);
                                ObjInvoice.Duedate = answer;

                                db.Invoices.Add(ObjInvoice);
                                db.SaveChanges(userId);
                            }
                            else
                            {
                                ObjInformalInvoices.UserId = customerid;
                                ObjInformalInvoices.orderNumber = orderNo;
                                // ObjInvoice.InvoiceNo = jobNo;
                                ObjInformalInvoices.IsBilled = false;
                                ObjInformalInvoices.CustomerVatReg = vatReg;
                                ObjInformalInvoices.ProjectNumber = ProjectNumber;
                                ObjInformalInvoices.subtotal = subtotal;
                                ObjInformalInvoices.vat = vat;
                                ObjInformalInvoices.total = total;
                                ObjInformalInvoices.currencysubtotal = currencysubtotal;
                                ObjInformalInvoices.currencyvat = currencyvat;
                                ObjInformalInvoices.currencytotal = currencytotal;
                                ObjInformalInvoices.payment = payment;
                                ObjInformalInvoices.balance = balance;
                                ObjInformalInvoices.CurrencyId = CurrencyId;
                                ObjInformalInvoices.IsPurchaseOrSale = "Sale";
                                ObjInformalInvoices.WarehouseId = warehouse;
                                ObjInformalInvoices.DispatchAt = wareId;
                                ObjInformalInvoices.DateAdded = DateTime.Now;
                                ObjInformalInvoices.DateModied = DateTime.Now;
                                ObjInformalInvoices.AddedBy = AddedBy;
                                ObjInformalInvoices.InvoiceNo = ObjInvoice.Id;
                                ObjInformalInvoices.salesrepId = salesrepid;
                                ObjInformalInvoices.InvoicePaymentMethodId = InvoicePaymentsMethod;
                                ObjInformalInvoices.Remarks = remarks;
                                ObjInformalInvoices.CustomerId = customerid;
                                var duein = db.InvoicePaymentMethods.FirstOrDefault(i => i.Id == (ObjInformalInvoices.InvoicePaymentMethodId)).DueIn;
                                DateTime answer = today.AddDays(duein);
                                ObjInformalInvoices.Duedate = answer;
                                db.InformalInvoices.Add(ObjInformalInvoices);
                                db.SaveChanges(userId);
                            }
                            db.SaveChanges(userId);
                            Sale ObjSale = new Models.Sale();

                            foreach (var inmat in invoicemat)
                            {
                                var selectedProduct = db.Products.FirstOrDefault(i => i.Id == inmat.ProductId);

                                var ObjWarehouseStock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == inmat.ProductId && i.WarehouseId == warehouse);
                                var selectedTax = db.Taxs.FirstOrDefault(i => i.Id == selectedProduct.TaxId);
                                // Add to invoice items


                                ObjSale.ProductId = inmat.ProductId;
                                ObjSale.Quantity = inmat.quantity;
                                ObjSale.SalePrice = inmat.rate;
                                ObjSale.TotalAmount = (inmat.rate * ObjSale.Quantity);
                                ObjSale.WarehouseId = wareId;
                                ObjSale.AddedBy = AddedBy;
                                ObjSale.CustomerUserId = customerid;
                                ObjSale.DateAdded = DateTime.Now;
                                ObjSale.DateModied = DateTime.Now;
                                ObjSale.ModifiedBy = AddedBy;
                                ObjSale.PaidAmount = (inmat.rate * ObjSale.Quantity);
                                ObjSale.PaymentModeId = roysen;


                                ObjSale.InventoryTypeId = 2; // why is inventory InventoryTypeId hard coded
                                
                                ObjSale.isFormalSale = IsFormalInvoice;
                                if (IsFormalInvoice)
                                {
                                    ObjSale.InvoiceId = ObjInvoice.Id;
                                }
                                else
                                {
                                    ObjSale.InvoiceId = ObjInformalInvoices.Id;
                                }


                                // ObjSale.bond = bond;
                                //    ObjSale.rtgs = swipe;
                                //   ObjSale.ecocash = ecocash;

                                db.Sales.Add(ObjSale);
                                db.SaveChanges();
                                
                             
                                Purchase ObjPurchase = new Purchase();

                                //ProductStock begin
                                ProductStock ps = new ProductStock();
                                ps.ProductId = ObjSale.ProductId;
                                ps.Quantity = ObjSale.Quantity;

                                ps.PurchasePrice = selectedProduct.PurchasePrice;

                                ps.TotalPurchaseAmount = (selectedProduct.PurchasePrice * ObjSale.Quantity);

                                ps.SalePrice = inmat.rate;
                                ps.Discount = selectedProduct.Discount;
                                ps.TotalSaleAmount = (inmat.rate * ObjSale.Quantity);

                                decimal TaxAmount = 0;
                                if (selectedTax.Other == "GST")
                                {
                                    decimal taxSplit = selectedTax.TaxRate / 2;
                                    ps.CGST = selectedTax.Id;
                                    ps.CGST_Rate = taxSplit;
                                    ps.SGST = selectedTax.Id;
                                    ps.SGST_Rate = taxSplit;

                                    TaxAmount = ((selectedTax.TaxRate) / (100)) * ps.TotalSaleAmount;
                                }
                                else if (selectedTax.Other == "IGST")
                                {
                                    ps.IGST = selectedTax.Id;
                                    ps.IGST_Rate = selectedTax.TaxRate;

                                    TaxAmount = ((selectedTax.TaxRate) / (100)) * ps.TotalSaleAmount;
                                }
                                else if (selectedTax.Other == "Other")
                                {
                                    ps.TaxId = selectedTax.Id;
                                    ps.OtherTaxValue = selectedTax.TaxRate;
                                    TaxAmount = ((selectedTax.TaxRate) / (100)) * ps.TotalSaleAmount;
                                }


                                ps.TotalSaleAmountWithTax = (inmat.rate * ObjSale.Quantity) + TaxAmount;
                                ps.TaxAmount = TaxAmount;
                                ps.Profit = (ps.TotalSaleAmount - (ps.TotalPurchaseAmount));//+ TaxAmount
                                ps.ProfitWithTax = (ps.TotalSaleAmount - ps.TotalPurchaseAmount);//+ TaxAmount

                                ps.Description = "Invoice Sale";
                                ps.AddedBy = AddedBy;
                                ps.DateAdded = DateTime.Now;
                                ps.ModifiedBy = AddedBy;
                                ps.DateModied = DateTime.Now;
                                ps.InventoryTypeId = 2;
                                ps.WarehouseId = wareId;
                                if (IsFormalInvoice)
                                {
                                    ps.IsFormal = true;
                                }
                                else
                                {
                                    ps.IsFormal = false;
                                }



                                db.ProductStocks.Add(ps);
                                db.SaveChanges(userId);

                                //end

                                try
                                {

                                    InvoiceMaterials mat = new InvoiceMaterials();
                                    mat.ProductId = inmat.ProductId;
                                    mat.description = inmat.description;
                                    mat.quantity = inmat.quantity;
                                    mat.rate = inmat.rate;
                                    mat.vat = inmat.vat;
                                    if (IsFormalInvoice)
                                    {
                                        mat.InvoiceId = ObjInvoice.Id;
                                    }
                                    else
                                    {
                                        mat.InformalInvoiceId = ObjInformalInvoices.Id;
                                    }

                                    db.InvoiceMaterial.Add(mat);
                                    db.SaveChanges(userId);

                                }catch(Exception e)
                                {
                                    return Json(e.Message, JsonRequestBehavior.AllowGet);

                                }




                                //Get Ledger Account
                                int vendorLedger = 0;
                                string CustomerName = db.Users.FirstOrDefault(i => i.Id == customerid).UserName;
                                var LedgerA = db.LedgerAccounts.FirstOrDefault(i => i.Name.Trim() == CustomerName.Trim());
                                if (LedgerA != null)
                                {
                                    vendorLedger = LedgerA.Id;
                                }
                                else
                                {
                                    LedgerAccount la = new LedgerAccount();
                                    la.Name = CustomerName.Trim();
                                    la.ParentId = 12;
                                    la.AddedBy = AddedBy;
                                    la.DateAdded = DateTime.Now;
                                    db.LedgerAccounts.Add(la);
                                    db.SaveChanges();

                                    vendorLedger = la.Id;
                                }
                                //end 

                                //transaction
                                Transaction tr = new Transaction();
                                tr.AddedBy = AddedBy;
                                tr.DebitLedgerAccountId = vendorLedger;
                                tr.DebitAmount = total;
                                //tr.CreditLedgerAccountId = 11;
                                tr.CreditLedgerAccountId = db.LedgerAccounts.FirstOrDefault(i => i.Name == ("Sale")).Id;
                                tr.CreditAmount = payment;
                                tr.DateAdded = DateTime.Now;
                                tr.Remarks = "Sale, Sale Account credit and " + CustomerName + " account debit";
                                tr.Other = null;
                                tr.PurchaseOrSale = "Sale";
                                if (IsFormalInvoice == true)
                                {
                                    tr.IsFormal = true;
                                    tr.PurchaseIdOrSaleId = ObjInvoice.Id;
                                }
                                else
                                {
                                    tr.IsFormal = false;
                                    tr.PurchaseIdOrSaleId = ObjInformalInvoices.Id;
                                }
                                tr.PurchaseIdOrSaleId = ObjInvoice.Id;
                                tr.WarehouseId = wareId;
                                db.Transactions.Add(tr);
                                //end

                                db.SaveChanges(userId);

                                InvoiceItems Iitem = new InvoiceItems();

                                Iitem.ProductId = ObjSale.ProductId;
                                Iitem.Quantity = ObjSale.Quantity;
                                Iitem.TaxAmount = TaxAmount;
                                Iitem.AddedBy = AddedBy;
                                Iitem.DateAdded = DateTime.Now;
                                Iitem.SalePrice = inmat.rate;
                                Iitem.TotalAmount = ps.TotalSaleAmount;
                                Iitem.TotalAmountWithTax = ps.TotalSaleAmountWithTax;
                                Iitem.TaxId = selectedTax.Id;
                                Iitem.PurchaseId = null;
                                Iitem.SaleId = ObjSale.Id;
                                Iitem.ProductStockId = ps.Id;
                                Iitem.TransactionId = tr.Id;
                                Iitem.WarehouseId = wareId;
                                if (IsFormalInvoice)
                                {
                                    Iitem.InvoiceId = ObjInvoice.Id;

                                }
                                else
                                {
                                    Iitem.InformalInvoiceId = ObjInformalInvoices.Id;
                                }
                                db.InvoiceItemss.Add(Iitem);
                                db.SaveChanges();
                                if (orderNo.HasValue && orderNo.Value > 0)
                                {

                                    selectedSaleOrder.approved = true;
                                    selectedSaleOrder.ValidUntil = DateTime.Now;
                                    db.Entry(selectedSaleOrder).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                         

                                var saleUpdate = db.Sales.FirstOrDefault(i => i.Id == ObjSale.Id);
                                saleUpdate.TotalAmountWithTax = ps.TotalSaleAmountWithTax;
                                db.Entry(saleUpdate).State = EntityState.Modified;

                                db.Entry(selectedProduct).State = EntityState.Modified;
                                db.SaveChanges(userId);
                            }
                            if (IsFormalInvoice)
                            {
                                invoiceId = ObjInvoice.Id;
                            }
                            else
                            {
                                invoiceId = ObjInformalInvoices.Id;
                            }
                            if (InvoicePaymentsMethod == 7)
                            {
                                User mybalance = db.Users.FirstOrDefault(n => n.Id == customerid);
                                mybalance.credit = mybalance.credit + balance;
                                db.Entry(mybalance).State = EntityState.Modified;
                                db.SaveChanges(userId);
                            }
                      

                            sb.Append("Submitted");
                            returval = "Submitted";
                            List<SaleReturn> retVal = new List<SaleReturn>();
                            //retVal.Add(new SaleReturn { msg = "Submitted", value = invoiceId });
                            retVal.Add(new SaleReturn { msg = "Submitted", value = invoiceId, isformal = IsFormalInvoice });
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
                }
         }
            return Content(sb.ToString());
        }


   
        public class SaleReturn
        {
            public string msg { get; set; }
            public int value { get; set; }
            public bool isformal { get; set; }

        }
        // GET Invoice/GetGrid
        public ActionResult GetGrid()
        {
            var tak = db.Invoices.Where(i => i.IsPurchaseOrSale == "Purchase").ToArray();

            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.IsBilled),
            Convert.ToString(c.UserId),
            Convert.ToString(c.IsPurchaseOrSale),
            Convert.ToString(c.AddedBy),
            Convert.ToString(c.DateAdded),
            Convert.ToString(c.DateModied),
            Convert.ToString(c.ModifiedBy),
             Convert.ToString(c.WarehouseId),
             };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sale()
        {
            return View();
        }
        public ActionResult DuePayments()
        {
            return View();
        }
        public ActionResult GetDuePayments()
        {
            var tak = db.Invoices.Where(i => i.IsPurchaseOrSale == "Sale" && i.CustomerId > 0 && i.balance > 0 && i.Duedate < DateTime.Now).ToArray();
            var tak2 = db.InformalInvoices.Where(i => i.IsPurchaseOrSale == "Sale" && i.CustomerId > 0 && i.balance > 0 && i.Duedate < DateTime.Now).ToArray();

            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(db.Users.FirstOrDefault(i => i.Id == (c.CustomerId)).UserName),
            Convert.ToString(c.orderNumber),
            Convert.ToString(c.total),
            Convert.ToString(c.payment),
            Convert.ToString(c.balance),
            Convert.ToString(c.DateAdded),
            Convert.ToString(c.Duedate),
             Convert.ToString(db.Warehouses.FirstOrDefault(i => i.Id == (c.WarehouseId)).Name),
             };
            var result2 = from c in tak2
                          select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
           Convert.ToString(db.Users.FirstOrDefault(i => i.Id == (c.CustomerId)).UserName),
            Convert.ToString(c.orderNumber),
            Convert.ToString(c.total),
            Convert.ToString(c.payment),
            Convert.ToString(c.balance),
            Convert.ToString(c.DateAdded),
            Convert.ToString(c.Duedate),
             Convert.ToString(db.Warehouses.FirstOrDefault(i => i.Id == (c.WarehouseId)).Name),
             };

            var newList = result.Concat(result2);
            return Json(new { aaData = newList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetGridSale()
        {
            var tak = db.Invoices.Where(i => i.IsPurchaseOrSale == "Sale" && i.CustomerId > 0).ToArray();
            var tak2 = db.InformalInvoices.Where(i => i.IsPurchaseOrSale == "Sale" && i.CustomerId > 0).ToArray();

            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(db.Users.FirstOrDefault(i => i.Id == (c.CustomerId)).FullName),
             Convert.ToString(c.orderNumber),
            //Convert.ToString(db.Currencies.FirstOrDefault(i => i.Id == c.CurrencyId).Name),
            //Convert.ToString(c.CurrencyId),
          //  Convert.ToString(c.Remarks),
            Convert.ToString(c.total),
            Convert.ToString(c.payment),
            Convert.ToString(c.balance),
            Convert.ToString(c.DateAdded),
            Convert.ToString(c.Duedate),
            Convert.ToString(db.Warehouses.FirstOrDefault(i => i.Id == (c.WarehouseId)).Name),
             };
            var result2 = from c in tak2
                          select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
           Convert.ToString(db.Users.FirstOrDefault(i => i.Id == (c.CustomerId)).FullName),
            Convert.ToString(c.orderNumber),
             //Convert.ToString(db.Currencies.FirstOrDefault(i => i.Id == c.CurrencyId).Name),
         //  Convert.ToString(c.Remarks),
            Convert.ToString(c.total),
            Convert.ToString(c.payment),
            Convert.ToString(c.balance),
            Convert.ToString(c.DateAdded),
            Convert.ToString(c.Duedate),
             Convert.ToString(db.Warehouses.FirstOrDefault(i => i.Id == (c.WarehouseId)).Name),
             };

            var newList = result.Concat(result2);
            return Json(new { aaData = newList }, JsonRequestBehavior.AllowGet);
        }
        // GET: /Invoice/ModelBindIndex
        public ActionResult ModelBindIndex()
        {
            return View();
        }
        // GET: /Invoice/Details/5
        public ActionResult GetPaymentDetails(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice ObjInvoice = db.Invoices.Find(id);
            if (ObjInvoice == null)
            {

                InformalInvoice ObjInvoice2 = db.InformalInvoices.Find(id);
                if (ObjInvoice2 == null) {
                    return HttpNotFound();
                }
                else
                {
                    return Json(new { Id = ObjInvoice2.Id, balance = ObjInvoice2.balance }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Id = ObjInvoice.Id, balance = ObjInvoice.balance }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDetails(int? id)
        {
            List<Invoice> lstInvoice = new List<Invoice>();
            Invoice itemInvoice = new Invoice();
            decimal? TotalBalance = 0;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CustomerBalance = db.Users.FirstOrDefault(n => n.Id == id).credit;
            var ObjInvoice = db.Invoices.Where(i => i.CustomerId == (id) && i.balance > 0).ToArray();
            
            if (ObjInvoice == null)
            {
                var ObjInvoice2 = db.Invoices.Where(i => i.CustomerId == (id)).ToArray();
                if (ObjInvoice2 == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    foreach (var item in ObjInvoice2)
                    {
                        //itemInvoice.Id = item.Id;
                        //itemInvoice.balance=item.balance;
                        //lstInvoice.Add(itemInvoice);
                        TotalBalance += item.balance;


                    }
                    return Json(new { invoiceBalance = CustomerBalance }, JsonRequestBehavior.AllowGet);
                }
            }
            foreach (var item in ObjInvoice)
            {
                //itemInvoice.Id = item.Id;
                //.balance = item.balance;
                //lstInvoice.Add(itemInvoice);
                TotalBalance += item.balance;
            }
            return Json(new { invoiceBalance = CustomerBalance }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice ObjInvoice = db.Invoices.Find(id);
            if (ObjInvoice == null)
            {
                return HttpNotFound();
            }
            return View(ObjInvoice);
        }
        // GET: /Invoice/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: /Invoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Invoice ObjInvoice)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Invoices.Add(ObjInvoice);
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
        // GET: /Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice ObjInvoice = db.Invoices.Find(id);
            if (ObjInvoice == null)
            {
                return HttpNotFound();
            }

            return View(ObjInvoice);
        }

        // POST: /Invoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Invoice ObjInvoice)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjInvoice).State = EntityState.Modified;
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
        // GET: /Invoice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice ObjInvoice = db.Invoices.Find(id);
            if (ObjInvoice == null)
            {
                return HttpNotFound();
            }
            return View(ObjInvoice);
        }

        // POST: /Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {

                Invoice ObjInvoice = db.Invoices.Find(id);
                db.Invoices.Remove(ObjInvoice);
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
        // GET: /Invoice/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            Invoice ObjInvoice = db.Invoices.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;

            }

            return View(ObjInvoice);
        }

        // POST: /Invoice/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(Invoice ObjInvoice)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjInvoice).State = EntityState.Modified;
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

        private SIContext db = new SIContext();
        public ActionResult printformal(int? id)
        {
            try
            {

                if (id == null)
                {
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }


                InvoiceDto inv = new InvoiceDto();
                int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
                var invoice = db.Invoices.FirstOrDefault(i => i.Id == id);

                int? CustomerId;
                string CustomerBranchName;
                string currencySymbol;
                User user = new User();
                //var user = db.Users.FirstOrDefault(i => i.Id == invoice.UserId);

                user = db.Users.FirstOrDefault(i => i.Id == invoice.UserId);
                CustomerId = db.Invoices.FirstOrDefault(i => i.Id == id).CustomerId;

                CustomerBranchName = db.Users.FirstOrDefault(i => i.Id == CustomerId).FullName;
                currencySymbol = db.Currencies.FirstOrDefault(i => i.Id == invoice.CurrencyId).CurrencySymbol;

                var invoiceitem = db.InvoiceItemss.Where(i => i.InvoiceId == id).ToArray();
                var serviceitem = db.InvoiceMaterial.Where(i => i.InvoiceId == id).ToArray();

                var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
                var setting = db.Settings.Where(i => i.sGroup == "Invoice").ToArray();
                var tax = db.Taxs.ToArray();

                inv.InvoiceId = id;

                inv.InvoiceDate = invoice.DateAdded.Value;
                inv.Type = invoice.IsPurchaseOrSale;
                inv.Duedate = invoice.Duedate;
                //inv.PaymentTerms = db.InvoicePaymentMethods.FirstOrDefault(i => i.Id == invoice.InvoicePaymentMethodId).Name;
                db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
                inv.Remarks = invoice.Remarks;


                inv.InvoiceFooterText = invoiceFormat.FooterInfo;

                inv.Logo = Env.GetSiteRoot() + "/Uploads/" + invoiceFormat.Logo;
                inv.CurrencySymbol = setting.FirstOrDefault(i => i.sKey == "CurrencySymbol").sValue;
                inv.ToName = user.FullName;
                inv.ToInfo = user.Address + "<br/> " + user.Mobile + "<br/> " + user.About;
                inv.CustomerAddress = user.Address;
                inv.CompanyAddress = invoiceFormat.AddressInfo;
                inv.WarehouseId = CustomerBranchName;
                inv.CompanyContact = invoiceFormat.OtherInfo;
                inv.CompanyName = invoiceFormat.CompanyName;
                inv.BranchName = CustomerBranchName;
                inv.CurrencySymbol = currencySymbol;
                inv.CompanyvatNo = invoiceFormat.VatNumber;
                inv.vatNo = user.vatNumber;
                inv.Remarks = invoice.Remarks;
             
                inv.DispatchAt = db.Warehouses.FirstOrDefault(i => i.Id == invoice.DispatchAt).Name;
                if (invoice.orderNumber > 0)
                {
                    inv.orderNo = Convert.ToString(db.Quotations.FirstOrDefault(i => i.Id == invoice.orderNumber).Id);
                }


                List<InvoiceItemsDto> listItem = new List<InvoiceItemsDto>();

                foreach (var item in invoiceitem)
                {
                    InvoiceItemsDto li = new InvoiceItemsDto();
                    li.Price = item.SalePrice;
                    li.ProcuctName = item.Product_ProductId.Name;
                    li.Quantity = item.Quantity;
                    li.Tax = item.TaxAmount;
                    li.TotalAmountWithTax = item.TotalAmountWithTax;
                    li.Remarks = item.Remarks;


                    li.TaxInfo = tax.FirstOrDefault(i => i.Id == item.TaxId).Name;

                    li.SubTotal = item.TotalAmount;
                    //inv.TotalAmount = inv.SubTotal + inv.Tax;
                    listItem.Add(li);
                }
                List<servicesDto> serviceItems = new List<servicesDto>();

                foreach (var items in serviceitem)
                {
                    servicesDto lii = new servicesDto();
                    lii.Code = items.Product_ProductId.BarCode;
                    lii.productn = items.Product_ProductId.Name;
                    lii.description = items.description;
                    lii.quantity = items.quantity;
                    lii.rates = items.rate;
                    lii.total = (items.rate * items.quantity);
                    lii.total = decimal.Round(lii.total, 2);
                    lii.vat = items.vat;
                    Convert.ToString(items.Remarks);
                    serviceItems.Add(lii);


                }

                inv.SubTotal = invoice.subtotal;
                inv.Tax = invoice.vat;
                //inv.Tax = invoiceitem.Sum(i => i.TaxAmount);
                //inv.TotalAmount = invoiceitem.Sum(i => i.TotalAmountWithTax);
                inv.TotalAmount = inv.Tax + invoice.subtotal;
                //inv.TotalAmount = invoice.currencytotal;
                inv.payment = invoice.payment;
                inv.balance = invoice.balance;
                inv.Remarks = invoice.Remarks;
                inv.invoiceItem = listItem;
                inv.services = serviceItems;
                inv.TaxInfo = "";

                return View(inv);
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult printInformal(int? id)
        {
            try
            {
                //return HttpNotFound();
                if (id == null)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }
                else
                {
                    InvoiceDto inv = new InvoiceDto();
                    int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
                    /* var invoice = db.Invoices.FirstOrDefault(i => i.Id == id && i.WarehouseId == warehouse*//*);*/
                    var Informalinvoice = db.InformalInvoices.FirstOrDefault(i => i.Id == id);
                    int? CustomerId;
                    string CustomerBranchName;
                    string currencySymbol;
                    User user = new User();

                    user = db.Users.FirstOrDefault(i => i.Id == Informalinvoice.UserId);
                    CustomerId = db.InformalInvoices.FirstOrDefault(i => i.Id == id).CustomerId;
                    CustomerBranchName = db.Users.FirstOrDefault(i => i.Id == CustomerId).FullName;
                    currencySymbol = db.Currencies.FirstOrDefault(i => i.Id == Informalinvoice.CurrencyId).CurrencySymbol;
                    //currencySymbol = db.Currencies.FirstOrDefault(i => i.Id == invoice.CurrencyId).CurrencySymbol;

                    var invoiceitem = db.InvoiceItemss.Where(i => i.InvoiceId == id).ToArray();
                    var serviceitem = db.InvoiceMaterial.Where(i => i.InvoiceId == id).ToArray();

                    var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
                    var setting = db.Settings.Where(i => i.sGroup == "Invoice").ToArray();
                    var tax = db.Taxs.ToArray();

                    inv.InvoiceId = id;

                    inv.InvoiceDate = Informalinvoice.DateAdded.Value;
                    inv.Type = Informalinvoice.IsPurchaseOrSale;
                    inv.Duedate = Informalinvoice.Duedate;
                    inv.PaymentTerms = db.InvoicePaymentMethods.FirstOrDefault(i => i.Id == Informalinvoice.InvoicePaymentMethodId).Name;
                    inv.Remarks = Informalinvoice.Remarks;
                    serviceitem = db.InvoiceMaterial.Where(i => i.InformalInvoiceId == id).ToArray();
                    invoiceitem = db.InvoiceItemss.Where(i => i.InformalInvoiceId == id).ToArray();


                    inv.InvoiceFooterText = invoiceFormat.FooterInfo;

                    inv.Logo = Env.GetSiteRoot() + "/Uploads/" + invoiceFormat.Logo;
                    inv.CurrencySymbol = setting.FirstOrDefault(i => i.sKey == "CurrencySymbol").sValue;
                    inv.ToName = user.FullName;
                    inv.ToInfo = user.Address + user.Mobile;
                    inv.CompanyAddress = invoiceFormat.AddressInfo;
                    inv.WarehouseId = CustomerBranchName;
                    inv.CompanyContact = invoiceFormat.OtherInfo;
                    inv.CompanyName = invoiceFormat.CompanyName;
                    inv.BranchName = CustomerBranchName;
                    inv.CurrencySymbol = currencySymbol;
                    inv.CompanyvatNo = invoiceFormat.VatNumber;
                    inv.vatNo = user.vatNumber;
                   // inv.Remarks = invoice.Remarks;
                    inv.DispatchAt = db.Warehouses.FirstOrDefault(i => i.Id == Informalinvoice.DispatchAt).Name;

                    if (Informalinvoice.orderNumber > 0)
                    {
                        inv.orderNo = Convert.ToString(db.Quotations.FirstOrDefault(i => i.Id == Informalinvoice.orderNumber).Id);
                    }
                    List<InvoiceItemsDto> listItem = new List<InvoiceItemsDto>();

                    foreach (var item in invoiceitem)
                    {
                        InvoiceItemsDto li = new InvoiceItemsDto();
                        li.Price = item.SalePrice;
                        li.ProcuctName = item.Product_ProductId.Name;
                        li.Quantity = item.Quantity;
                        li.Tax = item.TaxAmount;
                        li.TotalAmountWithTax = item.TotalAmountWithTax;
                        li.Remarks = item.Remarks;

                        li.TaxInfo = tax.FirstOrDefault(i => i.Id == item.TaxId).Name;
                        li.SubTotal = item.TotalAmount;
                        li.SubTotal = item.TotalAmount;
                        listItem.Add(li);
                    }
                    List<servicesDto> serviceItems = new List<servicesDto>();

                    foreach (var items in serviceitem)
                    {
                        servicesDto lii = new servicesDto();
                        lii.Code = items.Product_ProductId.BarCode;
                        lii.productn = items.Product_ProductId.Name;
                        lii.description = items.description;
                        lii.quantity = items.quantity;
                        lii.rates = items.rate;
                        lii.total = (items.rate * items.quantity);
                        lii.total = decimal.Round(lii.total, 2);
                        lii.vat = items.vat;
                        Convert.ToString(items.Remarks);
                        serviceItems.Add(lii);


                    }
                    //inv.SubTotal = invoiceitem.Sum(i => i.TotalAmount) + Informalinvoice.subtotal;
                    inv.SubTotal = Informalinvoice.subtotal;
                    inv.Tax = Informalinvoice.vat;
                    //inv.Tax = invoiceitem.Sum(i => i.TaxAmount);
                    //inv.TotalAmount = invoiceitem.Sum(i => i.TotalAmountWithTax);
                    inv.TotalAmount = Informalinvoice.subtotal + Informalinvoice.vat;
                    //inv.TotalAmount = Informalinvoice.currencytotal;
                    inv.payment = Informalinvoice.payment;
                    inv.balance = Informalinvoice.balance;
                    inv.Remarks = Informalinvoice.Remarks;
                    inv.invoiceItem = listItem;
                    inv.services = serviceItems;
                    inv.TaxInfo = "";


                    return View(inv);
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult print(string id)
        {
            int Id = 0;
            string customerName = "";
            bool IsFormalInvoice = true;
            try {
                //return HttpNotFound();
                if (id == null)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }
                else
                {
                    string[] broken_str = id.Split(',');
                    Id = int.Parse(broken_str[0]);
                    customerName = broken_str[1].Trim();
                }
                //var VatNum = db.Users.FirstOrDefault(i => i.FullName == customerName).vatNumber;
                //if (VatNum == null)
                //{
                //    IsFormalInvoice = false;
                //}
                //else
                //{
                //    IsFormalInvoice = true;

                //}
                //bool IsFormalInvoice = true;
                InvoiceDto inv = new InvoiceDto();
                int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
                var invoice = db.Invoices.FirstOrDefault(i => i.Id == Id);
                //var Informalinvoice = db.InformalInvoices.FirstOrDefault(i => i.Id == Id);
                var Informalinvoice = db.InformalInvoices.FirstOrDefault(i => i.Id == Id);
                int? CustomerId;
                string CustomerBranchName;
                string currencySymbol;
                //string Remarks;
                //inv.Remarks = invoice.Remarks;
                User user = new User();
                //var user = db.Users.FirstOrDefault(i => i.Id == invoice.UserId);
                if (invoice != null)
                {
                   

                    user = db.Users.FirstOrDefault(i => i.Id == invoice.UserId);
                    CustomerId = db.Invoices.FirstOrDefault(i => i.Id == Id).CustomerId;
                    CustomerBranchName = db.Users.FirstOrDefault(i => i.Id == CustomerId).FullName;
                    currencySymbol = db.Currencies.FirstOrDefault(i => i.Id == invoice.CurrencyId).CurrencySymbol;
                    inv.DispatchAt = db.Warehouses.FirstOrDefault(i => i.Id == invoice.DispatchAt).Name;

                }
                else
                {
                 user = db.Users.FirstOrDefault(i => i.Id == Informalinvoice.UserId);
                    CustomerId = (int)db.InformalInvoices.FirstOrDefault(i => i.Id == Id).CustomerId;
                    CustomerBranchName = db.Users.FirstOrDefault(i => i.Id == CustomerId).FullName;
                    currencySymbol = db.Currencies.FirstOrDefault(i => i.Id == Informalinvoice.CurrencyId).CurrencySymbol;
                    inv.DispatchAt = db.Warehouses.FirstOrDefault(i => i.Id == Informalinvoice.DispatchAt).Name;
                }

                var invoiceitem = db.InvoiceItemss.Where(i => i.InvoiceId == Id).ToArray();
                var serviceitem = db.InvoiceMaterial.Where(i => i.InvoiceId == Id).ToArray();

                var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
                var setting = db.Settings.Where(i => i.sGroup == "Invoice").ToArray();
                var tax = db.Taxs.ToArray();

                inv.InvoiceId = Id;
                if (invoice == null) {
               
                    inv.InvoiceDate = Informalinvoice.DateAdded.Value;
                    inv.Type = Informalinvoice.IsPurchaseOrSale;
                    inv.Duedate = Informalinvoice.Duedate;
                    //   inv.PaymentTerms = db.InvoicePaymentMethods.FirstOrDefault(i => i.Id == Informalinvoice.InvoicePaymentMethodId).Name;
                    inv.Remarks = Informalinvoice.Remarks;
                    serviceitem = db.InvoiceMaterial.Where(i => i.InformalInvoiceId == Id).ToArray();
                    invoiceitem = db.InvoiceItemss.Where(i => i.InformalInvoiceId == Id).ToArray();
                    //if (invoice.orderNumber > 0) {
                    //    var saleorder = db.SaleOrders.FirstOrDefault(i => i.Id == invoice.orderNumber);
                    //    if (saleorder.CustomerOrderNumber == null || saleorder.CustomerOrderNumber == "") { }
                    //    else {
                    //        inv.orderNo = saleorder.CustomerOrderNumber;
                    //    }
                    //}
                    //inv.orderNo = db.SaleOrders.FirstOrDefault(i => i.Id == invoice.orderNumber).CustomerOrderNumber;
                }
                else
                {
                    inv.InvoiceDate = invoice.DateAdded.Value;
                    inv.Type = invoice.IsPurchaseOrSale;
                    inv.Duedate = invoice.Duedate;
                    // inv.PaymentTerms = db.InvoicePaymentMethods.FirstOrDefault(i => i.Id == invoice.InvoicePaymentMethodId).Name;
                    inv.Remarks = invoice.Remarks;
                    db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
                    //if (invoice.orderNumber > 0)
                    //{
                    //    inv.orderNo = db.SaleOrders.FirstOrDefault(i => i.Id == Informalinvoice.orderNumber).CustomerOrderNumber;
                    //}
                }

                inv.InvoiceFooterText = invoiceFormat.FooterInfo;

                inv.Logo = Env.GetSiteRoot() + "/Uploads/" + invoiceFormat.Logo;
                inv.CurrencySymbol = setting.FirstOrDefault(i => i.sKey == "CurrencySymbol").sValue;
                inv.CurrencySymbol = currencySymbol;
                inv.ToName = user.FullName;
                inv.ToInfo = user.Address +  user.Mobile;
                inv.CustomerAddress = user.Address;
                inv.CompanyAddress = invoiceFormat.AddressInfo;
                inv.WarehouseId = CustomerBranchName;
                inv.CompanyContact = invoiceFormat.OtherInfo;
                inv.CompanyName = invoiceFormat.CompanyName;
                inv.BranchName = CustomerBranchName;
                inv.CompanyvatNo = invoiceFormat.VatNumber;
                inv.BP = invoiceFormat.BPNumber;
                inv.vatNo = user.vatNumber;
                //inv.Remarks = invoice.Remarks;
                //inv.Remarks = Informalinvoice.Remarks;


                List<InvoiceItemsDto> listItem = new List<InvoiceItemsDto>();

                foreach (var item in invoiceitem)
                {
                    InvoiceItemsDto li = new InvoiceItemsDto();
                    li.Price = item.SalePrice;
                    li.ProcuctName = item.Product_ProductId.Name;
                    li.Quantity = item.Quantity;
                    li.Tax = item.TaxAmount;
                    li.TotalAmountWithTax = item.TotalAmountWithTax;
                    //li.Remarks = item.Remarks;


                    li.TaxInfo = tax.FirstOrDefault(i => i.Id == item.TaxId).Name;

                    li.SubTotal = item.TotalAmount;
                    listItem.Add(li);
                }
                List<servicesDto> serviceItems = new List<servicesDto>();

                foreach (var items in serviceitem)
                {
                    servicesDto lii = new servicesDto();
                    lii.Code = items.Product_ProductId.BarCode;
                    lii.productn = items.Product_ProductId.Name;
                    lii.description = items.description;
                    lii.quantity = items.quantity;
                    lii.rates = items.rate;
                    lii.total = (items.rate * items.quantity);
                    lii.total = decimal.Round(lii.total, 2);
                    lii.vat = items.vat;
                    // lii.Remarks = items.Remarks;
                    Convert.ToString(items.Remarks);
                    serviceItems.Add(lii);


                }
                if (invoice == null)
                {
                   

                    //inv.SubTotal = invoiceitem.Sum(i => i.TotalAmount) + Informalinvoice.subtotal;
                    inv.SubTotal = Informalinvoice.subtotal;
                    inv.Tax = Informalinvoice.vat;
                    inv.TotalAmount = Informalinvoice.currencytotal;
                    //inv.Tax = invoiceitem.Sum(i => i.TaxAmount);
                    inv.TotalAmount = invoiceitem.Sum(i => i.TotalAmountWithTax);
                    inv.TotalAmount = Informalinvoice.subtotal + Informalinvoice.vat;
                    inv.payment = Informalinvoice.payment;
                    inv.balance = Informalinvoice.balance;
                    inv.Remarks = Informalinvoice.Remarks;
                }
                else
                {
                    inv.SubTotal = invoice.subtotal;
                    inv.Tax = invoice.vat;
                    //inv.Tax = invoiceitem.Sum(i => i.TaxAmount);
                    inv.TotalAmount = invoiceitem.Sum(i => i.TotalAmountWithTax);
                    inv.TotalAmount = invoice.currencytotal;
                    inv.TotalAmount = invoice.vat + invoice.subtotal;
                    inv.payment = invoice.payment;
                    inv.balance = invoice.balance;
                    //inv.Remarks = invoice.Remarks;  
                }
                inv.invoiceItem = listItem;
                inv.services = serviceItems;
                inv.TaxInfo = "";

                return View(inv);
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                return HttpNotFound(ex.Message);
            }
        }

        public JsonResult CashToCurrencyConvertor(int selectedCurrency, decimal subtotal, decimal vattotal  )
        {
            try
            {
                //Convert.ToString(db.Users.FirstOrDefault(i => i.Id == Int32.Parse(c.UserName)).UserName),
                List<double> convertedAmounts = new List<double>();
                //var CurrencyId = db.Currencies.FirstOrDefault(x => x.Name.Equals(selectedCurrency)).Id;
                var CurrencyId = selectedCurrency;
                double currencyRate = db.Rates
                               .Where(x => x.CurrencyId == CurrencyId)
                               .OrderByDescending(x => x.DateModified)
                               .First().CurrencyRate;
                // decimal currencyRate = db.Rates.LastOrDefault(rate => rate.Currency.Name == (selectedCurrecy)).CurrencyRate;
                double convertedAmount = (double)subtotal / currencyRate;
                convertedAmounts.Add(convertedAmount);
                double vattotalconverted = (double)vattotal / currencyRate;
                //double discountConverted = (double)discount / currencyRate;
                convertedAmounts.Add(vattotalconverted);
                //return Json(convertedAmounts, JsonRequestBehavior.AllowGet);

                List<convertedAmounts> retVal = new List<convertedAmounts>();
              
                retVal.Add(new convertedAmounts { subtotal = convertedAmount, totalvat  = vattotalconverted /*discount = discountConverted*/ });
                //return Json(returval, JsonRequestBehavior.AllowGet);
                return Json(retVal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult CurrencyToCashConvertor(int selectedCurrency, decimal subtotal, decimal vattotal)
        {

            double convertedsubtotal = 0;
            double convertedvattotal = 0;
            List<double> convertedAmounts = new List<double>();
            //currencyAmounts.ForEach(CurrencyAmount =>
            //{
            if (!subtotal.Equals(0))
                {
                var CurrencyId = selectedCurrency;

                double currencyRate = db.Rates
                              .Where(x => x.CurrencyId == CurrencyId)
                              .OrderByDescending(x => x.DateModified)
                              .First().CurrencyRate;
                convertedsubtotal = (double)subtotal * currencyRate;
                convertedvattotal = (double)vattotal * currencyRate;
                convertedAmounts.Add(convertedsubtotal);
                convertedAmounts.Add(convertedvattotal);
            }

            List<convertedAmounts> retVal = new List<convertedAmounts>();

            retVal.Add(new convertedAmounts { subtotal = convertedsubtotal, totalvat = convertedvattotal });
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }
        public class convertedAmounts
        {
            public double subtotal { get; set; }
            public double totalvat { get; set; }
            //public double discount { get; set; }
           
        }
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

