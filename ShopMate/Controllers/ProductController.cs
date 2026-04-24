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
    public class ProductController : BaseController
    {
        string userId = Env.GetUserInfo("name");
        int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
        // GET: /Product/
        public ActionResult Index()
        {
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategorys, "Id", "Name");
            return View();
        }
        public ActionResult BarcodeScaner()
        {
            return View();
        }
        public ActionResult Autobreak(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var myProductWarehouse = db.Users.FirstOrDefault(i => i.UserName == userId).WarehouseId; 
            string result = "Break Failed.";
            var tak = db.Products.Find(id);
            var me = db.WarehouseStocks.FirstOrDefault(n => n.ProductId == id && n.WarehouseId == myProductWarehouse).ProductId;
            var tak1 = db.WarehouseStocks.FirstOrDefault(m =>m.ProductId == me && m.WarehouseId == myProductWarehouse);

            if (tak.ProductCaseId != null)
            {                
                var ProdCase = db.Products.Find(tak.ProductCaseId);
                var WareProdCase = db.WarehouseStocks.FirstOrDefault(m =>m.ProductId == tak1.Product_ProductId.ProductCaseId && m.WarehouseId == myProductWarehouse );      
           
                if (tak1.RemainingQuantity <= 0  || WareProdCase.RemainingQuantity -5 <= ProdCase.StockAlert)
                {
                    //Break one case 
                    //subtract 1 case from Prodcase
                    WareProdCase.RemainingQuantity = WareProdCase.RemainingQuantity - 1;

                    // add 1* number of singles in case to tak
                    tak1.RemainingQuantity = (int)(tak1.RemainingQuantity + (1 * ProdCase.NumOfSinglesInCase));             

                }
                else
                {

                    //break 5 cases
                    //subtract 5 case from Prodcase

                    ProdCase.RemainingQuantity = ProdCase.RemainingQuantity - 5;
                    ProdCase.RemainingAmount = ProdCase.RemainingAmount - (5 * ProdCase.SalePrice);

                    // add 5 * number of singles in case to tak
                    tak.RemainingQuantity = (int)(tak.RemainingQuantity + (5 * ProdCase.NumOfSinglesInCase));
                    tak.RemainingAmount = (int)(tak.RemainingAmount - (5 * ProdCase.NumOfSinglesInCase * tak.SalePrice));

                }

                db.Entry(tak).State = EntityState.Modified;
                db.Entry(tak1).State = EntityState.Modified;
                db.Entry(WareProdCase).State = EntityState.Modified;
                db.Entry(ProdCase).State = EntityState.Modified;
                db.SaveChanges(userId);

            }
            result = "Success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetail(int? id, int warehouseFrom)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string result = "Error! Adjustment  Is Not Complete!";
            Product ObjProduct = db.Products.Find(id);
            //int wareId = int.Parse
            WarehouseStock stock = db.WarehouseStocks.Where(m => m.ProductId == ObjProduct.Id && m.WarehouseId == warehouseFrom).FirstOrDefault();

            if (stock == null)
            {
                //result = " Error! Adjustment  Is Not Complete!" ;
                //return Json(result, JsonRequestBehavior.AllowGet);
                return HttpNotFound();
            }


            if (ObjProduct == null)
            {
                return HttpNotFound();
            }

            //return Json(new { price = ObjProduct.SalePrice, description = ObjProduct.ProductDescription, Name = ObjProduct.Name, taxId = ObjProduct.TaxId, remainingquantity = ObjProduct.RemainingQuantity }, JsonRequestBehavior.AllowGet);
            return Json(new { price = ObjProduct.PurchasePrice, description = ObjProduct.ProductDescription, Name = ObjProduct.Name, taxId = ObjProduct.TaxId, remainingquantity = stock.RemainingQuantity }, JsonRequestBehavior.AllowGet);
        }



        // GET Product/GetGrid
        public ActionResult GetGrid()
        {
            try {
                var tak = db.Products.Where(m=> m.IsActive==true).ToArray();
                // var tax =  db.Taxs;
                var user = db.Users.ToArray();
                var tax =  db.Taxs.ToArray() ;
                var myProductWarehouse = db.Users.FirstOrDefault(i => i.UserName == userId).WarehouseId;
          //      if (myProductWarehouse== 1)
          //      {
          //          var result = from c in tak
          //                       join pd in db.WarehouseStocks on c.Id equals pd.ProductId
          //                       select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
          //  Convert.ToString(c.Name),
          //  Convert.ToString(db.Products.FirstOrDefault(m=> m.Id ==c.ProductCaseId).Name),
          //  Convert.ToString(c.NumOfSinglesInCase),
          //  Convert.ToString(c.BarCode),
          //  Convert.ToString(c.PurchasePrice),
          //  Convert.ToString(c.SalePrice),
          // // Convert.ToString(c.ProductImage),
          // // Convert.ToString(c.AddedBy),
          //  Convert.ToString(c.ProductDescription),
          //  Convert.ToString(c.WarehouseId),
          ////  Convert.ToString(c.StockAlert),
          //  Convert.ToString(pd.RemainingQuantity),
          //  //Convert.ToString(tax.FirstOrDefault(i=>i.Id==c.TaxId).TaxRate+" %")
                
                
                
          //         // Convert.ToString(tax.FirstOrDefault(i => i.Id == c.TaxId).TaxRate + " %")


          //   };
          //          return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
          //      }
          //      else
                {
                    var result = from c in tak.Where(n=> n.WarehouseId == myProductWarehouse)
                                 join pd in db.WarehouseStocks on c.Id equals pd.ProductId
                                 select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.Name),
          Convert.ToString(db.Products.FirstOrDefault(m=> m.Id ==c.ProductCaseId).Name),
               Convert.ToString(c.NumOfSinglesInCase),
            Convert.ToString(c.BarCode),
            Convert.ToString(c.PurchasePrice),
            Convert.ToString(c.SalePrice),
            //Convert.ToString(c.ProductImage),
            //Convert.ToString(c.AddedBy),
            Convert.ToString(c.ProductDescription),
            Convert.ToString(c.WarehouseId),
           // Convert.ToString(c.StockAlert),
            Convert.ToString(pd.RemainingQuantity),
         //   Convert.ToString(tax.FirstOrDefault(i=>i.Id==c.TaxId).TaxRate+" %")
                
                
                
                   // Convert.ToString(tax.FirstOrDefault(i => i.Id == c.TaxId).TaxRate + " %")


             };
                    return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
                }
              
             
              
               
               
               
            }catch(NullReferenceException ex)
            {
                Helper.WriteError(ex, ex.Message);
                return View(ex.Message);
            }
            }
        //Update Product Prices
       
        
        [HttpPost]
        public void UpdatePrice(int Id, decimal addprice)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try
            {
                //  ViewBag.ProductCategoryId = db.Products.Where(i => i.ProductCategoryId == category);
                string constring = System.Configuration.ConfigurationManager.ConnectionStrings["SIConnectionString"]
                   .ConnectionString;
                List<Product> products = new List<Product>();

                string qury = "UPDATE Product SET SalePrice=SalePrice+('" + (addprice / 100) + "')*SalePrice WHERE ProductCategoryId='" + Id + "'";
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand(qury, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }


            }




            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }


            //  return Content(sb.ToString());

        }
        //decrease product prices by category
        [HttpPost]
        public void DecreasePrice(int Id, decimal addprice)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try
            {
                //  ViewBag.ProductCategoryId = db.Products.Where(i => i.ProductCategoryId == category);
                string constring = System.Configuration.ConfigurationManager.ConnectionStrings["SIConnectionString"]
                   .ConnectionString;
                List<Product> products = new List<Product>();

                string qury = "UPDATE Product SET SalePrice=SalePrice-('" + (addprice / (100+addprice)) + "')*SalePrice WHERE ProductCategoryId='" + Id + "'";
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand(qury, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }






            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }


            //  return Content(sb.ToString());

        }
        //increase all prouduct prices
        [HttpPost]
        public void increaseall(decimal addprice)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try
            {
                //  ViewBag.ProductCategoryId = db.Products.Where(i => i.ProductCategoryId == category);
                string constring = System.Configuration.ConfigurationManager.ConnectionStrings["SIConnectionString"]
                   .ConnectionString;
                List<Product> products = new List<Product>();

                string qury = "UPDATE Product SET SalePrice=SalePrice+('" + (addprice / 100) + "')*SalePrice";
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand(qury, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }






            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }


            //  return Content(sb.ToString());

        }
        //decrease all product prices
        [HttpPost]
        public void decreaseall(decimal addprice)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try
            {
                //  ViewBag.ProductCategoryId = db.Products.Where(i => i.ProductCategoryId == category);
                string constring = System.Configuration.ConfigurationManager.ConnectionStrings["SIConnectionString"]
                   .ConnectionString;
                List<Product> products = new List<Product>();

                string qury = "UPDATE Product SET SalePrice=SalePrice-('" + (addprice / (100+addprice)) + "')*SalePrice";
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand(qury, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }






            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }


            //  return Content(sb.ToString());

        }
        // GET: /Product/ModelBindIndex
        public ActionResult ModelBindIndex()
        {
            return View();
        }
        // GET: /Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product ObjProduct = db.Products.Find(id);
            if (ObjProduct == null)
            {
                return HttpNotFound();
            }
            return View(ObjProduct);
        }
        public ActionResult GetDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product ObjProduct = db.Products.Find(id);
            if (ObjProduct == null)
            {
                return HttpNotFound();
            }
            //return Json(new { price = ObjProduct.SalePrice, description = ObjProduct.ProductDescription, Name = ObjProduct.Name, taxId = ObjProduct.TaxId, remainingquantity = ObjProduct.RemainingQuantity }, JsonRequestBehavior.AllowGet);
            return Json(new { price = ObjProduct.PurchasePrice, description = ObjProduct.ProductDescription, Name = ObjProduct.Name, taxId = ObjProduct.TaxId, remainingquantity =ObjProduct.RemainingQuantity}, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult PurchaseGetDetails(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    //Product ObjProduct = db.Products.Find(id);
        //    Product ObjProduct =(Product)db.Products.Where(p => p.ProductType == "EXTERNAL");
        //    if (ObjProduct == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    //return Json(new { price = ObjProduct.SalePrice, description = ObjProduct.ProductDescription, Name = ObjProduct.Name, taxId = ObjProduct.TaxId, remainingquantity = ObjProduct.RemainingQuantity }, JsonRequestBehavior.AllowGet);
        //    return Json(new { price = ObjProduct.SalePrice, description = ObjProduct.ProductDescription, Name = ObjProduct.Name, taxId = ObjProduct.TaxId, remainingquantity = ObjProduct.RemainingQuantity }, JsonRequestBehavior.AllowGet);
        //}
        // GET: /Product/Create
        public ActionResult Create()
        {
            var myProductWarehouse = db.Users.FirstOrDefault(i => i.UserName == userId).WarehouseId;
            //if (myProductWarehouse== 1)
            //{
            //    ViewBag.ProductCaseId = new SelectList(db.Products, "Id", "Name");
            //    ViewBag.ProductCategoryId = new SelectList(db.ProductCategorys, "Id", "Name");
            //    ViewBag.WareHouse = new SelectList(db.Warehouses, "Id", "Name");
            //    ViewBag.TaxId = new SelectList(db.Taxs, "Id", "Name");
            //}
            //else
            {
                ViewBag.ProductCaseId = new SelectList(db.Products.Where(n => n.WarehouseId == myProductWarehouse), "Id", "Name");
                ViewBag.ProductCategoryId = new SelectList(db.ProductCategorys.Where(n => n.WarehouseId == myProductWarehouse), "Id", "Name");
                ViewBag.WareHouse = new SelectList(db.Warehouses.Where(n => n.Id == myProductWarehouse), "Id", "Name");
                ViewBag.TaxId = new SelectList(db.Taxs, "Id", "Name");
            }
      
            return View();
        }

        // POST: /Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Product ObjProduct, HttpPostedFileBase ProductImage, string HideImage1)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! Product not added! The Product already exist.";
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var tak = db.Products.Where(i => i.Name == ObjProduct.Name && i.WarehouseId == ObjProduct.WarehouseId).FirstOrDefault();
            var tak2 = db.WarehouseStocks.Where(i => i.Product_ProductId.Name == ObjProduct.Name && i.WarehouseId == warehouse).FirstOrDefault();
            var warehouses = db.Warehouses.ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    if (ProductImage != null)
                    {
                        var fileName = MicrosoftHelper.MSHelper.StarkFileUploaderCSharp(ProductImage, Server.MapPath("~/Uploads"));
                        ModelState.Clear();
                        ObjProduct.ProductImage = fileName;
                    }
                    else
                    {
                        ObjProduct.ProductImage = HideImage1;
                    }
                 
                    if (tak != null)
                    {
                        //foreach (var item in warehouses)
                        {
                            if (tak2 == null)
                            {
                                WarehouseStock wstock = new WarehouseStock();
                                wstock.ProductId = db.Products.FirstOrDefault(i => i.Name == ObjProduct.Name).Id;
                                wstock.RemainingQuantity = 0;
                                 wstock.WarehouseId = warehouse;
                                db.WarehouseStocks.Add(wstock);
                                db.SaveChanges(userId);
                            }

                        }
                        sb.Append("Product Already Exist ");
                        return Content(sb.ToString());
                       // return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if(ObjProduct.ProductCaseId == null)
                        {
                            ObjProduct.ProductCaseId = 1;
                        }
                        ObjProduct.WarehouseId = warehouse;
                        db.Products.Add(ObjProduct);
                        //db.SaveChanges(userId);
                   //     foreach (var item in warehouses)
                        {
                            WarehouseStock wstock = new WarehouseStock();
                            wstock.ProductId = ObjProduct.Id;
                            wstock.RemainingQuantity = 0;
                            wstock.WarehouseId = warehouse;
                            db.WarehouseStocks.Add(wstock);
                            db.SaveChanges(userId);
                            //result = "Success! Product Added";
                        }
                        //result = "Success! Product Added";
                     
                       //return Json(result, JsonRequestBehavior.AllowGet);
                    }


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
        // GET: /Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product ObjProduct = db.Products.Find(id);

            if (ObjProduct == null)
            {
                return HttpNotFound();
            }
            var myProductWarehouse = db.Users.FirstOrDefault(i => i.UserName == userId).WarehouseId;
            if (myProductWarehouse==1)
            {
                ViewBag.ProductCategoryId = new SelectList(db.ProductCategorys, "Id", "Name", ObjProduct.ProductCategoryId);
                ViewBag.TaxId = new SelectList(db.Taxs, "Id", "Name", ObjProduct.TaxId);
                ViewBag.ProductCaseId = new SelectList(db.Products, "Id", "Name");
                ViewBag.WareHouse = new SelectList(db.Warehouses, "Id", "Name");
            }
            else
            {

            ViewBag.ProductCategoryId = new SelectList(db.ProductCategorys, "Id", "Name", ObjProduct.ProductCategoryId);
            ViewBag.TaxId = new SelectList(db.Taxs, "Id", "Name", ObjProduct.TaxId);
            ViewBag.ProductCaseId = new SelectList(db.Products.Where(n => n.WarehouseId == myProductWarehouse), "Id", "Name");
            ViewBag.WareHouse = new SelectList(db.Warehouses.Where(n => n.Id == myProductWarehouse), "Id", "Name");
            }
         

            return View(ObjProduct);
        }

        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product ObjProduct, HttpPostedFileBase ProductImage, string HideImage1)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try
            {
                //  List<ProductStock> stock = db.ProductStocks.Where(i=>i.Id==ObjProduct.ProductStock_ProductIds);
                if (ModelState.IsValid)
                {
                    if (ProductImage != null)
                    {
                        var fileName = MicrosoftHelper.MSHelper.StarkFileUploaderCSharp(ProductImage, Server.MapPath("~/Uploads"));
                        // ModelState.Clear();
                        ObjProduct.ProductImage = fileName;
                    }
                    else
                    {
                        ObjProduct.ProductImage = HideImage1;
                    }

                    //if (ObjProduct.ProductCaseId==null)
                    //{
                    //    var caseId = db.Products.FirstOrDefault(m => m.Id == ObjProduct.Id);
                    //    ObjProduct.HSN = Convert.ToString(caseId.ProductCaseId);

                    //    ObjProduct.ProductCaseId = 783;
                    //}
                       
                    ObjProduct.WarehouseId = warehouse;
                    db.Entry(ObjProduct).State = EntityState.Modified;
                    db.SaveChanges(userId);

                    //if (ObjProduct.ProductCaseId == 783)
                    //{
                    //    var caseId = db.Products.FirstOrDefault(m => m.Id == ObjProduct.Id);
                    //    ObjProduct.ProductCaseId = Convert.ToInt32(caseId.HSN);
                    //}
                    //db.Entry(ObjProduct).State = EntityState.Modified;
                    //db.SaveChanges(userId);


                    sb.Append("Sumitted");
                    //end
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

        // GET: /Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product ObjProduct = db.Products.Find(id);
            if (ObjProduct == null)
            {
                return HttpNotFound();
            }
            return View(ObjProduct);
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                WarehouseStock wstock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == id);
                Product ObjProduct = db.Products.Find(id);
                db.Products.Remove(ObjProduct);
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
        // GET: /Product/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            Product ObjProduct = db.Products.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;
                ViewBag.ProductCategoryId = new SelectList(db.ProductCategorys, "Id", "Name", ObjProduct.ProductCategoryId);
                ViewBag.TaxId = new SelectList(db.Taxs, "Id", "Name", ObjProduct.TaxId);
            }

            return View(ObjProduct);
        }

        // POST: /Product/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(Product ObjProduct, HttpPostedFileBase ProductImage, string HideImage1)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {
                    if (ProductImage != null)
                    {
                        var fileName = MicrosoftHelper.MSHelper.StarkFileUploaderCSharp(ProductImage, Server.MapPath("~/Uploads"));
                        ModelState.Clear();
                        ObjProduct.ProductImage = fileName;
                    }
                    else
                    {
                        ObjProduct.ProductImage = HideImage1;
                    }


                    db.Entry(ObjProduct).State = EntityState.Modified;
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

        public ActionResult ProductStockGetGrid(int id = 0)
        {
            AuditLogger auditLogger = new AuditLogger();

            var tak = db.ProductStocks.Where(i => i.ProductId == id).ToArray();

            var result = from c in tak
                         select new string[] { Convert.ToString(c.Id),Convert.ToString(c.Id),
                Convert.ToString(c.ProductId),
                Convert.ToString(c.Quantity),
                Convert.ToString(c.PurchasePrice),
                Convert.ToString(c.TotalPurchaseAmount),
                Convert.ToString(c.TotalSaleAmount),
                Convert.ToString(c.TotalSaleAmountWithTax),
                Convert.ToString(c.Discount),
                Convert.ToString(c.CGST),
                Convert.ToString(c.CGST_Rate),
                Convert.ToString(c.SGST),
                Convert.ToString(c.SGST_Rate),
                Convert.ToString(c.IGST),
                Convert.ToString(c.IGST_Rate),
                Convert.ToString(c.TaxId),
                Convert.ToString(c.OtherTaxValue),
                Convert.ToString(c.Description),
                Convert.ToString(c.AddedBy),
                Convert.ToString(c.DateAdded),
                Convert.ToString(c.ModifiedBy),
                Convert.ToString(c.DateModied),
                Convert.ToString(c.InventoryType_InventoryTypeId.Name),Convert.ToString(c.SalePrice),
                Convert.ToString(c.TaxAmount),
                Convert.ToString(c.Profit),
                Convert.ToString(c.ProfitWithTax),
                Convert.ToString(c.WarehouseId),
                 };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaleGetGrid(int id = 0)
        {
            var tak = db.Sales.Where(i => i.ProductId == id).ToArray();

            var result = from c in tak
                         select new string[] { Convert.ToString(c.Id),Convert.ToString(c.Id),
                Convert.ToString(c.User_CustomerUserId.UserName),Convert.ToString(c.Quantity),
                Convert.ToString(c.SalePrice),
                Convert.ToString(c.PaymentMode_PaymentModeId.Name),Convert.ToString(c.TotalAmount),
                Convert.ToString(c.PaidAmount),
                Convert.ToString(c.ProductId),
                Convert.ToString(c.DateAdded),
                Convert.ToString(c.ModifiedBy),
                Convert.ToString(c.DateModied),
                Convert.ToString(c.AddedBy),
                Convert.ToString(c.WarehouseId),
                 };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InvoiceItemsGetGrid(int id = 0)
        {
            var tak = db.InvoiceItemss.Where(i => i.ProductId == id).ToArray();

            var result = from c in tak
                         select new string[] { Convert.ToString(c.Id),Convert.ToString(c.Id),
                Convert.ToString(c.ProductId),
                Convert.ToString(c.Quantity),
                Convert.ToString(c.SalePrice),
                Convert.ToString(c.TaxAmount),
                Convert.ToString(c.TotalAmount),
                Convert.ToString(c.TotalAmountWithTax),
                Convert.ToString(c.AddedBy),
                Convert.ToString(c.DateAdded),
                Convert.ToString(c.TaxId),
                Convert.ToString(c.PurchaseId),
                Convert.ToString(c.SaleId),
                Convert.ToString(c.ProductStockId),
                Convert.ToString(c.TransactionId),
                Convert.ToString(c.WarehouseId),
                 };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PurchaseGetGrid(int id = 0)
        {
            var tak = db.Purchases.Where(i => i.ProductId == id).ToArray();

            var result = from c in tak
                         select new string[] { Convert.ToString(c.Id),Convert.ToString(c.Id),
                Convert.ToString(c.User_VendorUserId.UserName),Convert.ToString(c.ProductId),
                Convert.ToString(c.Quantity),
                Convert.ToString(c.UnitPrice),
                Convert.ToString(c.TotalAmount),
                Convert.ToString(c.DateAdded),
                Convert.ToString(c.AddedBy),
                Convert.ToString(c.WarehouseId),
                 };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        private SIContext db = new SIContext();
        public ActionResult ProductHistory(int? id)
        {
            Product ProductInfo = db.Products.Find(id);
            var stockitems = db.ProductStocks.Where(q => q.ProductId == id).ToArray();

            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);

            if (ProductInfo == null)
            {
                return HttpNotFound();
            }

            ProductHistoryDto dto = new ProductHistoryDto();
            var remaining = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == id && i.WarehouseId == ProductInfo.WarehouseId).RemainingQuantity;
            dto.Warehouse = db.Warehouses.FirstOrDefault(i => i.Id == ProductInfo.WarehouseId).Name;
            dto.Id = ProductInfo.Id;
            // dto.StockValueRtgs = ProductInfo.StockValueRtgs;
            dto.ProductName = ProductInfo.Name;
            dto.SalePrice = ProductInfo.SalePrice;
            // dto.Van = ProductInfo.Van_VanId.RegNumber;
            dto.IsActive = ProductInfo.IsActive;
            dto.PurchasePrice = ProductInfo.PurchasePrice;
            dto.RemainingQty = remaining;
            //dto.Route = ProductInfo.Route;

            List<ProductHistoryitemsDto> itemsList = new List<ProductHistoryitemsDto>();

            foreach (var items in stockitems)
            {
                ProductHistoryitemsDto itemDto = new ProductHistoryitemsDto();
                // double? myrtgdstockvalue = items.StockValueRtgs;
                itemDto.Id = itemDto.Id;
                itemDto.ProductName = items.Product_ProductId.Name;
                itemDto.ProductId = items.ProductId;
                itemDto.Quantity = items.Quantity;
                itemDto.UnitPrice = items.SalePrice;
                itemDto.SalePrice = itemDto.SalePrice;
                itemDto.TotalSaleAmount = items.TotalSaleAmount;
                itemDto.TotalPurchaseAmount = items.TotalPurchaseAmount;
                itemDto.DateAdded = (DateTime)items.DateAdded;
                itemDto.AddedBy = items.AddedBy;
                itemDto.InventoryTypeId = items.InventoryType_InventoryTypeId.Name;
                //itemDto.ReceiptNo = items.recieptNumber;
                itemDto.RemainingQty = items.RemainingQuantity;
                itemsList.Add(itemDto);
            }

            dto.items = itemsList;

            return View(dto);
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

