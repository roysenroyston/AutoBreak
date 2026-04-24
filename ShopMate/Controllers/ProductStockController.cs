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
using ShopMate.ModelDto;
using WebErrorLogging.Utilities;

namespace ShopMate.Controllers
{
    public class ProductStockController : BaseController
    {
        string userId = Env.GetUserInfo("name");
        string warehouse = Env.GetUserInfo("WarehouseId");
        int AddedBy = int.Parse(Env.GetUserInfo("userid"));
        int warehouses = int.Parse(Env.GetUserInfo("WarehouseId"));
        // GET: /ProductStock/
        public ActionResult Index()
        {
            //ViewBag.ProductId = new SelectList(db.ProductStocks, "Id", "Name");
            return View();
        }

        // GET ProductStock/GetGrid
        public ActionResult GetGrid()
        {
            try
            {
            //  int wareid = Convert.ToInt16(warehouse);
                var tak = db.ProductStocks.Where(k=> k.WarehouseId == warehouses).ToArray();
                var user = db.Users.ToArray();
                var tax = db.Taxs.ToArray();
            
                
                    var result = from c in tak.Where(n =>  n.InventoryTypeId== 1012 || n.InventoryTypeId == 1011)
                                 select new string[] {
                            c.Id.ToString(),
                            Convert.ToString(c.Id),
            Convert.ToString(c.Product_ProductId.Name),
            Convert.ToString(c.Quantity),
            Convert.ToString(c.Description),
            Convert.ToString(db.Users.FirstOrDefault(m=> m.Id==c.AddedBy).UserName),
            Convert.ToString(c.DateAdded),
            Convert.ToString((db.Users.FirstOrDefault(m=> m.Id==c.ModifiedBy).UserName)),
            Convert.ToString(c.DateModied),
            Convert.ToString(c.InventoryType_InventoryTypeId.Name),
            Convert.ToString(db.Warehouses.FirstOrDefault( w => w.Id == c.WarehouseId).Name)
            };
                    return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
                
                

            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                return View(ex.Message);
            }

            //bool isAdmin = false;
            ////TODO: Check the user if it is admin or normal user, (true-Admin, false- Normal user)
            //string output = isAdmin ? "Welcome to the Admin User" : "Welcome to the User";

            //return Json(output, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ModelBindIndex()
        {
            return View();
        }
        // GET: /ProductStock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStock ObjProductStock = db.ProductStocks.Find(id);
            if (ObjProductStock == null)
            {
                return HttpNotFound();
            }
            return View(ObjProductStock);
        }
       
        // GET: /ProductStock/Create
        public ActionResult Create()
        {
            var userCustomers = db.Users.FirstOrDefault(n => n.UserName == userId).WarehouseId;
            if (userId == "Zimhope")
            {
                ViewBag.WareHouse = new SelectList(db.Warehouses, "Id", "Name");
                ViewBag.ProductId = new SelectList(db.Products.Where(m=> m.IsActive == true), "Id", "Name");
                ViewBag.InventoryTypeId = new SelectList(db.InventoryTypes.Where(n => n.Id == 1011 || n.Id == 1012), "Id", "Name");
                ViewBag.taxId = new SelectList(db.Taxs, "Id", "Name");
                return View();
            }
            else
            {
                ViewBag.WareHouse = new SelectList(db.Warehouses.Where(n => n.Id == userCustomers), "Id", "Name");
                ViewBag.ProductId = new SelectList(db.Products.Where(i=>i.WarehouseId== userCustomers), "Id", "Name");
                ViewBag.InventoryTypeId = new SelectList(db.InventoryTypes.Where(n => n.Id == 1011 || n.Id == 1012), "Id", "Name");
                ViewBag.taxId = new SelectList(db.Taxs, "Id", "Name");
                return View();
            }
        }

        // POST: /ProductStock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        //   [ValidateInput(false)]
        //public ActionResult Create(ProductStock ObjProductStock)
        public ActionResult Create(/*ProductStock ObjProductStock,*/ int? InvenoryId, ProductStock[] productss, int? WarehouseId, string Description = "")
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! Adjustment  Is Not Complete!";



            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in productss)
                    {
                        ProductStock ObjProductStock = new ProductStock();
                        ObjProductStock.ProductId = item.ProductId;
                        ObjProductStock.InventoryTypeId = InvenoryId;
                        ObjProductStock.WarehouseId = (int)WarehouseId;
                        ObjProductStock.Description = Description;
                        ObjProductStock.Quantity = item.Quantity;
                        ObjProductStock.AddedBy = AddedBy;
                        ObjProductStock.DateAdded = DateTime.Now;
                        ObjProductStock.DateModied = DateTime.Now;
                        ObjProductStock.ModifiedBy = AddedBy;



                        var selectedProduct = db.Products.FirstOrDefault(i => i.Id == ObjProductStock.ProductId);
                        var ObjWarehouseStock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == ObjProductStock.ProductId && i.WarehouseId == ObjProductStock.WarehouseId);//ngoni

