using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShopMate.Migrations;
using ShopMate.Models;
using WebErrorLogging.Utilities;

namespace ShopMate.Controllers
{
	public class AppController : ApiController
	{
		private SIContext db = new SIContext();
		private string userId = Env.GetUserInfo("name");

		[Route("api/App/test")]
		[HttpGet, ActionName("test")]
		public async Task<HttpResponseMessage> test()
		{
			//string[] emails = { "trynosmuch@gmail.com", "ngonidzashe@zimhope.co.zw" };
			//var body = File.ReadAllText(HttpContext.Current.Server.MapPath("/Views/Mail/vancreate.mail.htm"));
			//body = string.Format(body, "New Van Sell : CF85-1");

			var message = new MailMessage();
			message.To.Add(new MailAddress("trynosmuch@gmail.com"));
			message.Subject = "New Van Sell";
			message.Body = "Ndiripo";
			//  System.Diagnostics.Debug.WriteLine("Email : " + email);

			message.IsBodyHtml = true;
			using (var smtp = new SmtpClient())
			{
				await smtp.SendMailAsync(message);
			}

			return Request.CreateResponse(HttpStatusCode.OK, "Email Sent : ");
		}

		//[Route("api/App/getRates")]
		//[HttpGet, ActionName("getRates")]
		//public HttpResponseMessage getRates()
		//{
		//    var warehouseId = db.Users.FirstOrDefault(n => n.UserName == userId).WarehouseId;
		//    var paymentMethods = db.Currencies.Where(i => i.Name.ToLower() != "usd").OrderBy(t => t.Name);
		//    List<string> listRates = new List<string>();
		//    List<string> listPays = new List<string>();
		//    //userId = "Sale2";

		//    foreach (var pay in paymentMethods.ToList())
		//    {
		//        double gonzo = 1 * Env.GetRate(pay.Name.ToLower(), warehouseId);
		//        if (gonzo != 0.000147)
		//        {
		//            listRates.Add(gonzo.ToString());
		//            listPays.Add(pay.Name);
		//        }
		//    }

		//    //string[] paymentMethodsRates = listRates.ToArray();
		//    //// string[] payMethod = listPays.ToArray();

		//    //string[] payMethod = { "Cash", "Zipit", "Ecocash" };

		//    string[] paymentMethodsRates = listRates.ToArray();
		//    string[] payMethod = listPays.ToArray();
		//    //string[] paymentMethodsRates = { "1500", "1500", "1500", "1500", "1500" };

		//    return Request.CreateResponse(
		//        HttpStatusCode.OK,
		//        new
		//        {
		//            rates = paymentMethodsRates,
		//            paymethods = payMethod
		//        },
		//        JsonMediaTypeFormatter.DefaultMediaType);
		//}
		[Route("api/App/getRates")]
		[HttpGet, ActionName("getRates")]
		public HttpResponseMessage getRates(int userWarehouse)
		{

			// userId = "Life";
			var warehouseId = userWarehouse;
			var paymentMethods = db.Currencies.Where(i => i.Name.ToLower() != "usd" && i.WarehouseId == userWarehouse).OrderBy(t => t.Name);
			List<string> listRates = new List<string>();
			List<string> listPays = new List<string>();

			foreach (var pay in paymentMethods.ToList())
			{
				double gonzo = 1 * Env.GetRate1(pay.Name.ToLower(), warehouseId);
				if (gonzo != 0.000147)
				{
					listRates.Add(gonzo.ToString());
					listPays.Add(pay.Name);
				}
			}

			string[] paymentMethodsRates = listRates.ToArray();
			string[] payMethod = listPays.ToArray();

			//        string[] payMethod = { "Cash", "Ecocash", "Zipit" };
			//string[] paymentMethodsRates = { "1500", "1500", "1500", "1500", "1500" };

			return Request.CreateResponse(
		   HttpStatusCode.OK,
		   new
		   {
			   rates = paymentMethodsRates,
			   paymethods = payMethod
		   },
		   JsonMediaTypeFormatter.DefaultMediaType);
		}
		//[Route("api/App/login")]
		//[HttpPost, ActionName("login")]
		//public HttpResponseMessage login([FromBody] JObject value)
		//{
		//    try
		//    {
		//        string email = value["email"].ToString();
		//        string password = value["password"].ToString();

		//        User login = db.Users.FirstOrDefault(i => i.UserName == email && i.CanLogin == true);
		//        //  login.JoinDate
		//        DateTime dateOfJoining = (DateTime)login.JoinDate; // Example

		//        // Calculate time difference
		//        TimeSpan timeDifference = DateTime.Now - dateOfJoining;

