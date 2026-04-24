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
    public class DeclaredayEndController : Controller
    {
        string userId = Env.GetUserInfo("name");
        string warehouseId = Env.GetUserInfo("WarehouseId");
        // GET: DeclaredayEnd
        public ActionResult Index()
        {
            return View();
        }
        private SIContext db = new SIContext();
        public ActionResult GetGrid()
        {
            var tak = db.DayEnds.ToArray();
            var user = db.Users.ToArray();
            var userWarehouse = db.Users.FirstOrDefault(i => i.UserName == userId).WarehouseId;
            if (userId == "Zimhope")
            {
                var result = from c in tak
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            //Convert.ToString(c.DayEnddeclare_Ids),
            //Convert.ToString(c.UserId),
            Convert.ToString(c.totalcash),
            Convert.ToString(c.totalCashUsd),
            Convert.ToString(c.ecocash),
            //Convert.ToString(c.telecash),
            //Convert.ToString(c.onemoney),
            Convert.ToString(c.rtgs),
            //Convert.ToString(c.nostro),
            //Convert.ToString(c.totalCashUsd),
            Convert.ToString(c.totalAmount),
            Convert.ToString(c.totalChange),
             Convert.ToString(c.AddedBy),
           // Convert.ToString(user.FirstOrDefault(i=>i.Id==c.AddedBy).UserName),
            Convert.ToString(c.DateAdded),
            //Convert.ToString(c.DateModied),
            //Convert.ToString(c.ModifiedBy),
            Convert.ToString(c.Declared),
            Convert.ToString(c.WarehouseId),
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = from c in tak.Where(n => n.WarehouseId == userWarehouse)
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            //Convert.ToString(c.DayEnddeclare_Ids),
            //Convert.ToString(c.UserId),
            Convert.ToString(c.totalcash),
            Convert.ToString(c.totalCashUsd),
            Convert.ToString(c.ecocash),
            //Convert.ToString(c.telecash),
            //Convert.ToString(c.onemoney),
            Convert.ToString(c.rtgs),
            //Convert.ToString(c.nostro),
            //Convert.ToString(c.totalCashUsd),
            Convert.ToString(c.totalAmount),
            Convert.ToString(c.totalChange),
             Convert.ToString(c.AddedBy),
           // Convert.ToString(user.FirstOrDefault(i=>i.Id==c.AddedBy).UserName),
            Convert.ToString(c.DateAdded),
            //Convert.ToString(c.DateModied),
            //Convert.ToString(c.ModifiedBy),
            Convert.ToString(c.Declared),
            Convert.ToString(c.WarehouseId),
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult ModelBindIndex()
        {
            return View();
        }
        // GET: DeclaredayEnd/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeclaredayEnd takings = db.DayEnds.Find(id);
            if (takings == null)
            {
                return HttpNotFound();
            }
            return View(takings);
        }
        // GET: DeclaredayEnd/Create
        public ActionResult Create()
        {
            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
            ViewBag.Tilloperator = db.Users.FirstOrDefault(i=>i.Id==AddedBy).UserName;

            return View();
        }

        // POST: DeclaredayEnd/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(DeclaredayEnd en)
        {
            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
         
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {
                    en.AddedBy = AddedBy;
                    en.DateAdded = DateTime.Now;
                    en.WarehouseId = Convert.ToInt32(warehouseId);
                    db.DayEnds.Add(en);
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

        // GET: DeclaredayEnd/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeclaredayEnd ObjInvoiceItems = db.DayEnds.Find(id);
            if (ObjInvoiceItems == null)
            {
                return HttpNotFound();
            }
           // ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", ObjInvoiceItems.ProductId);

            return View(ObjInvoiceItems);
        }

        // POST: DeclaredayEnd/Edit/5
        [HttpPost]
        public ActionResult Edit(DeclaredayEnd en)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(en).State = EntityState.Modified;
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

        // GET: DeclaredayEnd/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclaredayEnd/Delete/5aa
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                return View();
            }
        }
    }
}
