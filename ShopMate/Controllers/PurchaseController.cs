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
using System.Data.SqlClient;
using WebErrorLogging.Utilities;
using System.Security.Claims;
using System.Threading;

namespace ShopMate.Controllers
{
    public class PurchaseController : BaseController
    {
        private string userId = Env.GetUserInfo("name");

        // GET: /Purchase/
        public ActionResult Index()
        {
            return View();
        }

        // GET Purchase/GetGrid
        public ActionResult GetGrid()
        {
            int warehouses = int.Parse(Env.GetUserInfo("WarehouseId"));
            var warehouse = db.Warehouses.ToArray();
            var user = db.Users.ToArray();
            var tak = db.Purchases.Where(i => i.WarehouseId == warehouses).ToArray();
            var userwarehouse = db.Users.FirstOrDefault(n => n.UserName == userId).WarehouseId;

            {
                var result = from c in tak.Where(i => i.WarehouseId == userwarehouse)
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.User_VendorUserId.UserName),
            Convert.ToString(c.Product_ProductId.Name),
            Convert.ToString(c.Quantity),
            Convert.ToString(c.ReturnedQuantity),
            Convert.ToString(c.UnitPrice),
            Convert.ToString(c.TotalAmount),
            Convert.ToString(c.DateAdded),
           Convert.ToString(user.FirstOrDefault(i=>i.Id==c.AddedBy).UserName),
            Convert.ToString(warehouse.FirstOrDefault(i=>i.Id==c.WarehouseId).Name),
            Convert.ToString(c.InventoryTypeId),
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: /Purchase/ModelBindIndex
        public ActionResult ModelBindIndex()
        {
            return View();
        }

        // GET: /Purchase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase ObjPurchase = db.Purchases.Find(id);
            if (ObjPurchase == null)
            {
                return HttpNotFound();
            }
            return View(ObjPurchase);
        }

        // GET: /Purchase/Create

        public ActionResult Create()
        {
            var userwarehouse = db.Users.FirstOrDefault(n => n.UserName == userId).WarehouseId;

            {
                ViewBag.VendorUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Supplier" && i.WarehouseId == userwarehouse), "Id", "FullName");
                ViewBag.ProductId = new SelectList(db.Products.Where(i => i.WarehouseId == userwarehouse && i.IsActive == true), "Id", "Name");
                ViewBag.WarehouseId = new SelectList(db.Warehouses.Where(i => i.Id == userwarehouse), "Id", "Name");
            }
            //var tax = db.Taxs.ToArray();
            //ViewBag.TaxId = new SelectList(tax, "Id", "Name");

            return View();
        }

