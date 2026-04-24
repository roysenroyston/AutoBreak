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
using WebErrorLogging.Utilities;

namespace ShopMate.Controllers
{
    public class ExpenseController : BaseController
    {
        string userId = Env.GetUserInfo("name");
        int Warehouses = int.Parse(Env.GetUserInfo("WarehouseId"));
        // GET: /Expense/
        public ActionResult Index()
        {
            return View();
        }

        // GET Expense/GetGrid
        public ActionResult GetGrid()
        {
            var tak = db.Expenses.ToArray();

            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),                        
                         Convert.ToString(db.LedgerAccounts.FirstOrDefault(i => i.Id == c.ExpenseId).Name),
                         //Convert.ToString(db.Users.FirstOrDefault(i => i.Id == c.User_VendorUserId).FullName),
                        //Convert.ToString(c.User_VendorUserId.FullName),
                          //Convert.ToString(c.Vendorname),
           
             Convert.ToString(db.Users.FirstOrDefault(n => n.Id ==c.Vendorname).FullName),
            Convert.ToString(c.Remarks),
            Convert.ToString(c.VatNumber),
            Convert.ToString(c.InvoiceNumber),
           
             Convert.ToString(c.InvoiceDate),
            Convert.ToString(c.SubTotal),
            Convert.ToString(c.TaxAmount),
              Convert.ToString(c.Amount),
            Convert.ToString(db.Users.FirstOrDefault(i => i.Id == c.AddedBy).UserName), 
            Convert.ToString(c.DateAdded), 
            Convert.ToString(c.WarehouseId), 
             };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        // GET: /Expense/ModelBindIndex
        public ActionResult ModelBindIndex()
        {
            return View();
        }
        // GET: /Expense/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense ObjExpense = db.Expenses.Find(id);
            if (ObjExpense == null)
            {
                return HttpNotFound();
            }
            return View(ObjExpense);
        }
        // GET: /Expense/Create
        public ActionResult Create()
        {

            ViewBag.VendorUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Supplier" && i.WarehouseId == Warehouses), "Id", "FullName");
            ViewBag.ExpenseLedgerId = new SelectList(db.LedgerAccounts.Where(i => i.ParentId == 2), "Id", "Name");
            return View();
        }

        // POST: /Expense/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Expense ObjExpense, int VendorUserId, int? ExpenseLedgerId)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                string VendorName = db.Users.FirstOrDefault(i => i.Id == VendorUserId).FullName;
                if (ModelState.IsValid)
                {
                    if (ExpenseLedgerId == null)
                    {
                        sb.Append("Error : Select Expense For First");
                        return Content(sb.ToString());
                    }
                    int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
                

                    Expense OBJExpenses = new Expense();
                    OBJExpenses.AddedBy = ObjExpense.AddedBy;
                    OBJExpenses.Amount = ObjExpense.Amount;
                    OBJExpenses.SubTotal = ObjExpense.SubTotal;
                    OBJExpenses.DateAdded = ObjExpense.DateAdded;
                    OBJExpenses.Id = ObjExpense.Id;
                    OBJExpenses.Remarks = ObjExpense.Remarks;
                    OBJExpenses.ExpenseId = ExpenseLedgerId;
                    OBJExpenses.WarehouseId = warehouse;
                    OBJExpenses.VatNumber = ObjExpense.VatNumber;
                    OBJExpenses.InvoiceNumber = ObjExpense.InvoiceNumber;
                    OBJExpenses.InvoiceDate = ObjExpense.InvoiceDate;
                    OBJExpenses.TaxAmount = ObjExpense.TaxAmount;
                    OBJExpenses.Vendorname = VendorUserId;
                    db.Expenses.Add(OBJExpenses);
                    db.SaveChanges(userId);
                    
                    //transaction
                    Transaction tr = new Transaction();
                    tr.AddedBy = ObjExpense.AddedBy;
                    tr.DebitLedgerAccountId = ExpenseLedgerId;
                    tr.DebitAmount = ObjExpense.Amount;
                    tr.CreditLedgerAccountId = 14;
                    tr.CreditAmount = ObjExpense.Amount;
                    tr.DateAdded = DateTime.Now;
                    tr.Remarks = "Expense, Expense Account debit and Cash account credit";
                    tr.Other = null;
                    tr.PurchaseOrSale = "Expense";
                    tr.PurchaseIdOrSaleId = ObjExpense.Id;
                    tr.WarehouseId = warehouse;

                    db.Transactions.Add(tr);
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
        // GET: /Expense/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense ObjExpense = db.Expenses.Find(id);
            if (ObjExpense == null)
            {
                return HttpNotFound();
            }

            return View(ObjExpense);
        }

        // POST: /Expense/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Expense ObjExpense)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjExpense).State = EntityState.Modified;
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
        // GET: /Expense/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense ObjExpense = db.Expenses.Find(id);
            if (ObjExpense == null)
            {
                return HttpNotFound();
            }
            return View(ObjExpense);
        }

        // POST: /Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {

                Expense ObjExpense = db.Expenses.Find(id);
                try
                {
                    Transaction ObjTransaction = db.Transactions.FirstOrDefault(i => i.PurchaseOrSale == "Expense" && i.PurchaseIdOrSaleId == id);
                    db.Transactions.Remove(ObjTransaction);
                }
                catch (Exception ex)
                {
                    Helper.WriteError(ex, ex.Message);
                }
                

                db.Expenses.Remove(ObjExpense);
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
        // GET: /Expense/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            Expense ObjExpense = db.Expenses.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;

            }

            return View(ObjExpense);
        }

        // POST: /Expense/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(Expense ObjExpense)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjExpense).State = EntityState.Modified;
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