		//        // Check if one year has passed
		//        if (timeDifference.TotalDays >= 365)
		//        {
		//            ModelState.AddModelError(string.Empty, "You are not allowed to log in as one year has passed since your date of joining.");
		//            //    ViewBag.Msg = "Your Account Expired, Contact 0783 284 440";
		//            return Request.CreateResponse(HttpStatusCode.Forbidden, "Your Account Expired, Contact 0783 284 440");
		//        }
		//        try
		//        {
		//            if (BCrypt.Net.BCrypt.Verify(password, login.Password))
		//            {
		//                if (login.RoleId == 2 || login.RoleId == 7)
		//                {
		//                    var shopdetails = db.Warehouses.FirstOrDefault(i => i.Id == login.WarehouseId);

		//                    var wareId = db.Users.FirstOrDefault(n => n.UserName == email).WarehouseId;

		//                    var paymentMethods = db.Currencies.Where(i => i.Name.ToLower() != "usd").OrderBy(t => t.Name).ToArray();
		//                    List<string> listRates = new List<string>();
		//                    List<string> listPays = new List<string>();

		//                    userId = email;
		//                    foreach (var pay in paymentMethods.ToList())
		//                    {
		//                        double gonzo = 1 * Env.GetRate(pay.Name.ToLower(), login.WarehouseId);
		//                        if (gonzo != 0.000147)
		//                        {
		//                            listRates.Add(gonzo.ToString());
		//                            listPays.Add(pay.Name);
		//                        }
		//                    }

		//                   // string[] payMethod = { "Cash", "Zipit", "Ecocash" };
		//                    string[] paymentMethodsRates = listRates.ToArray();
		//                      string[] payMethod = listPays.ToArray();

		//                    var rowCount = new
		//                    {
		//                        user = new
		//                        {
		//                            id = login.Id,
		//                            name = login.FullName.ToString(),
		//                            warehouse = login.WarehouseId,
		//                            storeName = shopdetails.Name.ToString(),
		//                            storAddress = shopdetails.Address.ToString(),
		//                            storeContact = shopdetails.Mobile.ToString(),
		//                            paymentMethods = JsonConvert.SerializeObject(paymentMethods),
		//                            paymentMethodsRates = JsonConvert.SerializeObject(paymentMethodsRates),
		//                            roleId = login.RoleId,
		//                        }
		//                    };

		//                    //var user = new string[] {
		//                    //    login.Id.ToString(),
		//                    //    login.FullName.ToString(),
		//                    //    login.WarehouseId.ToString()

		//                    //};
		//                    return Request.CreateResponse(
		//                        HttpStatusCode.OK,
		//                        rowCount,
		//                        JsonMediaTypeFormatter.DefaultMediaType);
		//                    //return Request.CreateResponse(HttpStatusCode.OK, userApp.ToString());
		//                }
		//                else
		//                {
		//                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Access denied you are unauthorized to access this platform");
		//                }
		//            }
		//            else
		//            {
		//                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
		//            }
		//        }
		//        catch (InvalidOperationException ex)
		//        {
		//            System.Diagnostics.Debug.WriteLine("Test1 : " + ex.Message.ToString());

		//            Helper.WriteError(ex, ex.Message);
		//            return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
		//        }
		//        catch (Exception ex)
		//        {
		//            System.Diagnostics.Debug.WriteLine("Test1 : " + ex.Message.ToString());
		//            Helper.WriteError(ex, ex.Message);
		//            return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        System.Diagnostics.Debug.WriteLine("Test1 : " + ex.Message.ToString());
		//        Helper.WriteError(ex, ex.Message);
		//        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
		//    }
		//}