        // POST: /Purchase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Purchase ObjPurchase, int warehouse, int VendorUserId, InvoiceItems[] productss, string Description = "")
        {
            string result = "Error! Purchase  Is Not Complete!";
            //Get the current claims principal
            // var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            //int warehouse = Int16.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.Actor)
            //               .Select(c => c.Value).SingleOrDefault());
            try
            {
                string VendorName = db.Users.FirstOrDefault(i => i.Id == VendorUserId).FullName;
                int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
                // int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
                foreach (var item in productss)
                {
                    var selectedProduct1 = db.Products.FirstOrDefault(i => i.Id == item.ProductId);
                    var myproductname = db.Products.FirstOrDefault(n => n.Id == item.ProductId).Name;
                    var ObjWarehouseStock1 = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == item.ProductId && i.WarehouseId == warehouse);
                    if (ObjWarehouseStock1 == null)
                    {
                        result = myproductname + " Is not from the selected warehouse ";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                try
                {
                    Invoice inv = new Invoice();
                    inv.AddedBy = AddedBy;
                    inv.DateAdded = DateTime.Now;
                    inv.DateModied = DateTime.Now;
                    inv.IsBilled = false;
                    inv.IsPurchaseOrSale = "Purchase";
                    inv.ModifiedBy = AddedBy;
                    inv.UserId = VendorUserId;
                    inv.WarehouseId = warehouse;
                    db.Invoices.Add(inv);

                    db.SaveChanges(userId);

                    foreach (var item in productss)
                    {
                        var selectedProduct = db.Products.FirstOrDefault(i => i.Id == item.ProductId);
                        var ObjWarehouseStock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == item.ProductId && i.WarehouseId == warehouse);

                        var selectedTax = db.Taxs.FirstOrDefault(i => i.Id == selectedProduct.TaxId);
                        // if (TaxName.Contains("IGST") || TaxName.Contains("Other"))
                        // {
                        //   selectedTax = db.Taxs.FirstOrDefault(i => i.Name == TaxName.Trim());
                        // }

                        //   Purchase ObjPurchase = new Models.Purchase();

                        ObjPurchase.ProductId = item.ProductId;
                        ObjPurchase.Quantity = item.Quantity;
                        // ObjPurchase.UnitPrice = selectedProduct.PurchasePrice;
                        ObjPurchase.UnitPrice = item.SalePrice;
                        ObjPurchase.TotalAmount = (item.SalePrice * item.Quantity);
                        ObjPurchase.WarehouseId = warehouse;
                        ObjPurchase.AddedBy = AddedBy;
                        ObjPurchase.TotalAmountWithTax = (selectedTax.TaxRate * item.SalePrice) * item.Quantity;
                        ObjPurchase.VendorUserId = VendorUserId;
                        ObjPurchase.DateAdded = DateTime.Now;
                        ObjPurchase.InventoryTypeId = 1;

                        db.Purchases.Add(ObjPurchase);
                        db.SaveChanges(userId);

                        //product begin here
                        string constring = System.Configuration.ConfigurationManager.ConnectionStrings["SIConnectionString"]
                   .ConnectionString;

                        string qury = "UPDATE Product SET PurchasePrice='" + item.SalePrice + "' WHERE 'Id'='" + selectedProduct.Id + "'";
                        using (SqlConnection con = new SqlConnection(constring))
                        {
                            using (SqlCommand cmd = new SqlCommand(qury, con))
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }

                        //Product pr = new Product();
                        //pr.Id = selectedProduct.Id;
                        //pr.PurchasePrice = item.SalePrice;
                        //db.Entry(pr).State = EntityState.Modified;
                        //db.SaveChanges();

                        ProductStock ps = new ProductStock();
                        ps.ProductId = ObjPurchase.ProductId;
                        ps.Quantity = item.Quantity;
                        ps.PurchasePrice = item.SalePrice;

                        ps.TotalPurchaseAmount = (item.SalePrice * item.Quantity);

                        ps.SalePrice = selectedProduct.SalePrice;

                        ps.Discount = selectedProduct.Discount;

                        decimal TaxAmount = 0;
                        decimal vatonreturn = 0;
                        if (selectedTax.Other == "GST")
                        {
                            decimal taxSplit = selectedTax.TaxRate / 2;
                            ps.CGST = selectedProduct.TaxId;
                            ps.CGST_Rate = taxSplit;
                            ps.SGST = selectedProduct.TaxId;
                            ps.SGST_Rate = taxSplit;

                            TaxAmount = ((selectedTax.TaxRate) / (115)) * ps.TotalPurchaseAmount;
                            vatonreturn = ((selectedTax.TaxRate) / (100)) * ps.TotalSaleAmount;
                            //  TaxAmount = (15 /115) * ps.TotalPurchaseAmount;
                        }
                        else if (selectedTax.Other == "IGST")
                        {
                            ps.IGST = selectedProduct.TaxId;
                            ps.IGST_Rate = selectedTax.TaxRate;
                            //TaxAmount = (15 / 115) * ps.TotalPurchaseAmount;
                            TaxAmount = ((selectedTax.TaxRate) / (115)) * ps.TotalPurchaseAmount;
                            vatonreturn = ((selectedTax.TaxRate) / (100)) * ps.TotalSaleAmount;
                        }
                        else if (selectedTax.Other == "Other")
                        {
                            ps.TaxId = selectedProduct.TaxId;
                            ps.OtherTaxValue = selectedTax.TaxRate;
                            //TaxAmount = (15 / 115) * ps.TotalPurchaseAmount;
                            TaxAmount = ((selectedTax.TaxRate) / (115)) * ps.TotalPurchaseAmount;
                            vatonreturn = ((selectedTax.TaxRate) / (100)) * ps.TotalSaleAmount;
                        }

                        ps.TotalSaleAmount = (item.SalePrice * item.Quantity) - vatonreturn;
                        ps.TotalSaleAmountWithTax = (selectedProduct.SalePrice * item.Quantity);//+ TaxAmount
                        ps.TaxAmount = TaxAmount;
                        ps.Profit = (ps.TotalSaleAmount - ps.TotalPurchaseAmount) - vatonreturn;
                        ps.ProfitWithTax = (ps.TotalSaleAmountWithTax - ps.TotalPurchaseAmount);//+ TaxAmount

                        ps.Description = "Purchase";
                        ps.AddedBy = ObjPurchase.AddedBy;
                        ps.DateAdded = DateTime.Now;
                        ps.ModifiedBy = ObjPurchase.AddedBy;
                        ps.DateModied = DateTime.Now;
                        ps.InventoryTypeId = 1;
                        ps.WarehouseId = warehouse;
                        db.ProductStocks.Add(ps);
                        db.SaveChanges(userId);

                        //end

                        //Get Ledger Account
                        int vendorLedger = 0;

                        var LedgerA = db.LedgerAccounts.FirstOrDefault(i => i.Name.Trim() == VendorName.Trim());
                        if (LedgerA != null)
                        {
                            vendorLedger = LedgerA.Id;
                        }
                        else
                        {
                            LedgerAccount la = new LedgerAccount();
                            la.Name = VendorName.Trim();
                            la.ParentId = 12;
                            la.AddedBy = AddedBy;
                            la.DateAdded = DateTime.Now;
                            db.LedgerAccounts.Add(la);
                            db.SaveChanges(userId);

                            vendorLedger = la.Id;
                        }
                        //end

                        //transaction
                        Transaction tr = new Transaction();
                        //tr.AddedBy = ObjPurchase.AddedBy;
                        ////tr.DebitLedgerAccountId = 12;//Purchase ledger account
                        //tr.DebitLedgerAccountId = db.LedgerAccounts.FirstOrDefault(i => i.Name == (VendorName.Trim())).Id;
                        //tr.DebitAmount = (ps.TotalPurchaseAmount);//+ TaxAmount
                        //tr.CreditLedgerAccountId = vendorLedger;
                        //tr.CreditAmount = (ps.TotalPurchaseAmount);//+ TaxAmount
                        //tr.DateAdded = DateTime.Now;
                        //tr.Remarks = "Purchase, Purchase Account debit and " + VendorName + " account credit";
                        //tr.Other = null;
                        //tr.PurchaseOrSale = "Purchase";
                        //tr.PurchaseIdOrSaleId = ObjPurchase.Id;
                        //tr.WarehouseId = warehouse;
                        //tr.IsFormal= true;
                        //db.Transactions.Add(tr);

                        tr.AddedBy = ObjPurchase.AddedBy;
                        //tr.DebitLedgerAccountId = 12;//Purchase ledger account
                        tr.DebitLedgerAccountId = vendorLedger;
                        tr.DebitAmount = (ps.TotalPurchaseAmount);//+ TaxAmount
                        tr.CreditLedgerAccountId = 13;
                        tr.CreditAmount = (ps.TotalPurchaseAmount);//+ TaxAmount
                        tr.DateAdded = DateTime.Now;
                        tr.Remarks = "Purchase, Purchase Account debit and " + VendorName + " account credit";
                        tr.Other = null;
                        tr.PurchaseOrSale = "Purchase";
                        tr.PurchaseIdOrSaleId = ObjPurchase.Id;
                        tr.WarehouseId = warehouse;
                        tr.IsFormal = true;
                        db.Transactions.Add(tr);

                        //end

                        db.SaveChanges(userId);

                        InvoiceItems Iitem = new InvoiceItems();

                        Iitem.ProductId = item.ProductId;
                        Iitem.Quantity = item.Quantity;
                        Iitem.TaxAmount = TaxAmount;
                        Iitem.AddedBy = ObjPurchase.AddedBy;
                        Iitem.DateAdded = DateTime.Now;
                        // Iitem.SalePrice = selectedProduct.PurchasePrice;
                        Iitem.SalePrice = item.SalePrice;
                        Iitem.TotalAmount = ps.TotalPurchaseAmount - TaxAmount;
                        Iitem.TotalAmountWithTax = ps.TotalPurchaseAmount;//+ TaxAmount
                        Iitem.TaxId = selectedTax.Id;
                        Iitem.PurchaseId = ObjPurchase.Id;
                        Iitem.SaleId = null;
                        Iitem.ProductStockId = ps.Id;
                        Iitem.TransactionId = tr.Id;
                        Iitem.WarehouseId = warehouse;

                        Iitem.InvoiceId = inv.Id;
                        db.InvoiceItemss.Add(Iitem);

                        db.SaveChanges(userId);

                        if (selectedProduct.RemainingQuantity == 0.00m && selectedProduct.RemainingAmount == 0.00m)
                        {
                            //selectedProduct.RemainingQuantity = ObjPurchase.Quantity;
                            selectedProduct.RemainingAmount = ps.TotalSaleAmountWithTax;
                        }
                        else
                        {
                            //selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity + ObjPurchase.Quantity;
                            selectedProduct.RemainingAmount = selectedProduct.RemainingAmount + ps.TotalSaleAmountWithTax;
                        }
                        selectedProduct.PurchasePrice = ps.PurchasePrice;
                        db.Entry(selectedProduct).State = EntityState.Modified;
                        ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity + ObjPurchase.Quantity;
                        //     ObjPurchase.TotalAmountWithTax = ps.TotalPurchaseAmount;//+ TaxAmount
                        db.Entry(ObjPurchase).State = EntityState.Modified;
                        db.SaveChanges(userId);

                        ProductStock ngonie = db.ProductStocks.FirstOrDefault(k => k.Id == ps.Id);
                        ngonie.RemainingQuantity = ObjWarehouseStock.RemainingQuantity;
                        db.Entry(ngonie).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    result = "Success! Purchase Completed";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Helper.WriteError(ex, ex.Message);
                    // retVal.Add(new SaleReturn { msg = "error:" + ex.Message, value = 0 });
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                /// retVal.Add(new SaleReturn { msg = "error:" + ex.Message, value = 0 });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase ObjPurchase = db.Purchases.Find(id);
            if (ObjPurchase == null)
            {
                return HttpNotFound();
            }
            var userware = db.Users.FirstOrDefault(i => i.UserName == userId).WarehouseId;
            if (userId == "'Zimhope")
            {
                ViewBag.VendorUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Vendor"), "Id", "UserName", ObjPurchase.VendorUserId);
                ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", ObjPurchase.ProductId);
            }
            else
            {
                ViewBag.VendorUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Vendor" && i.WarehouseId == userware), "Id", "UserName", ObjPurchase.VendorUserId);
                ViewBag.ProductId = new SelectList(db.Products.Where(i => i.WarehouseId == userware), "Id", "Name", ObjPurchase.ProductId);
            }

            return View(ObjPurchase);
        }

        // POST: /Purchase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Purchase ObjPurchase)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(ObjPurchase).State = EntityState.Modified;
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

        // GET: /Purchase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase ObjPurchase = db.Purchases.Find(id);
            if (ObjPurchase == null)
            {
                return HttpNotFound();
            }
            return View(ObjPurchase);
        }

        // POST: /Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                Purchase ObjPurchase = db.Purchases.Find(id);

                InvoiceItems ObjInvoiceItems = db.InvoiceItemss.FirstOrDefault(i => i.PurchaseId == ObjPurchase.Id);

                Invoice ObjInvoice = db.Invoices.FirstOrDefault(i => i.Id == ObjInvoiceItems.InvoiceId);

                ProductStock ObjProductStock = db.ProductStocks.FirstOrDefault(i => i.Id == ObjInvoiceItems.ProductStockId);

                Transaction ObjTransaction = db.Transactions.FirstOrDefault(i => i.Id == ObjInvoiceItems.TransactionId);

                if (ObjPurchase.InventoryTypeId == 1)
                {
                    var selectedProduct = db.Products.FirstOrDefault(i => i.Id == ObjPurchase.ProductId);

                    selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity - ObjPurchase.Quantity;
                    selectedProduct.RemainingAmount = selectedProduct.RemainingAmount - (ObjProductStock.TotalSaleAmountWithTax);

                    db.Entry(selectedProduct).State = EntityState.Modified;
                    db.SaveChanges(userId);
                }

                db.ProductStocks.Remove(ObjProductStock);

                db.Transactions.Remove(ObjTransaction);

                db.Purchases.Remove(ObjPurchase);

                db.InvoiceItemss.Remove(ObjInvoiceItems);

                db.Invoices.Remove(ObjInvoice);

                db.SaveChanges(userId);

                try
                {
                    //if double antry of purchase or purchase retrun in transaction
                    Transaction ObjTran2 = db.Transactions.FirstOrDefault(i => i.PurchaseOrSale == "Purchase" && i.PurchaseIdOrSaleId == id);
                    db.Transactions.Remove(ObjTran2);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }

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

        // GET: /Purchase/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            Purchase ObjPurchase = db.Purchases.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;
                ViewBag.VendorUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Vendor"), "Id", "UserName", ObjPurchase.VendorUserId);
                ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", ObjPurchase.ProductId);
            }

            return View(ObjPurchase);
        }

