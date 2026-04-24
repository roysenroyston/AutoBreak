using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShopMate.Models;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using WebErrorLogging.Utilities;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Net.Mail;

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
        public HttpResponseMessage login([FromBody] JObject value)
        {


            try
            {
                string email = value["email"].ToString();
                string password = value["password"].ToString();

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

                    Helper.WriteError(ex, ex.Message);
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Test1 : " + ex.Message.ToString());
                    Helper.WriteError(ex, ex.Message);
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Test1 : " + ex.Message.ToString());
                Helper.WriteError(ex, ex.Message);
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid details please try again");
            }
        }

        [HttpGet, ActionName("getProducts")]
        public HttpResponseMessage getProducts(int userWarehouse)

        {
            System.Diagnostics.Debug.WriteLine("Test1 : " + userWarehouse);
            var stockdata = db.WarehouseStocks.Where(i => i.WarehouseId == userWarehouse);
      

            var res = from sd in stockdata.ToList()
                      join pd in db.Products on sd.ProductId equals pd.Id
                      where pd.IsActive == true
                      orderby pd.Name
                      select new
                      {
                          id = pd.Id,
                          name = pd.Name,
                          price = pd.SalePrice,
                          //priceRTGS = pd.RtgsPrice,
                          image = pd.ProductImage,
                          tax = db.Taxs.FirstOrDefault(i => i.Id == pd.TaxId).TaxRate,
                          barcode = pd.BarCode,
                          quantity = sd.RemainingQuantity
                      };



            //if (userWarehouse==8)
            //{
            //    //var roysen = db.WarehouseStocks.Where(m => m.Product_ProductId.ProductType == "case".ToLower() && m.RemainingQuantity > 0 && m.WarehouseId == userWarehouse);
            //    //if (roysen != null)
            //    //    if (stockdata = 2)
            //    //    {
            //    //    }
            //  var  res2 = from sd in stockdata.Where(k=>k.RemainingQuantity > 0 && k.Product_ProductId.ProductType=="CASE").ToList()
            //          join pd in db.Products on sd.ProductId equals pd.Id
            //          where pd.IsActive == true
                      
            //          orderby pd.Name
            //          select new
            //          {
            //              id = pd.Id,
            //              name = pd.Name,
            //              price = pd.SalePrice,
            //              //priceRTGS = pd.RtgsPrice,
            //              image = pd.ProductImage,
            //              tax = db.Taxs.FirstOrDefault(i => i.Id == pd.TaxId).TaxRate,
            //              barcode = pd.BarCode,
            //              quantity = sd.RemainingQuantity
            //          };
            // var res3 = from sd in stockdata.Where(k =>  k.Product_ProductId.ProductType == "SINGLE").ToList()
            //           join pd in db.Products on sd.ProductId equals pd.Id
            //           where pd.IsActive == true

            //           orderby pd.Name
            //           select new
            //           {
            //               id = pd.Id,
            //               name = pd.Name,
            //               price = pd.SalePrice,
            //               //priceRTGS = pd.RtgsPrice,
            //               image = pd.ProductImage,
            //               tax = db.Taxs.FirstOrDefault(i => i.Id == pd.TaxId).TaxRate,
            //               barcode = pd.BarCode,
            //               quantity = sd.RemainingQuantity
            //           };
            //    res = res2.Concat(res3);
           // }

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
        public HttpResponseMessage sell([FromBody] JObject sell)
        {
            Helper.WriteDebug(new Exception(), sell["sell"].ToString());
            var test = false;

            String value = sell["sell"].ToString();
            List<MySell> maSells = JsonConvert.DeserializeObject<List<MySell>>(value);
            var csello = new JArray();
            var duplicates = new JArray();
            var sellCount = new JArray();

            foreach (MySell mySell in maSells)
            {
                csello.Add(mySell.paymentMethod);
                if (!test)
                {
                    User seller_user = db.Users.FirstOrDefault(i => i.Id == mySell.userId);
                    Sale ObjSale = new Sale();
                    foreach (var item in mySell.products)
                    {
                        sellCount.Add(item.prodId);
                        var selectedProduct = db.Products.Where(i => i.Id == item.prodId).FirstOrDefault();
                        var ObjWarehouseStock = db.WarehouseStocks.Where(i => i.ProductId == item.prodId && i.WarehouseId == seller_user.WarehouseId).FirstOrDefault();
                        DateTime nowDate = DateTime.ParseExact(mySell.date + " " + mySell.time, "dd/MM/yyyy HH:mm:ss", null);
                        decimal taxAmount = db.Taxs.FirstOrDefault(i => i.Id == selectedProduct.TaxId).TaxRate;

                        if (mySell.currency == "USD")
                        {
                            ObjSale.ProductId = item.prodId;
                            ObjSale.Quantity = item.quantity;

                            //ObjSale.UnitPrice = selectedProduct.PurchasePrice * item.quantity;
                            ObjSale.SalePrice = item.price;
                            ObjSale.TotalAmount = (item.price * ObjSale.Quantity);
                            if (selectedProduct.TaxId != 2)
                            {
                                ObjSale.TotalAmountWithTax = ObjSale.TotalAmount;
                            }
                            else
                            {
                                ObjSale.TotalAmountWithTax = ObjSale.TotalAmount + (taxAmount * ObjSale.Quantity);
                            }
                            ObjSale.WarehouseId = (int)seller_user.WarehouseId;
                            ObjSale.AddedBy = seller_user.Id;
                            ObjSale.CustomerUserId = 29611;
                            ObjSale.DateAdded = nowDate;
                            ObjSale.DateModied = nowDate;
                            ObjSale.ModifiedBy = seller_user.Id;
                            ObjSale.PaidAmount = (item.price * ObjSale.Quantity);
                            ObjSale.PaymentModeId = db.PaymentModes.FirstOrDefault(i => i.Name == mySell.currency).Id; /*PaymentModeId;*/
                            ObjSale.InventoryTypeId = 2;
                            ObjSale.isFormalSale = false;
                        }
                        else
                        {
                            var mypayment = db.PaymentModes.FirstOrDefault(i => i.Name == mySell.paymentMethod).Name;
                            var mycurrency = db.Currencies.FirstOrDefault(i => i.Name == mypayment).Id;
                            var priceRate = db.Rates.Where(i => i.CurrencyId == mycurrency).OrderByDescending(i => i.DateModified).First().CurrencyRate;
                            ObjSale.ProductId = item.prodId;
                            ObjSale.Quantity = item.quantity;
                            //    ObjSale.UnitPrice = selectedProduct.PurchasePrice * item.quantity;
                            ObjSale.SalePrice = item.price;
                            ObjSale.TotalAmount = item.price * ObjSale.Quantity;
                            if (selectedProduct.TaxId != 2)
                            {
                                ObjSale.TotalAmountWithTax = ObjSale.TotalAmount;
                            }
                            else
                            {
                                ObjSale.TotalAmountWithTax = ObjSale.TotalAmount + (taxAmount * ObjSale.Quantity);
                            }
                            ObjSale.WarehouseId = (int)seller_user.WarehouseId;
                            ObjSale.AddedBy = seller_user.Id;
                            ObjSale.CustomerUserId = 29611;
                            ObjSale.DateAdded = nowDate;
                            ObjSale.DateModied = nowDate;
                            ObjSale.ModifiedBy = seller_user.Id;
                            ObjSale.PaidAmount = item.price * ObjSale.Quantity;
                            ObjSale.PaymentModeId = db.PaymentModes.FirstOrDefault(i => i.Name == mySell.paymentMethod).Id; /*PaymentModeId;*/
                            ObjSale.InventoryTypeId = 2;
                            ObjSale.rtgs = ObjSale.TotalAmount * (decimal)priceRate;
                            ObjSale.isFormalSale = false;
                        }
                        ObjSale.customerName = mySell.customer;
                        ObjSale.recieptNumber = mySell.invoiceId;

                        try
                        {
                            db.Sales.Add(ObjSale);
                            db.SaveChanges(seller_user.FullName);
                            if (ObjWarehouseStock.RemainingQuantity < item.quantity)
                            {
                                string result = "Break Failed.";
                                var tak = db.Products.Find(item.prodId);
                                var me = db.WarehouseStocks.FirstOrDefault(n => n.ProductId == item.prodId && n.WarehouseId == seller_user.WarehouseId).ProductId;
                                var tak1 = db.WarehouseStocks.FirstOrDefault(m => m.ProductId == me && m.WarehouseId == seller_user.WarehouseId);
                                var tak2 = db.Products.FirstOrDefault(m => m.Name == "0" && m.WarehouseId == seller_user.WarehouseId);
                                if (tak2 == null)
                                {
                                    tak2.Id = 1;
                                }
                                if (tak.ProductCaseId != tak2.Id)
                                {
                                    var ProdCase = db.Products.Find(tak.ProductCaseId);
                                    var WareProdCase = db.WarehouseStocks.FirstOrDefault(m => m.ProductId == tak1.Product_ProductId.ProductCaseId && m.WarehouseId == seller_user.WarehouseId);

                                    if (tak1.RemainingQuantity <= 0 || WareProdCase.RemainingQuantity - 5 <= ProdCase.StockAlert)
                                    {
                                        //Break one case
                                        //subtract 1 case from Prodcase
                                        WareProdCase.RemainingQuantity = WareProdCase.RemainingQuantity - 1;

                                        // add 1* number of singles in case to tak
                                        tak1.RemainingQuantity = (int)(tak1.RemainingQuantity + (1 * ProdCase.NumOfSinglesInCase));
                                    }

                                    db.Entry(tak).State = EntityState.Modified;
                                    db.Entry(tak1).State = EntityState.Modified;
                                    db.Entry(WareProdCase).State = EntityState.Modified;
                                    db.Entry(ProdCase).State = EntityState.Modified;
                                    db.SaveChanges(seller_user);
                                }
                            }

                            WarehouseStock warehse = new WarehouseStock();
                            warehse = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == item.prodId && i.WarehouseId == seller_user.WarehouseId);
                            warehse.RemainingQuantity = ObjWarehouseStock.RemainingQuantity - (decimal)item.quantity;
                            db.Entry(warehse).State = EntityState.Modified;
                            db.SaveChanges();

                            ProductStock ps = new ProductStock();
                            ps.ProductId = ObjSale.ProductId;
                            ps.Quantity = ObjSale.Quantity;
                            ps.PurchasePrice = selectedProduct.PurchasePrice;
                            ps.TotalPurchaseAmount = (selectedProduct.PurchasePrice * ObjSale.Quantity);
                            ps.SalePrice = ObjSale.SalePrice;
                            ps.Discount = selectedProduct.Discount;
                            ps.TotalSaleAmount = (ObjSale.SalePrice * ObjSale.Quantity);
                            decimal TaxAmount = 0;
                            ps.TotalSaleAmountWithTax = (selectedProduct.SalePrice * ObjSale.Quantity);//+ TaxAmount
                            ps.TaxAmount = TaxAmount;
                            ps.Profit = (ps.TotalSaleAmount - (ps.TotalPurchaseAmount));//+ TaxAmount
                            ps.ProfitWithTax = (ps.TotalSaleAmount - ps.TotalPurchaseAmount);//+ TaxAmount
                            ps.Description = "SaleNote";
                            ps.AddedBy = seller_user.Id;
                            ps.DateAdded = nowDate;
                            ps.ModifiedBy = seller_user.Id;
                            ps.DateModied = DateTime.Now;
                            ps.InventoryTypeId = 2;
                            ps.WarehouseId = (int)seller_user.WarehouseId;
                            ps.IsFormal = false;
                            ps.OtherTaxValue = ObjSale.Id;
                            ps.RemainingQuantity = ObjWarehouseStock.RemainingQuantity;
                            db.ProductStocks.Add(ps);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            duplicates.Add(Convert.ToString(new { reciept = mySell.invoiceId, prodId = item.prodId.ToString(), prodName = selectedProduct.Name.ToString() }));
                        }
                    }
                }
            }

            return Request.CreateResponse(
            HttpStatusCode.OK,
            new { trynos = csello.ToString(), sellsCount = sellCount.Count(), duplicatesCount = duplicates.Count(), duplicatesList = duplicates.ToString() },
            JsonMediaTypeFormatter.DefaultMediaType);
        }
    }
}