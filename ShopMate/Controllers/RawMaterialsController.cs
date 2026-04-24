using ShopMate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebErrorLogging.Utilities;
using ExcelDataReader;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace ShopMate.Controllers
{
    public class RawMaterialsController : Controller
    {
        string userId = Env.GetUserInfo("name");
        private SIContext db = new SIContext();
        // GET: /Product/
        public ActionResult Index() 
        {
            
            return View();
        }
        public ActionResult GetGrid()
        {
            try
            {
                var tak = db.RawMaterial.ToArray();
                // var tax =  db.Taxs;
                var tax = db.Taxs.ToArray();

                var result = from c in tak
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.Name),
             Convert.ToString(c.ProductCategory),
             Convert.ToString(c.CostPrice),
               Convert.ToString(c.BarCode),
            Convert.ToString(c.RemainingQuantity),
            Convert.ToString(c.StockAlert),
            Convert.ToString(c.WarehouseId),
            Convert.ToString(c.DateAdded),
            Convert.ToString(db.Users.FirstOrDefault(r => r.Id == c.AddedBy).FullName),
            Convert.ToString(tax.FirstOrDefault(i=>i.Id==c.TaxId).TaxRate+" %")
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
            }
            catch (NullReferenceException ex)
            {
                Helper.WriteError(ex, ex.Message);
                return View(ex.Message);
            }
        }
        // GET: RawMaterials/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RawMaterials ObjRawMaterials = db.RawMaterial.Find(id);
            if (ObjRawMaterials == null)
            {
                return HttpNotFound();
            }
            ViewBag.taxId = Convert.ToString(db.Taxs.FirstOrDefault(i => i.Id == (ObjRawMaterials.TaxId)).Name);
            
            //return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
            return View(ObjRawMaterials);
        }


        // GET: RawMaterials/Create
        public ActionResult Create()
        {
            ViewBag.TaxId = new SelectList(db.Taxs, "Id", "Name");
            return View();
        }

        // POST: /Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(RawMaterials ObjProduct)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! Raw Material not added! The Material already exist.";
            var tak = db.RawMaterial.Where(i => i.Name == ObjProduct.Name).FirstOrDefault();

            try
            {
                if (ModelState.IsValid)
                {
                    if (tak != null)
                    {
                        sb.Append("Material Already Exist ");
                        result = "Material Already Exist";
                      //  return Content(sb.ToString());
                         return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        db.RawMaterial.Add(ObjProduct);
                        db.SaveChanges(userId);

                        sb.Append("Sumitted");
                        result = "Sumitted";

                        return Json(result, JsonRequestBehavior.AllowGet);
                        //return View(result);

                    }


                    // sb.Append("Sumitted");
                    //  return Content(sb.ToString());
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
        // GET: RawMaterials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RawMaterials ObjProduct = db.RawMaterial.Find(id);
            if (ObjProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaxId = new SelectList(db.Taxs, "Id", "Name", ObjProduct.TaxId);

            return View(ObjProduct);
        }

        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(RawMaterials ObjProduct)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try
            {
                //  List<ProductStock> stock = db.ProductStocks.Where(i=>i.Id==ObjProduct.ProductStock_ProductIds);
                if (ModelState.IsValid)
                {
                    

                    //ObjProduct.RemainingQuantity = ObjProduct.RemainingQuantity;
                    db.Entry(ObjProduct).State = EntityState.Modified;
                    // db.SaveChanges();
                    int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
                    // var selectedProduct = db.Products.FirstOrDefault(i => i.Id == ObjPurchase.ProductId);
                    var selectedTax = db.Taxs.FirstOrDefault(i => i.Id == ObjProduct.TaxId);

                    sb.Append("Sumitted");



                    db.SaveChanges(userId);


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

        // GET: RawMaterials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RawMaterials ObjProduct = db.RawMaterial.Find(id);
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

                RawMaterials ObjProduct = db.RawMaterial.Find(id);
                db.RawMaterial.Remove(ObjProduct);
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
        public ActionResult RawMaterialsUpdate()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UpdateFile(HttpPostedFileBase importFile)
        {
            if (importFile == null)
            {
                return Json(new { Status = 0, Message = "No File Selected" });
            }

            try
            {
                var fileData = GetDataFromCSVFile(importFile.InputStream);

                foreach (RawmaterialInsert product in fileData)
                {
                    //ProductPrice updateProductPrice = db.ProductPrices.Find(productprice.Id);

                    RawMaterials updateProduct = db.RawMaterial.Where(i => i.Id == product.Id).FirstOrDefault();
                    if (updateProduct != null)
                    {
                        updateProduct.Name = product.Name;
                        updateProduct.BarCode = product.Barcode;
                        updateProduct.CostPrice = product.CostPrice;
                        updateProduct.StockAlert = product.StockAlert;
                        updateProduct.RemainingQuantity = product.RemainingQuantity;
                        //updateProduct.TaxId = product.TaxId;
                       // updateProduct.WarehouseId = product.WarehouseId;
                        db.Entry(updateProduct).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {


                        //   updateProduct.Id = product.Id;
                        updateProduct.Name = product.Name;
                        updateProduct.BarCode = product.Barcode;
                        updateProduct.CostPrice = product.CostPrice;
                        updateProduct.StockAlert = product.StockAlert;
                        updateProduct.RemainingQuantity = product.RemainingQuantity;
                        //updateProduct.TaxId = product.TaxId;
                        //updateProduct.WarehouseId = product.WarehouseId;
                        db.RawMaterial.Add(updateProduct);
                        //  db.Entry(updateProduct).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                return Json(new { Status = 1, Message = "File Imported Successfully ", items = fileData.ToArray() });

                //var dtProducts = fileData.ToDataTable();
                //var tblProductParameter = new SqlParameter("Product", SqlDbType.Structured)
                //{
                //    TypeName = "dbo.Product",
                //    Value = dtProducts
                //};
                //await db.Database.ExecuteSqlCommandAsync("EXEC spBulkImportProduct @Product", tblProductParameter);
                //return Json(new { Status = 1, Message = "File Imported Successfully " });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = ex.Message });
            }
        }

        //private object GetDataFromCSVFile(Stream inputStream)
        //{
        //    throw new NotImplementedException();
        //}
        private List<RawmaterialInsert> GetDataFromCSVFile(Stream stream)
        {
            var empList = new List<RawmaterialInsert>();
            try
            {
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true // To set First Row As Column Names    
                        }
                    });

                    if (dataSet.Tables.Count > 0)
                    {
                        var dataTable = dataSet.Tables[0];
                        foreach (DataRow objDataRow in dataTable.Rows)
                        {
                            if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;
                            empList.Add(new RawmaterialInsert()
                            {
                                Id = Convert.ToInt16(objDataRow["Id"].ToString()),
                                Name = Convert.ToString(objDataRow["Name"].ToString()),
                                CostPrice = Convert.ToString(objDataRow["Cost Price"].ToString()),
                                RemainingQuantity= Convert.ToDecimal(objDataRow["Remaining Quantity"].ToString()),
                                Barcode = Convert.ToString(objDataRow["Barcode"].ToString()),
                                StockAlert = Convert.ToInt32(objDataRow["Stock Alert"].ToString()),
                                //WarehouseId = Convert.ToInt32(objDataRow["Warehouse Id"].ToString()),

                                //RtgsPrice = Convert.ToDecimal(objDataRow["Sale Rtgs Price"].ToString()),

                                //Ngoni to add spefic parameters for the product model

                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return empList;
        }


        public class RawmaterialInsert
        {
            public int Id { get; set; }
            //[Required]
            public string Name { get; set; }
            public string CostPrice { get; set; }
            public string Barcode { get; set; }
            public int StockAlert { get; set; }//ngoni
            public Decimal RemainingQuantity { get; set; }

            //public int WarehouseId { get; set; }//ngoni

            //public int TaxId { get; set; }//ngoni
        }

    }
  
}