		[Route("api/App/login")]
		[HttpPost, ActionName("login")]
		public HttpResponseMessage Login([FromBody] JObject value)
		{


			try
			{
				string email = value["email"].ToString();
				string password = value["password"].ToString();
				Console.WriteLine("About to login..." + email, password);
				User login = db.Users.FirstOrDefault(i => i.UserName == email && i.CanLogin == true);
				//  login.JoinDate
				DateTime dateOfJoining = (DateTime)login.JoinDate; // Example

				// Calculate time difference
				TimeSpan timeDifference = DateTime.Now - dateOfJoining;

				// Check if one year has passed
				if (timeDifference.TotalDays >= 365)
				{
					ModelState.AddModelError(string.Empty, "You are not allowed to log in as one year has passed since your date of joining.");
					//    ViewBag.Msg = "Your Account Expired, Contact 0783 284 440";
					return Request.CreateResponse(HttpStatusCode.Forbidden, "Your Account Expired, Contact 0783 284 440");
				}


				try
				{
					if (BCrypt.Net.BCrypt.Verify(password, login.Password))
					{
						if (login.RoleId == 2 || login.RoleId == 7)
						{
							var shopdetails = db.Warehouses.FirstOrDefault(i => i.Id == login.WarehouseId);
							var taxpayer = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == login.WarehouseId);
							var paymentMethods = db.Currencies.Where(i => i.Name.ToLower() != "usd" && i.WarehouseId == login.WarehouseId).OrderBy(t => t.Name).ToArray();
							List<string> listRates = new List<string>();
							List<string> listPays = new List<string>();

							foreach (var pay in paymentMethods.ToList())
							{
								double gonzo = 1 * Env.GetRate1(pay.Name.ToLower(), login.WarehouseId);
								if (gonzo != 0.000147)
								{
									listRates.Add(gonzo.ToString());
									listPays.Add(pay.Name);
								}
							}
							var valadation = db.Sales.Where(k => k.WarehouseId == login.WarehouseId && k.AddedBy == login.Id).Count();
							var sales = 0;
							if (valadation != 0)
							{
								sales = db.Sales
												   .Where(x => x.WarehouseId == login.WarehouseId && x.AddedBy == login.Id)
												   .OrderByDescending(x => x.DateAdded)
												   .First().recieptNumber;
							}
							int recieptNumber = sales + 1;
							string[] paymentMethodsRates = listRates.ToArray();
							string[] payMethod = listPays.ToArray();

							var rowCount = new
							{
								user = new
								{
									id = login.Id,
									name = login.FullName.ToString(),
									warehouse = login.WarehouseId,
									storeName = shopdetails.Name.ToString(),
									storAddress = shopdetails.Address.ToString(),
									storeContact = shopdetails.Mobile.ToString(),
									paymentMethods = JsonConvert.SerializeObject(paymentMethods),
									paymentMethodsRates = JsonConvert.SerializeObject(paymentMethodsRates),
									roleId = login.RoleId,
									ReceiptNumber = recieptNumber,
									Negative = taxpayer.AllowNegative1,
									ShowStocks = taxpayer.ShowQuantity,
								}
							};

							//var user = new string[] {
							//    login.Id.ToString(),
							//    login.FullName.ToString(),
							//    login.WarehouseId.ToString()

							//};
							return Request.CreateResponse(
								HttpStatusCode.OK,
								rowCount,
								JsonMediaTypeFormatter.DefaultMediaType);
							//return Request.CreateResponse(HttpStatusCode.OK, userApp.ToString());
						}


						else
						{
							return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Access denied you are unauthorized to access this platform");
						}
					}
					else
					{
						return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
					}
				}
				catch (InvalidOperationException ex)
				{
					System.Diagnostics.Debug.WriteLine("Test1 : " + ex.Message.ToString());
					Console.WriteLine("Error..." + ex.Message);
					Helper.WriteError(ex, ex.Message);
					return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error..." + ex.Message);
					System.Diagnostics.Debug.WriteLine("Test1 : " + ex.Message.ToString());
					Helper.WriteError(ex, ex.Message);
					return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error..." + ex.Message);
				System.Diagnostics.Debug.WriteLine("Test1 : " + ex.Message.ToString());
				Helper.WriteError(ex, ex.Message);
				return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
			}
		}

		[HttpGet, ActionName("getProducts")]
		public HttpResponseMessage getProducts(int userWarehouse)

