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
		public ActionResult UpdateFile(HttpPostedFileBase importFile)
		{
			if (importFile == null)
			{
				return Json(new { Status = 0, Message = "No File Selected" });
			}

			// Begin a database transaction
			using (var transaction = db.Database.BeginTransaction())
			{

				try
				{
					var fileData = GetDataFromCSVFile(importFile.InputStream).ToList();

					// Preload current user (assuming 'userId' is a class field or property)
					User currentUser = db.Users.FirstOrDefault(n => n.UserName == userId) ?? throw new Exception("User not found");
					int UserId = currentUser.Id;

					// Preload existing products that match any Id in the CSV
					var existingProductIds = fileData.Select(p => p.Id).Distinct().ToList();
					var existingProducts = db.Products
						.Where(p => existingProductIds.Contains(p.Id))
						.ToDictionary(p => p.Id);

					// Preload all possible 'case' products (Name + WarehouseId) for mapping
					var caseProducts = db.Products
						.Select(p => new { p.Name, p.WarehouseId, p.Id })
						.ToList();
					var caseIdLookup = caseProducts
						.GroupBy(p => new { p.Name, p.WarehouseId })
						.ToDictionary(g => g.Key, g => g.First().Id);

					// Collections for batched operations
					var productsToUpdate = new List<Product>();
					var newProducts = new List<Product>();          // new Product entities to be added
					var tempNewProductData = new List<(Product entity, Product source)>(); // store source data for later stock creation

					// For stocks and warehouse stocks – will be populated after new product IDs are known
					var productStocksToAdd = new List<ProductStock>();
					var warehouseStocksToAdd = new List<WarehouseStock>();

					foreach (Product product in fileData)
					{
						// Get caseId (original logic: assumes it always exists, throws if null)
						var caseKey = new { Name = product.ProductType, WarehouseId = product.WarehouseId };
						if (!caseIdLookup.TryGetValue(caseKey, out int caseId))
							continue;

						if (existingProducts.TryGetValue(product.Id, out Product updateProduct))
						{
							// Product exists in DB
							if (updateProduct.Id == product.Id && updateProduct.WarehouseId == product.WarehouseId)
							{
								// Update existing product (no stock changes)
								updateProduct.Name = product.Name;
								updateProduct.BarCode = product.BarCode;
								updateProduct.SalePrice = product.SalePrice;
								updateProduct.ProductDescription = product.ProductDescription;
								updateProduct.PurchasePrice = product.PurchasePrice;
								updateProduct.WarehouseId = product.WarehouseId;
								updateProduct.ProductCaseId = caseId;
								updateProduct.NumOfSinglesInCase = product.NumOfSinglesInCase;
								updateProduct.UnitSalePrice = product.UnitSalePrice;
								updateProduct.ProductType = product.ProductType;
								db.Entry(updateProduct).State = EntityState.Modified;
								productsToUpdate.Add(updateProduct);
							    db.SaveChanges();
							}
							else
							{
								// Warehouse mismatch – treat as new product (original logic)
								var newprod = CreateNewProductEntity(product, UserId, caseId);
								newProducts.Add(newprod);
								tempNewProductData.Add((newprod, product));
								db.SaveChanges();
							}
						}
						else
						{
							// Product does not exist – create new
							var newprod = CreateNewProductEntity(product, UserId, caseId);
							newProducts.Add(newprod);
							tempNewProductData.Add((newprod, product));
							db.SaveChanges();
						}
					}

					// 1. Save all new products to generate their Ids
					if (newProducts.Any())
					{
						db.Products.AddRange(newProducts);
						db.SaveChanges();
					}

					// 2. Now build stock records using the generated Ids
					foreach (var (newprod, source) in tempNewProductData)
					{
						// Use the REAL database-generated product ID, not the CSV Id
						ProductStock ps = new ProductStock
						{
							ProductId = newprod.Id,  
							Quantity = source.RemainingQuantity,
							PurchasePrice = source.PurchasePrice
						};
						ps.TotalPurchaseAmount = source.PurchasePrice * ps.Quantity;
						ps.SalePrice = source.SalePrice;
						ps.Discount = 0;
						ps.TotalSaleAmount = ps.SalePrice * ps.Quantity;
						decimal TaxAmount = 0;
						ps.TotalSaleAmountWithTax = ps.SalePrice * ps.Quantity;
						ps.TaxAmount = TaxAmount;
						ps.ProfitWithTax = (ps.TotalSaleAmount - ps.TotalPurchaseAmount);
						ps.Description = "Product Import";
						ps.AddedBy = UserId;
						ps.DateAdded = DateTime.Now;
						ps.ModifiedBy = UserId;
						ps.DateModied = DateTime.Now;
						ps.InventoryTypeId = 1007;
						ps.WarehouseId = source.WarehouseId;
						ps.IsFormal = true;
						ps.RemainingQuantity = source.RemainingQuantity;
						productStocksToAdd.Add(ps);
						db.SaveChanges();

						// WarehouseStock (already correct – uses newprod.Id)
						WarehouseStock newProduct = new WarehouseStock
						{
							ProductId = newprod.Id,
							WarehouseId = newprod.WarehouseId,
							RemainingQuantity = source.RemainingQuantity,
							ReturnedQuantity = 0
						};
						warehouseStocksToAdd.Add(newProduct);
						db.SaveChanges();
					}

					// 3. Add all stock records and save
					if (productStocksToAdd.Any())
						db.ProductStocks.AddRange(productStocksToAdd);
					if (warehouseStocksToAdd.Any())
						db.WarehouseStocks.AddRange(warehouseStocksToAdd);
					db.SaveChanges();

					// Commit the transaction – all operations succeeded
					transaction.Commit();

					return Json(new { Status = 1, Message = "File Imported Successfully ", items = fileData.ToArray() });
				}
				catch (Exception ex)
				{
					// Rollback is automatic if transaction is disposed without commit,
					// but we explicitly rollback for clarity
					transaction.Rollback();
					return Json(new { Status = 0, Message = ex.Message });
				}
			}
		}

		// Helper method to create a new Product entity without saving (pure logic extraction)
		private Product CreateNewProductEntity(Product source, int userId, int caseId)
		{
			Product newprod = new Product
			{
				// newprod.Id = product.Id;  (commented in original, so omitted)
				Name = source.Name,
				BarCode = source.BarCode,
				SalePrice = source.SalePrice,
				ProductDescription = source.ProductDescription,
				PurchasePrice = source.PurchasePrice,
				IsActive = true,
				AddedBy = userId,
				WarehouseId = source.WarehouseId,
				StockAlert = 10,
				ProductCategoryId = 1,
				NumOfSinglesInCase = source.NumOfSinglesInCase,
				DateAdded = DateTime.Now,
				DateModied = DateTime.Now,
				TaxId = 5,
				ProductCaseId = source.ProductCaseId,
				UnitSalePrice = source.UnitSalePrice,
				ProductType = source.ProductType,
				Units = source.Units,
				MainParentId = source.MainParentId,
				RemainingQuantity = source.RemainingQuantity
				

			};
			return newprod;
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
                                ProductType = Convert.ToString(objDataRow["ProductType"].ToString()),
                                PurchasePrice = Convert.ToDecimal(objDataRow["Purchase Price"].ToString()),
                                SalePrice = Convert.ToDecimal(objDataRow["Sale Price"].ToString()),
                                ProductDescription = Convert.ToString(objDataRow["Product Description"].ToString()),
                                WarehouseId = Convert.ToInt16(objDataRow["Warehouse Id"].ToString()),
                                RemainingQuantity = Convert.ToDecimal(objDataRow["RemainingQuantity"].ToString()),
                                NumOfSinglesInCase = Convert.ToInt32(objDataRow["Number of singles in case"].ToString()),
								UnitSalePrice = Convert.ToDecimal(objDataRow["UnitSalePrice"].ToString()),

								Units = Convert.ToInt32(objDataRow["Units in case"].ToString()),
								MainParentId = Convert.ToInt32(objDataRow["MainParentId"].ToString()),
								ProductCaseId = Convert.ToInt32(objDataRow["ProductCaseId"].ToString()),
								// add spefic parameters for the product model
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