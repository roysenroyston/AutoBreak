using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using ShopMate.Models;


namespace ShopMate.Controllers
{
    public class ProductUpdateController : BaseController
    {

        private SIContext db = new SIContext();

        string userId = Env.GetUserInfo("name");
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: ProductUpdate
        public ActionResult Index()
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

                foreach (Product product in fileData)
                {
                    Product updateProduct = db.Products.Where(i => i.Id == product.Id).FirstOrDefault();
                    //  Product updateProduct = db.Products.Where(i => i.Id == product.Id).FirstOrDefault();
                    var caseId =  db.Products.FirstOrDefault(m => m.Name == product.ProductType && m.WarehouseId== product.WarehouseId).Id;
                    if ( updateProduct != null )
                    {

                        if (updateProduct.Id == product.Id && updateProduct.WarehouseId == product.WarehouseId) {

                            updateProduct.Name = product.Name;
                            updateProduct.BarCode = product.BarCode;
                            updateProduct.SalePrice = product.SalePrice;
                            updateProduct.ProductDescription = product.ProductDescription;
                            updateProduct.PurchasePrice = product.PurchasePrice;
                            updateProduct.WarehouseId = product.WarehouseId;
                            updateProduct.ProductCaseId = caseId;
                            updateProduct.NumOfSinglesInCase = product.NumOfSinglesInCase;

                            db.Entry(updateProduct).State = EntityState.Modified;
                            db.SaveChanges();

                            //WarehouseStock stock = db.WarehouseStocks.FirstOrDefault(b => b.ProductId == product.Id);
                            //stock.WarehouseId = product.WarehouseId;
                            //stock.RemainingQuantity = product.RemainingQuantity;
                            //db.Entry(stock).State = EntityState.Modified;
                            //db.SaveChanges();

                        }
                        else
                        {
                            Product newprod = new Product();
                            int UserId = db.Users.FirstOrDefault(n => n.UserName == userId).Id;
                           // newprod.Id = product.Id;
                            newprod.Name = product.Name;
                            newprod.BarCode = product.BarCode;
                            newprod.SalePrice = product.SalePrice;
                            newprod.ProductDescription = product.ProductDescription;
                            newprod.PurchasePrice = product.PurchasePrice;
                            newprod.IsActive = true;
                            newprod.AddedBy = UserId;
                            newprod.WarehouseId = product.WarehouseId;
                            newprod.StockAlert = 10;
                            newprod.ProductCategoryId = 1;
                            newprod.NumOfSinglesInCase = product.NumOfSinglesInCase;
                            newprod.DateAdded = DateTime.Now;
                            newprod.DateModied = DateTime.Now;
                            newprod.TaxId = 5;
                            newprod.ProductCaseId = caseId;
                            db.Products.Add(newprod);
                            db.SaveChanges();

                            ProductStock ps = new ProductStock();
                            ps.ProductId = product.Id;


                            ps.Quantity = product.RemainingQuantity;

                            ps.PurchasePrice = product.PurchasePrice;

                            ps.TotalPurchaseAmount = (product.PurchasePrice * ps.Quantity);

                            ps.SalePrice = product.SalePrice;
                            ps.Discount = 0;
                            ps.TotalSaleAmount = (ps.SalePrice * ps.Quantity);

                            decimal TaxAmount = 0;


                            ps.TotalSaleAmountWithTax = (ps.SalePrice * ps.Quantity);//+ TaxAmount
                            ps.TaxAmount = TaxAmount;

                            //  ps.Profit = (ps.TotalSaleAmount - (ps.TotalPurchaseAmount)) - (discount / mysellCount.Count);//+ TaxAmount
                            ps.ProfitWithTax = (ps.TotalSaleAmount - ps.TotalPurchaseAmount);//+ TaxAmount

                            ps.Description = "Product Import";
                            ps.AddedBy = UserId;
                            ps.DateAdded = DateTime.Now;
                            ps.ModifiedBy = UserId;
                            ps.DateModied = DateTime.Now;
                            ps.InventoryTypeId = 1007;
                            ps.WarehouseId = product.WarehouseId;
                            ps.IsFormal = true;
                            ps.RemainingQuantity = product.RemainingQuantity;

                            db.ProductStocks.Add(ps);
                            db.SaveChanges();




                            WarehouseStock newProduct = new WarehouseStock();
                            newProduct.ProductId = newprod.Id;
                            newProduct.WarehouseId = newprod.WarehouseId;
                            newProduct.RemainingQuantity = product.RemainingQuantity;
                            newProduct.ReturnedQuantity = 0;
                            db.WarehouseStocks.Add(newProduct);
                            db.SaveChanges();

                        }

                    }
                    else
                    {
                        Product newprod = new Product();
                        int UserId = db.Users.FirstOrDefault(n => n.UserName == userId).Id;
                       // newprod.Id = product.Id;
                        newprod.Name = product.Name;
                        newprod.BarCode = product.BarCode;
                        newprod.SalePrice = product.SalePrice;
                        newprod.ProductDescription = product.ProductDescription;
                        newprod.PurchasePrice = product.PurchasePrice;
                        newprod.IsActive = true;
                        newprod.AddedBy = UserId;
                        newprod.WarehouseId = product.WarehouseId;
                        newprod.StockAlert = 10;
                        newprod.ProductCategoryId = 1;
                        newprod.NumOfSinglesInCase = product.NumOfSinglesInCase;
                        newprod.DateAdded = DateTime.Now;
                        newprod.DateModied = DateTime.Now;
                        newprod.TaxId = 5;
                        newprod.ProductCaseId = caseId;
                        db.Products.Add(newprod);
                        db.SaveChanges();


                        ProductStock ps = new ProductStock();
                        ps.ProductId = product.Id;


                        ps.Quantity = product.RemainingQuantity;

                        ps.PurchasePrice = product.PurchasePrice;

                        ps.TotalPurchaseAmount = (product.PurchasePrice * ps.Quantity);

                        ps.SalePrice = product.SalePrice;
                        ps.Discount = 0;
                        ps.TotalSaleAmount = (ps.SalePrice * ps.Quantity);

                        decimal TaxAmount = 0;


                        ps.TotalSaleAmountWithTax = (ps.SalePrice * ps.Quantity);//+ TaxAmount
                        ps.TaxAmount = TaxAmount;

                        //  ps.Profit = (ps.TotalSaleAmount - (ps.TotalPurchaseAmount)) - (discount / mysellCount.Count);//+ TaxAmount
                        ps.ProfitWithTax = (ps.TotalSaleAmount - ps.TotalPurchaseAmount);//+ TaxAmount

                        ps.Description = "Product Import";
                        ps.AddedBy = UserId;
                        ps.DateAdded = DateTime.Now;
                        ps.ModifiedBy = UserId;
                        ps.DateModied = DateTime.Now;
                        ps.InventoryTypeId = 1007;
                        ps.WarehouseId = product.WarehouseId;
                        ps.IsFormal = true;
                        ps.RemainingQuantity = product.RemainingQuantity;

                        db.ProductStocks.Add(ps);
                        db.SaveChanges();






                        WarehouseStock newProduct = new WarehouseStock();
                        newProduct.ProductId = newprod.Id;
                        newProduct.WarehouseId = newprod.WarehouseId;
                        newProduct.RemainingQuantity = product.RemainingQuantity;
                        newProduct.ReturnedQuantity = 0;
                        db.WarehouseStocks.Add(newProduct);
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


        private List<Product> GetDataFromCSVFile(Stream stream)
        {
            var empList = new List<Product>();
            int Ngoni = 0;
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
                            empList.Add(new Product()
                            {
                                Id = Convert.ToInt16(objDataRow["Id"].ToString()),
                                Name = Convert.ToString(objDataRow["Name"].ToString()),
                                BarCode = Convert.ToString(objDataRow["Bar Code"].ToString()),
                                ProductType = Convert.ToString(objDataRow["Case Name"].ToString()),
                                PurchasePrice = Convert.ToDecimal(objDataRow["Purchase Price"].ToString()),
                                SalePrice = Convert.ToDecimal(objDataRow["Sale Price"].ToString()),
                               ProductDescription = Convert.ToString(objDataRow["Product Description"].ToString()),
                               WarehouseId = Convert.ToInt16(objDataRow["Warehouse Id"].ToString()),
                               RemainingQuantity = Convert.ToDecimal(objDataRow["RemainingQuantity"].ToString()),
                                NumOfSinglesInCase = Convert.ToInt32(objDataRow["No Of Singles"].ToString()),

                                //Ngoni to add spefic parameters for the product model

                            });
                            Ngoni = Ngoni + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var me = Ngoni;
                   // return Json(new { Status = 0, Message = ex.Message });
                throw;
            }
            return empList;
        }


    }
}