		{
			try
			{
				System.Diagnostics.Debug.WriteLine("Test1 : " + userWarehouse);
				var query = db.WarehouseStocks
	.Where(sd => sd.WarehouseId == userWarehouse)
	.Join(db.Products.Where(p => p.IsActive == true),
		sd => sd.ProductId,
		pd => pd.Id,
		(sd, pd) => new { sd, pd })
	.GroupJoin(db.WarehouseStocks.Where(p=>p.RemainingQuantity > 0),
		temp => temp.pd.MainParentId,
		sdP => sdP.ProductId,
		(temp, parentStockGroup) => new { temp.sd, temp.pd, parentStockGroup })
	.SelectMany(x => x.parentStockGroup.DefaultIfEmpty(),
		(x, sdP) => new { x.sd, x.pd, sdP })
	 .GroupJoin(db.WarehouseStocks.Where(p => p.RemainingQuantity > 0),
		temp => temp.pd.ProductCaseId,
		sdP => sdP.ProductId,
		(temp, parentStockGroup2) => new { temp.sd, temp.pd, temp.sdP, parentStockGroup2 })
	.SelectMany(x => x.parentStockGroup2.DefaultIfEmpty(),
		(x, sdPc) => new { x.sd, x.pd, x.sdP, parentStockCase = sdPc })
	.GroupJoin(db.Taxs,
		x => x.pd.TaxId,
		tax => tax.Id,
		(x, taxGroup) => new { x.sd, x.pd, x.sdP, taxGroup })
	.SelectMany(x => x.taxGroup.DefaultIfEmpty(),
		(x, tax) => new { x.sd, x.pd, x.sdP, tax })
	.OrderBy(x => x.pd.Name)
	.Select(x => new
	{
		id = x.pd.Id,
		name = x.pd.Name,
		price = x.pd.SalePrice,
		image = x.pd.ProductImage,
		tax = x.tax != null ? x.tax.TaxRate : 0,
		barcode = x.pd.BarCode,
		productType = x.pd.ProductType,
		quantity = x.sdP != null && x.sdP.RemainingQuantity > 0 && x.sdP.ProductId != x.pd.Id ? Math.Ceiling((x.sdP.RemainingQuantity * x.sdP.Product_ProductId.Units)/ x.pd.NumOfSinglesInCase) +x.pd.RemainingQuantity: "CASE".Equals(x.pd.ProductType) ? 10000000000000 :  x.sd.RemainingQuantity,
		remainingSinglesQuantity = 0,
		remainingQuantity = x.sdP != null && x.sdP.RemainingQuantity > 0 && x.sdP.ProductId != x.pd.Id ? Math.Ceiling((x.sdP.RemainingQuantity * x.sdP.Product_ProductId.Units)/ x.pd.NumOfSinglesInCase) + x.pd.RemainingQuantity : "CASE".Equals(x.pd.ProductType) ? 10000000000000 : x.sd.RemainingQuantity,
		unitSalePrice = x.pd.UnitSalePrice,
		numOfSinglesInCase = x.pd.NumOfSinglesInCase
	});

				var res = query.ToList();

				System.Diagnostics.Debug.WriteLine("Test1 : " + userWarehouse);

				//return Request.CreateResponse(HttpStatusCode.OK, res);

				if (res.ToArray().Length != 0)
				{
					return Request.CreateResponse(
					HttpStatusCode.OK,
					res.ToList(),
					JsonMediaTypeFormatter.DefaultMediaType);
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.NotFound, "Product not found , please try again");
				}
			}catch(Exception ex){
				Helper.WriteError(ex, "Error in sell method: " + ex.Message);
				Console.WriteLine(ex.InnerException.ToString());
				return Request.CreateResponse(HttpStatusCode.InternalServerError,
					new { error = "Transaction failed", message = ex.ToString() },
					JsonMediaTypeFormatter.DefaultMediaType);
			}

			//return Request.CreateResponse<IEnumerable<string[]>>(HttpStatusCode.OK, res);
		}