                        //if (ObjProductStock.InventoryTypeId == 1|| ObjProductStock.InventoryTypeId == 2)
                        if (ObjProductStock.InventoryTypeId == 1)
                        {
                            ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity - ObjProductStock.Quantity;
                            db.Entry(ObjWarehouseStock).State = EntityState.Modified;//ngonie
                            db.SaveChanges(userId);

                        }
                        else if (ObjProductStock.InventoryTypeId == 2)
                        {
                            //selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity - ObjProductStock.Quantity;
                            ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity - ObjProductStock.Quantity;
                            //db.Entry(selectedProduct).State = EntityState.Modified;
                            db.Entry(ObjWarehouseStock).State = EntityState.Modified;//ngonie
                            db.SaveChanges(userId);
                        }

                        else if (ObjProductStock.InventoryTypeId == 5)
                        {

                            selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity - ObjProductStock.Quantity;
                            selectedProduct.RemainingAmount = selectedProduct.RemainingAmount - ObjProductStock.TotalSaleAmountWithTax;
                            db.Entry(selectedProduct).State = EntityState.Modified;
                            db.SaveChanges(userId);
                        }
                        else if (ObjProductStock.InventoryTypeId == 6)
                        {
                            selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity + ObjProductStock.Quantity;
                            selectedProduct.RemainingAmount = selectedProduct.RemainingAmount + ObjProductStock.TotalSaleAmountWithTax;
                            db.Entry(selectedProduct).State = EntityState.Modified;
                            db.SaveChanges(userId);
                        }
                        else if (ObjProductStock.InventoryTypeId == 7)
                        {
                            selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity - ObjProductStock.Quantity;
                            selectedProduct.RemainingAmount = selectedProduct.RemainingAmount - ObjProductStock.TotalSaleAmountWithTax;
                            db.Entry(selectedProduct).State = EntityState.Modified;
                            db.SaveChanges(userId);
                        }
                        else if (ObjProductStock.InventoryTypeId == 3)
                        {
                            selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity - ObjProductStock.Quantity;
                            selectedProduct.RemainingAmount = selectedProduct.RemainingAmount - ObjProductStock.TotalSaleAmountWithTax;
                            db.Entry(selectedProduct).State = EntityState.Modified;
                            db.SaveChanges(userId);
                        }
                        else if (ObjProductStock.InventoryTypeId == 4)
                        {
                            //selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity + ObjProductStock.Quantity;
                            //selectedProduct.RemainingAmount = selectedProduct.RemainingAmount + ObjProductStock.TotalSaleAmountWithTax;
                            ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity + ObjProductStock.Quantity;
                            //db.Entry(selectedProduct).State = EntityState.Modified;
                            db.Entry(ObjWarehouseStock).State = EntityState.Modified;//ngonie
                            db.SaveChanges(userId);
                        }
                        else if (ObjProductStock.InventoryTypeId == 1011 || ObjProductStock.InventoryTypeId == 1013)
                        {
                            ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity + ObjProductStock.Quantity;
                            db.Entry(ObjWarehouseStock).State = EntityState.Modified;//ngonie
                            db.SaveChanges(userId);
                        }
                        else if (ObjProductStock.InventoryTypeId == 1012)
                        {
                            if (ObjWarehouseStock.RemainingQuantity > 0)
                            {
                                ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity - ObjProductStock.Quantity;
                                db.Entry(ObjWarehouseStock).State = EntityState.Modified;//ngonie
                                db.SaveChanges(userId);
                            }
                            else
                            {
                                sb.Append("error");
                            }

                        }

                        ObjProductStock.PurchasePrice = selectedProduct.PurchasePrice;
                        ObjProductStock.SalePrice = selectedProduct.SalePrice;
                        ObjProductStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity;
                        db.ProductStocks.Add(ObjProductStock);
                        db.SaveChanges(userId);

                    }
                    result = "Success! Adjustment Completed";
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

            //   return Content(sb.ToString());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: /ProductStock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStock ObjProductStock = db.ProductStocks.Find(id);
            if (ObjProductStock == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", ObjProductStock.ProductId);
            ViewBag.InventoryTypeId = new SelectList(db.InventoryTypes, "Id", "Name", ObjProductStock.InventoryTypeId);

            return View(ObjProductStock);
        }

        // POST: /ProductStock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ProductStock ObjProductStock)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjProductStock).State = EntityState.Modified;
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
        // GET: /ProductStock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStock ObjProductStock = db.ProductStocks.Find(id);
            if (ObjProductStock == null)
            {
                return HttpNotFound();
            }
            return View(ObjProductStock);
        }

        // POST: /ProductStock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {

                ProductStock ObjProductStock = db.ProductStocks.Find(id);
                db.ProductStocks.Remove(ObjProductStock);
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
        // GET: /ProductStock/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            ProductStock ObjProductStock = db.ProductStocks.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;
                ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", ObjProductStock.ProductId);
                ViewBag.InventoryTypeId = new SelectList(db.InventoryTypes, "Id", "Name", ObjProductStock.InventoryTypeId);

            }

            return View(ObjProductStock);
        }

        // POST: /ProductStock/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(ProductStock ObjProductStock)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjProductStock).State = EntityState.Modified;
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