        // POST: /Purchase/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(Purchase ObjPurchase)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(ObjPurchase).State = EntityState.Modified;
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

        [HttpGet]
        public ActionResult PurchaseReturn(int id)
        {
            try
            {
                ViewBag.qty = Request.QueryString["qty"].ToString();
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
            }

            ViewBag.purchaseid = id;
            return View();
        }

        [HttpPost]
        public ActionResult PurchaseReturn(decimal Quantity, int? id)
        {
            //algorithem
            ///if full quantity return than
            ///Purchase (Just change inventorytypeid)
            ///ProductStock (Just change inventorytypeid)
            ///[Transaction] (make reverse new entry) ::
            ///
            ///if less quantiry retun than
            ///Purchase (update qty and amounts as per purchase to same entry)
            ///ProductStock (update qty and amounts as per purchase to same entry)
            ///[Transaction] (make reverse new entry with full remarks how much buy and how much return) ::
            ///InvoiceItem (update qty and amounts as per purchase to same entry)
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                Purchase ObjPurchase = db.Purchases.Find(id);
                if (ObjPurchase.InventoryTypeId == 1)
                {
                    InvoiceItems ObjInvoiceItems = db.InvoiceItemss.FirstOrDefault(i => i.PurchaseId == ObjPurchase.Id);
                    ProductStock ObjProductStock = db.ProductStocks.FirstOrDefault(i => i.Id == ObjInvoiceItems.ProductStockId);
                    Transaction ObjTransaction = db.Transactions.FirstOrDefault(i => i.Id == ObjInvoiceItems.TransactionId);
                    var selectedProduct = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == ObjPurchase.ProductId && i.WarehouseId == ObjPurchase.WarehouseId);
                    var selectedTax = db.Taxs.FirstOrDefault(i => i.Id == selectedProduct.Product_ProductId.TaxId);

                    if (ObjPurchase.Quantity >= Quantity)
                    {
                        if (selectedProduct.RemainingQuantity < Quantity)
                        {
                            sb.Append("Not Allowed! Remaining quantity is: " + selectedProduct.RemainingQuantity);
                            return Content(sb.ToString());
                        }
                        selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity - Quantity;
                        db.Entry(selectedProduct).State = EntityState.Modified;
                        db.SaveChanges(userId);

                        ObjPurchase.ReturnedQuantity = ObjPurchase.ReturnedQuantity + Quantity;
                        ObjPurchase.Quantity = ObjPurchase.Quantity - Quantity;
                        ObjPurchase.TotalAmount = ObjPurchase.TotalAmount - (ObjPurchase.UnitPrice * Quantity);
                        db.Entry(ObjPurchase).State = EntityState.Modified;
                        db.SaveChanges(userId);

                        ProductStock ps = new ProductStock();
                        ps.ProductId = ObjPurchase.ProductId;
                        ps.Quantity = Quantity;
                        ps.PurchasePrice = ObjPurchase.UnitPrice;
                        ps.TotalPurchaseAmount = (ObjPurchase.UnitPrice * Quantity);
                        ps.SalePrice = selectedProduct.Product_ProductId.SalePrice;
                        ps.Discount = 0;
                        decimal TaxAmount = 0;
                        decimal vatonreturn = 0;
                        ps.TotalSaleAmount = (ps.SalePrice * ps.Quantity);
                        ps.TotalSaleAmountWithTax = (ps.TotalSaleAmount);//+ TaxAmount
                        ps.TaxAmount = TaxAmount;
                        ps.Profit = 0;
                        ps.ProfitWithTax = (ps.TotalSaleAmountWithTax - ps.TotalPurchaseAmount);//+ TaxAmount
                        ps.RemainingQuantity = selectedProduct.RemainingQuantity;
                        ps.Description = "Purchase Return";
                        ps.AddedBy = ObjPurchase.AddedBy;
                        ps.DateAdded = DateTime.Now;
                        ps.ModifiedBy = ObjPurchase.AddedBy;
                        ps.DateModied = DateTime.Now;
                        ps.InventoryTypeId = 1013;
                        ps.WarehouseId = ObjPurchase.WarehouseId;
                        ps.ReturnedQuantity = Quantity;
                        db.ProductStocks.Add(ps);
                        db.SaveChanges(userId);

                        sb.Append("Sumitted");
                        return Content(sb.ToString());
                    }
                    else
                    {
                        sb.Append("Error : your product stock remaining quantity is (" + ObjPurchase.Quantity + ") low than your given return quantity.");
                        return Content(sb.ToString());
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