		[HttpGet, ActionName("searchProduct")]
		public HttpResponseMessage searchProduct(int userWarehouse, string barcode)
		{
			System.Diagnostics.Debug.WriteLine("Test1 : " + barcode);

			var stockdata = db.WarehouseStocks.Where(i => i.WarehouseId == userWarehouse);

			var res = from sd in stockdata.ToList()
					  join pd in db.Products on sd.ProductId equals pd.Id
					  where pd.BarCode == barcode
					  where pd.IsActive == true
					  orderby pd.Name
					  select new
					  {
						  id = pd.Id,
						  name = pd.Name,
						  price = pd.SalePrice,
						  //   priceRTGS = pd.RtgsPrice,
						  image = pd.ProductImage,
						  tax = db.Taxs.FirstOrDefault(i => i.Id == pd.TaxId).TaxRate,
						  barcode = pd.BarCode,
						  quantity = sd.RemainingQuantity
					  };

			if (res.ToArray().Length != 0)
			{
				return Request.CreateResponse(
				HttpStatusCode.OK,
				res.ToList().Single(),
				JsonMediaTypeFormatter.DefaultMediaType);
			}
			else
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, "Product not found , please try again");
			}
		}

		[Route("api/App/sell")]
		[HttpPost, ActionName("sell")]
		public HttpResponseMessage Sell([FromBody] JObject sell)
		{
			Helper.WriteDebug(new Exception(), sell["sell"].ToString());
			var test = false;
			var result = new { trynos = "", sellsCount = 0, duplicatesCount = 0, duplicatesList = "" };

			string value = sell["sell"].ToString();
			List<MySell> maSells = JsonConvert.DeserializeObject<List<MySell>>(value);
			var csello = new JArray();
			var duplicates = new JArray();
			var sellCount = new JArray();

			// Start a database transaction
			using (var transaction = db.Database.BeginTransaction())
			{
				try
				{
					foreach (MySell mySell in maSells)
					{
						csello.Add(mySell.paymentMethod);
						var isUsd = mySell.currency.Equals("USD");
						var _rate = mySell.rate;
						if (!test)
						{
							// Validate seller exists
							User seller_user = db.Users.FirstOrDefault(i => i.Id == mySell.userId) ?? throw new Exception("User not found..." + mySell.userId);
							Sale ObjSale = new Sale();

							foreach (var item in mySell.products)
							{
								sellCount.Add(item.prodId);
								var selectedProduct = db.Products.Where(i => i.Id == item.prodId).FirstOrDefault() ?? throw new Exception("Product not found..." + item.prodId);
								var productIsCase = "CASE".Equals(selectedProduct.ProductType); // check if product is case

								int singlesQuantity = 0;

								decimal quantity = item.quantity;

								if (productIsCase)
								{ // if case look for singles
									var quntities = item.quantity.ToString().Split('.');
									if (quntities.Length == 2)
									{ 
										singlesQuantity = int.Parse(quntities[1]);
										if (singlesQuantity >= selectedProduct.NumOfSinglesInCase)
										{
											throw new Exception("Number of singles must be less than :" + selectedProduct.NumOfSinglesInCase);
										}
										quantity = decimal.Parse(quntities[0]);
									}
								}
								var ObjWarehouseStock = db.WarehouseStocks.Where(i => i.ProductId == item.prodId && i.WarehouseId == seller_user.WarehouseId).FirstOrDefault() ?? throw new Exception("Warehouse stock not found...Product Id:" + item.prodId + ", Warehouse Id:" + seller_user.WarehouseId);
								// Check for sufficient stock before proceeding
								if (ObjWarehouseStock.RemainingQuantity < quantity)
								{
									if ("CASE".Equals(selectedProduct.ProductType))
									{
										// calculate units required
										var unitsRequired = (selectedProduct.Units * quantity) + ((selectedProduct.Units/selectedProduct.NumOfSinglesInCase)*singlesQuantity);
										Console.WriteLine("Required units..." + unitsRequired);
										//process required units... by borrowing start with imme parent
										var productStock = db.WarehouseStocks
														 .Where(p => (p.Product_ProductId.Id == selectedProduct.MainParentId ||
														 p.Product_ProductId.MainParentId == selectedProduct.MainParentId))
														 .Distinct().OrderByDescending(p => p.Product_ProductId.Id)
														 .ToList();
										Console.WriteLine($"Products stock fetched. Count: {productStock.Count}");
									    WarehouseStock firstToBeDeductedStock = null;
										//now get quantities...
										foreach (var wh in productStock)
										{
											var warehouseProduct = wh.Product_ProductId;
											var warehouseUnitsFromCases = (warehouseProduct.Units * wh.RemainingQuantity);
											//var warehouseUnitsFromSingles = ((warehouseProduct.Units / warehouseProduct.NumOfSinglesInCase) * wh.RemainingSinglesQuantity);
											//int warehouseUnits = (int)warehouseUnitsFromCases + warehouseUnitsFromSingles;
											int warehouseUnits = (int)warehouseUnitsFromCases;

											if (warehouseUnits < unitsRequired || warehouseProduct.Id > selectedProduct.Id)
											{ // nothing to deduct from
												continue;
											}
											firstToBeDeductedStock = wh;
											//how many cases or units required from this product
											var casesRequired = Math.Ceiling(unitsRequired / warehouseProduct.Units);
											//var singlesRequired = Math.Ceiling((unitsRequired / warehouseProduct.Units)/warehouseProduct.NumOfSinglesInCase);
											Console.WriteLine($"Warehouse : {casesRequired}");
											wh.RemainingQuantity -= casesRequired;
											
											db.Entry(wh).State = EntityState.Modified;

											// Deduct stock using order...
											WarehouseStock lastImmeProductStock = wh;
											var productStockToDeduct = productStock
											.Where(p=>p.ProductId > warehouseProduct.Id)
												.OrderBy(p => p.Product_ProductId.Id).ToList();
											Console.WriteLine($"..... {productStockToDeduct.Count}");
											
											foreach (var uwh in productStockToDeduct)
											{
												var immeChild = db.Products.FirstOrDefault(p => p.Id == uwh.ProductId);

												if (immeChild == null) continue;
												Console.WriteLine($"Trace Product...{immeChild.Name}");
												var lastImmeProduct = lastImmeProductStock.Product_ProductId;
												casesRequired = Math.Ceiling(unitsRequired / lastImmeProduct.Units);
												Console.WriteLine($"Deduct... {immeChild.Name}");
												//remove case from current
												if (firstToBeDeductedStock != null && firstToBeDeductedStock.Id != uwh.Id)
												{
													if (wh.Id != lastImmeProductStock.Id)
													{
														lastImmeProductStock.RemainingQuantity -= casesRequired;
														lastImmeProduct.RemainingQuantity = lastImmeProductStock.RemainingQuantity;
													}
												}
												if (immeChild.Id == selectedProduct.Id)
												{
													break;
												}
												else if (immeChild.Id > warehouseProduct.Id)
												{
													//now add cases to the next product stock 
													var _providedUnits = lastImmeProduct.Units * casesRequired;
													var _requiredTargetCases = Math.Ceiling(_providedUnits / immeChild.Units);
													uwh.RemainingQuantity += _requiredTargetCases;
													db.Entry(uwh).State = EntityState.Modified;
													immeChild.RemainingQuantity = uwh.RemainingQuantity;
													db.Entry(immeChild).State = EntityState.Modified;
													lastImmeProductStock = uwh;
													db.SaveChanges();
												}
											}

											// add quantity to a target warehouse
											var productToBeDeductedFrom = lastImmeProductStock.Product_ProductId;
											var providedUnits = (productToBeDeductedFrom.Units * casesRequired);
											var requiredTargetCases = Math.Ceiling(providedUnits / selectedProduct.Units);
											ObjWarehouseStock.RemainingQuantity += requiredTargetCases;
											db.Entry(ObjWarehouseStock).State = EntityState.Modified;
											db.SaveChanges();
											break;

										}
										Console.WriteLine($"Products stock fetched. Count: {productStock.Count}");
									}
									else
									{
										throw new Exception("Insufficient stock..." + item.prodId + "  Product Name...:" + selectedProduct.Name + ",  Remaining Quantity:" + ObjWarehouseStock.RemainingQuantity + ", Quantity:" + item.quantity);
									}
									
								}
								DateTime nowDate;
								try
								{
									nowDate = DateTime.ParseExact(mySell.date + " " + mySell.time, "dd/MM/yyyy HH:mm:ss", null);
								}
								catch
								{
									nowDate = DateTime.Now;
								}
								//cal amounts
								var unitSalePrice = isUsd ? selectedProduct.UnitSalePrice : selectedProduct.UnitSalePrice * _rate;
								var totalAmountWithTax = (item.price * quantity) + (unitSalePrice * singlesQuantity);
								var purchasePrice = isUsd ? selectedProduct.PurchasePrice : selectedProduct.PurchasePrice * _rate;
								var totalPurchaseAmount = (purchasePrice * quantity) + ((purchasePrice / selectedProduct.NumOfSinglesInCase) * singlesQuantity);


								decimal taxAmount = 0;
								decimal taxPurchaseAmount = 0;
								var tax = db.Taxs.FirstOrDefault(i => i.Id == selectedProduct.TaxId);
								if (tax != null)
								{
									taxAmount = totalAmountWithTax * tax.TaxRate / 100;
									taxPurchaseAmount = (totalPurchaseAmount * tax.TaxRate / 100);
								}

								if (mySell.currency == "USD")
								{
									ObjSale.ProductId = item.prodId;
									ObjSale.Quantity = quantity;
									ObjSale.SalePrice = isUsd ? selectedProduct.SalePrice : selectedProduct.SalePrice * _rate;
									ObjSale.UnitSalePrice = isUsd ? selectedProduct.UnitSalePrice : selectedProduct.UnitSalePrice * _rate;
									ObjSale.Singles = singlesQuantity;
									ObjSale.TotalAmountWithTax = totalAmountWithTax;
									if (selectedProduct.TaxId != 2)
									{
										ObjSale.TotalAmount = ObjSale.TotalAmountWithTax;
									}
									else
									{
										ObjSale.TotalAmount = ObjSale.TotalAmountWithTax - taxAmount;
									}

									ObjSale.WarehouseId = (int)seller_user.WarehouseId;
									ObjSale.AddedBy = seller_user.Id;
									ObjSale.CustomerUserId = 29611;
									ObjSale.DateAdded = nowDate;
									ObjSale.DateModied = nowDate;
									ObjSale.ModifiedBy = seller_user.Id;
									ObjSale.PaidAmount = totalAmountWithTax;

									var paymentMode = db.PaymentModes.FirstOrDefault(i => i.Name == mySell.currency) ?? throw new Exception("Payment mode not found..." + mySell.currency);
									ObjSale.PaymentModeId = paymentMode.Id;
									ObjSale.InventoryTypeId = 2;
									ObjSale.isFormalSale = false;
								}
								else
								{
									var mypayment = db.PaymentModes.FirstOrDefault(i => i.Name == mySell.paymentMethod) ?? throw new Exception("Payment method not found..." + mySell.paymentMethod);
									var mycurrency = db.Currencies.FirstOrDefault(i => i.Name == mypayment.Name) ?? throw new Exception("Currency not found..." + mypayment.Name);
									var priceRate = db.Rates.Where(i => i.CurrencyId == mycurrency.Id)
										.OrderByDescending(i => i.DateModified).FirstOrDefault();

									decimal rate = priceRate != null ? (decimal)priceRate.CurrencyRate : 1;

									ObjSale.ProductId = item.prodId;
									ObjSale.Quantity = quantity;
									ObjSale.SalePrice = isUsd ? selectedProduct.SalePrice : selectedProduct.SalePrice * _rate; ;
									ObjSale.UnitSalePrice = isUsd ? selectedProduct.UnitSalePrice : selectedProduct.UnitSalePrice * _rate;
									ObjSale.Singles = singlesQuantity;
									ObjSale.TotalAmountWithTax = totalAmountWithTax;

									if (selectedProduct.TaxId != 2)
									{
										ObjSale.TotalAmount = ObjSale.TotalAmountWithTax;
									}
									else
									{
										ObjSale.TotalAmount = ObjSale.TotalAmountWithTax - taxAmount;
									}

									ObjSale.WarehouseId = (int)seller_user.WarehouseId;
									ObjSale.AddedBy = seller_user.Id;
									ObjSale.CustomerUserId = 29611;
									ObjSale.DateAdded = nowDate;
									ObjSale.DateModied = nowDate;
									ObjSale.ModifiedBy = seller_user.Id;
									ObjSale.PaidAmount = totalAmountWithTax;
									ObjSale.PaymentModeId = mypayment.Id;
									ObjSale.InventoryTypeId = 2;
									ObjSale.rtgs = ObjSale.TotalAmountWithTax * rate;
									ObjSale.isFormalSale = false;
								}

								ObjSale.customerName = mySell.customer;
								ObjSale.recieptNumber = mySell.invoiceId;
								ObjSale.Currency = mySell.currency;

								// Add Sale
								db.Sales.Add(ObjSale);
								db.SaveChanges();

								if (productIsCase)
								{
									int currentSingles = ObjWarehouseStock.RemainingSinglesQuantity;
									int remainingSinglesAfter = currentSingles - singlesQuantity;

									WarehouseStock childStock = selectedProduct.Units > 0 ? db.WarehouseStocks.Where(p=>p.Product_ProductId.ProductCaseId == selectedProduct.Id).FirstOrDefault() : null;
									Product childProduct = childStock != null && selectedProduct.Units > 0 ? childStock.Product_ProductId : null;
									// Reduce full case count
									ObjWarehouseStock.RemainingQuantity -= quantity;

									// Update loose singles based on the computed remaining amount
									if (currentSingles == 0 && remainingSinglesAfter == 0)
									{
									    if(childStock != null && singlesQuantity > 0){
											childStock.RemainingQuantity = 0;
										}
										//ObjWarehouseStock.RemainingSinglesQuantity = 0;
									}

									else if (remainingSinglesAfter < 0)
									{
										// Borrow a full case to cover the shortage
										if(childStock != null && singlesQuantity > 0)
											childStock.RemainingQuantity = (int)(selectedProduct.NumOfSinglesInCase + remainingSinglesAfter);
										//ObjWarehouseStock.RemainingSinglesQuantity = (int)(selectedProduct.NumOfSinglesInCase + remainingSinglesAfter);
										ObjWarehouseStock.RemainingQuantity--;   // one extra case broken open
									}
									else if (remainingSinglesAfter > 0)
									{
										// Only update if the remaining singles meet or exceed the singlesQuantity threshold
										if (currentSingles == singlesQuantity)
											//ObjWarehouseStock.RemainingSinglesQuantity = 0;
											if (childStock != null)
												childStock.RemainingQuantity = 0;
										else if (currentSingles > singlesQuantity && singlesQuantity > 0)
												childStock.RemainingQuantity = remainingSinglesAfter;
										// If 0 < remainingSinglesAfter < singlesQuantity, leave the value unchanged (original behavior)

									}
									else if (remainingSinglesAfter == 0)
									{
									 	if (childStock != null && singlesQuantity > 0)
											childStock.RemainingQuantity = 0;
									}
									// The original 'else if (singlesQuantity == 0)' branch was unreachable and has been removed
									if (childStock != null)
										db.Entry(childStock).State = EntityState.Modified;
									if(childProduct != null){
										childProduct.RemainingQuantity = childStock.RemainingQuantity;
										db.Entry(childProduct).State = EntityState.Modified;
									}
									db.SaveChanges();
								}
								else
								{
									// Non‑case product: treat RemainingQuantity as the count of individual items
									ObjWarehouseStock.RemainingQuantity -= quantity;
									ObjWarehouseStock.RemainingSinglesQuantity = 0;
									
								}

								//if (ObjWarehouseStock.RemainingQuantity < 0) throw new Exception("Insufficient stock");

								db.Entry(ObjWarehouseStock).State = EntityState.Modified;

								selectedProduct.RemainingQuantity = ObjWarehouseStock.RemainingQuantity;

								db.Entry(selectedProduct).State = EntityState.Modified;
								// Create product stock record
								ProductStock ps = new ProductStock
								{
									ProductId = ObjSale.ProductId,
									Quantity = ObjSale.Quantity,
									PurchasePrice = selectedProduct.PurchasePrice,
									TotalPurchaseAmount = totalPurchaseAmount,
									SalePrice = ObjSale.SalePrice,
									Discount = selectedProduct.Discount ?? 0,
									TotalSaleAmount = totalAmountWithTax
								};
								decimal TaxAmount = 0;
								ps.TotalSaleAmountWithTax = totalAmountWithTax;
								ps.TaxAmount = TaxAmount;
								ps.Profit = ps.TotalSaleAmount - ps.TotalPurchaseAmount;
								ps.ProfitWithTax = ps.TotalSaleAmount - ps.TotalPurchaseAmount;
								ps.Description = "SaleNote";
								ps.AddedBy = seller_user.Id;
								ps.DateAdded = nowDate;
								ps.ModifiedBy = seller_user.Id;
								ps.DateModied = DateTime.Now;
								ps.InventoryTypeId = 2;
								ps.WarehouseId = (int)seller_user.WarehouseId;
								ps.IsFormal = false;
								ps.OtherTaxValue = 0; // Will be set after saving to get ObjSale.Id
								ps.RemainingQuantity = ObjWarehouseStock.RemainingQuantity;

								db.ProductStocks.Add(ps);
								db.SaveChanges();
							}
						}
					}

					// Save all changes once
					try
					{
						db.SaveChanges();
					}
					catch (DbUpdateException dbEx)
					{
						// EF-specific exception with detailed inner SQL errors
						var inner = dbEx.InnerException?.InnerException?.Message ?? dbEx.InnerException?.Message ?? dbEx.Message;
						throw new Exception($"SaveChanges failed: {inner}", dbEx);
					}

					// Update OtherTaxValue with Sale Id for ProductStock records
					// This needs to be done after SaveChanges to get the generated Sale Ids
					var sales = db.Sales.OrderByDescending(s => s.Id).Take(maSells.Sum(m => m.products.Count)).ToList();
					var productStocks = db.ProductStocks.OrderByDescending(p => p.Id).Take(maSells.Sum(m => m.products.Count)).ToList();

					for (int i = 0; i < productStocks.Count && i < sales.Count; i++)
					{
						productStocks[i].OtherTaxValue = sales[i].Id;
					}
					try
					{
						db.SaveChanges();
					}
					catch (DbUpdateException dbEx)
					{
						// EF-specific exception with detailed inner SQL errors
						var inner = dbEx.InnerException?.InnerException?.Message ?? dbEx.InnerException?.Message ?? dbEx.Message;
						throw new Exception($"SaveChanges failed: {inner}", dbEx);
					}

					// Commit the transaction
					transaction.Commit();

					result = new
					{
						trynos = csello.ToString(),
						sellsCount = sellCount.Count(),
						duplicatesCount = duplicates.Count(),
						duplicatesList = duplicates.ToString()
					};

					return Request.CreateResponse(HttpStatusCode.OK, result, JsonMediaTypeFormatter.DefaultMediaType);
				}
				catch (Exception ex)
				{
					// Rollback transaction on error
					transaction.Rollback();
					Helper.WriteError(ex, "Error in sell method: " + ex.Message);
					//Console.WriteLine(ex.InnerException.ToString());
					return Request.CreateResponse(HttpStatusCode.InternalServerError,
						new { error = "Transaction failed", message = ex.ToString() },
						JsonMediaTypeFormatter.DefaultMediaType);
				}
			}
		}
	}
}