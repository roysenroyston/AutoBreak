using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopMate.ModelDto;
using ShopMate.Models;
using WebErrorLogging.Utilities;

namespace ShopMate.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string Date = "")
        {
           HomeDto home = new HomeDto();
         
            using (SIContext db = new SIContext())
            {
                DateTime tDate = DateTime.Now;
                if (Date.Length > 2)
                {
                    tDate = Convert.ToDateTime(Date);
                }
                try
                {

                    int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
                    int tDay = tDate.Day;
                    int tMonth = tDate.Month;
                    int tYear = tDate.Year;

                    ViewBag.dates = tDate.ToString("MM/dd/yyyy");

                    home.Role = db.Roles.Count();

                    home.User = db.Users.Where(i => i.Role_RoleId.RoleName == "Admin" || i.Role_RoleId.RoleName == "User" ).Count();                  
                    home.Customer = db.Users.Where(i => i.Role_RoleId.RoleName == "Customer").Count();
                    home.Vendor = db.Users.Where(i => i.Role_RoleId.RoleName == "Vendor" /*&& i.WarehouseId == warehouse*/).Count();
                    home.todaySaleOrders = db.SaleOrders.Where(i => (i.DateAdded.Day == tDay && i.DateAdded.Month == tMonth && i.DateAdded.Year == tYear)/* && i.WarehouseId == warehouse*/).Count();
                    home.todayProcessedSaleOrders = db.SaleOrders.Where(i => (i.DateModified.Day == tDay && i.DateModified.Month == tMonth && i.DateModified.Year == tYear)/* && i.WarehouseId == warehouse*/ && i.IsProcessed == true).Count();
                    home.todayPendingOrders = db.SaleOrders.Where(i => (i.DateModified.Day == tDay && i.DateModified.Month == tMonth && i.DateModified.Year == tYear) /*&& i.WarehouseId == warehouse*/ && i.IsProcessed == false).Count();

                    if (warehouse==1)
                    {

                        var todayPurchaseCount = db.Purchases.Where(i => (i.DateAdded.Day == tDay && i.DateAdded.Month == tMonth && i.DateAdded.Year == tYear) /*&& i.WarehouseId == warehouse*/).Select(i => new { i.Quantity }).ToArray();
                        if (todayPurchaseCount.Count() > 0)
                        {
                            home.todayPurchaseCount = todayPurchaseCount.Sum(i => i.Quantity);
                        }

                        var todaySaleCount = db.Sales.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) /*&& i.WarehouseId == warehouse*/).Select(i => new { i.Quantity }).ToArray();
                        if (todaySaleCount.Count() > 0)
                        {
                            home.todaySaleCount = todaySaleCount.Sum(i => i.Quantity);
                        }

                        var todayPurchaseReturnCount = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 1013 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.ReturnedQuantity }).ToArray();
                        if (todayPurchaseReturnCount.Count() > 0)
                        {
                            home.todayPurchaseReturnCount = todayPurchaseReturnCount.Sum(i => i.ReturnedQuantity);
                        }


                        var todaySaleReturnCount = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 4 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.ReturnedQuantity }).ToArray();
                        if (todaySaleReturnCount.Count() > 0)
                        {
                            home.todaySaleReturnCount = todaySaleReturnCount.Sum(i => i.ReturnedQuantity);
                        }


                        var todayPurchaseCostSum = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 1 /*&& i.WarehouseId == warehouse*/).Select(i => new { amt = i.TotalPurchaseAmount + i.TaxAmount }).ToArray();
                        if (todayPurchaseCostSum.Count() > 0)
                        {
                            home.todayPurchaseCostSum = todayPurchaseCostSum.Sum(i => i.amt);
                        }

                        var todayPurchasedRawMaterialsCostSum = db.RawMaterialStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 1 /*&& i.WarehouseId == warehouse*/).Select(i => new { amt = i.TotalPurchaseAmount }).ToArray();
                        if (todayPurchaseCostSum.Count() > 0)
                        {
                            home.todayPurchasedRawMaterialsCostSum = todayPurchasedRawMaterialsCostSum.Sum(i => i.amt);
                        }



                        var todaySaleCostSum = db.Sales.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 2 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.TotalAmount }).ToArray();
                        if (todaySaleCostSum.Count() > 0)
                        {
                            home.todaySaleCostSum = (decimal)todaySaleCostSum.Sum(i => i.TotalAmount);
                        }

                        var todayProfit = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 2 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.Profit }).ToArray();
                        if (todayProfit.Count() > 0)
                        {
                            home.todayProfit = todayProfit.Sum(i => i.Profit);
                        }

                        var todayProfitWithTax = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 2 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.ProfitWithTax }).ToArray();

                        if (todayProfitWithTax.Count() > 0)
                        {
                            home.todayProfitWithTax = todayProfitWithTax.Sum(i => i.ProfitWithTax);
                        }

                        //var PurchaseItemsQuantity = db.ProductStocks.Where(i => i.InventoryTypeId == 1 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.Quantity }).ToArray();
                        //if (PurchaseItemsQuantity.Count() > 0)
                        //{
                        //    home.PurchaseItemsQuantity = PurchaseItemsQuantity.Sum(i => i.Quantity);
                        //}

                        var SaleItemsQuantity = db.Sales.Where(i => i.InventoryTypeId == 2).Select(i => new { i.Quantity }).ToArray();
                        if (SaleItemsQuantity.Count() > 0)
                        {
                            home.SaleItemsQuantity = SaleItemsQuantity.Sum(i => i.Quantity);
                        }
                        var warestock = db.WarehouseStocks.Select(i => new { i.RemainingQuantity }).ToArray();
                        if (warestock.Count() > 0)
                        {
                            home.WarestockRemaining = warestock.Sum(i => i.RemainingQuantity);
                        }
                        var RawMaterialsItemsQuantity = db.RawMaterialStocks.Where(i => i.InventoryTypeId == 1 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.Quantity }).ToArray();
                        if (SaleItemsQuantity.Count() > 0)
                        {
                            home.RawMaterialsItemsQuantity = RawMaterialsItemsQuantity.Sum(i => i.Quantity);
                        }

                        var stocksadjustedItemsQuantity = db.ProductStocks.Where(i => i.InventoryTypeId == 6 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.Quantity }).ToArray();
                        if (stocksadjustedItemsQuantity.Count() > 0)
                        {
                            home.stocksadjustedItemsQuantity = stocksadjustedItemsQuantity.Sum(i => i.Quantity);
                        }

                        var expense = db.Expenses.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) /*&& i.WarehouseId == warehouse*/).Select(i => new { i.Amount }).ToArray();

                        // var PurchaseItemsQuantity1 = db.Purchases.Where(i => i.InventoryTypeId == 1 && i.WarehouseId == warehouse).OrderByDescending(o => o.DateAdded).First().Quantity;/*.Select(i => new { i.Quantity }).ToArray();*/

                        var PurchaseItemsQuantity = db.Purchases.Where(i => i.InventoryTypeId == 1 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.Quantity }).ToArray();
                        if (PurchaseItemsQuantity.Count() > 0)
                        {
                            home.PurchaseItemsQuantity = PurchaseItemsQuantity.Sum(i => i.Quantity);
                        }
                        var due = db.DuePayments.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear)/* && i.WarehouseId == warehouse*/).Select(i => new { i.DueAmount, i.IsReturn }).ToArray();

                        home.Expense = expense.Sum(i => i.Amount);
                        home.DueGiven = due.Where(i => i.IsReturn == false).Sum(i => i.DueAmount);
                        home.DueReturn = due.Where(i => i.IsReturn == true).Sum(i => i.DueAmount);
                    }
                    else
                    {

                        var todayPurchaseCount = db.Purchases.Where(i => (i.DateAdded.Day == tDay && i.DateAdded.Month == tMonth && i.DateAdded.Year == tYear) && i.WarehouseId == warehouse).Select(i => new { i.Quantity }).ToArray();
                        if (todayPurchaseCount.Count() > 0)
                        {
                            home.todayPurchaseCount = todayPurchaseCount.Sum(i => i.Quantity);
                        }

                        var todaySaleCount = db.Sales.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.WarehouseId == warehouse).Select(i => new { i.Quantity }).ToArray();
                        if (todaySaleCount.Count() > 0)
                        {
                            home.todaySaleCount = todaySaleCount.Sum(i => i.Quantity);
                        }

                        var todayPurchaseReturnCount = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 3 && i.WarehouseId == warehouse).Select(i => new { i.ReturnedQuantity }).ToArray();
                        if (todayPurchaseReturnCount.Count() > 0)
                        {
                            home.todayPurchaseReturnCount = todayPurchaseReturnCount.Sum(i => i.ReturnedQuantity);
                        }

                        var todaySaleReturnCount = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 4 && i.WarehouseId == warehouse).Select(i => new { i.ReturnedQuantity }).ToArray();
                        if (todaySaleReturnCount.Count() > 0)
                        {
                            home.todaySaleReturnCount = todaySaleReturnCount.Sum(i => i.ReturnedQuantity);
                        }


                        var todayPurchaseCostSum = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 1 && i.WarehouseId == warehouse).Select(i => new { amt = i.TotalPurchaseAmount + i.TaxAmount }).ToArray();
                        if (todayPurchaseCostSum.Count() > 0)
                        {
                            home.todayPurchaseCostSum = todayPurchaseCostSum.Sum(i => i.amt);
                        }

                        var todayPurchasedRawMaterialsCostSum = db.RawMaterialStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 1 && i.WarehouseId == warehouse).Select(i => new { amt = i.TotalPurchaseAmount }).ToArray();
                        if (todayPurchaseCostSum.Count() > 0)
                        {
                            home.todayPurchasedRawMaterialsCostSum = todayPurchasedRawMaterialsCostSum.Sum(i => i.amt);
                        }



                        var todaySaleCostSum = db.Sales.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 2 && i.WarehouseId == warehouse).Select(i => new { i.TotalAmount }).ToArray();
                        if (todaySaleCostSum.Count() > 0)
                        {
                            home.todaySaleCostSum = (decimal)todaySaleCostSum.Sum(i => i.TotalAmount);
                        }

                        var todayProfit = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 2 && i.WarehouseId == warehouse).Select(i => new { i.Profit }).ToArray();
                        if (todayProfit.Count() > 0)
                        {
                            home.todayProfit = todayProfit.Sum(i => i.Profit);
                        }

                        var todayProfitWithTax = db.ProductStocks.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.InventoryTypeId == 2 && i.WarehouseId == warehouse).Select(i => new { i.ProfitWithTax }).ToArray();

                        if (todayProfitWithTax.Count() > 0)
                        {
                            home.todayProfitWithTax = todayProfitWithTax.Sum(i => i.ProfitWithTax);
                        }

                        //var PurchaseItemsQuantity = db.ProductStocks.Where(i => i.InventoryTypeId == 1 /*&& i.WarehouseId == warehouse*/).Select(i => new { i.Quantity }).ToArray();
                        //if (PurchaseItemsQuantity.Count() > 0)
                        //{
                        //    home.PurchaseItemsQuantity = PurchaseItemsQuantity.Sum(i => i.Quantity);
                        //}

                        var SaleItemsQuantity = db.Sales.Where(i => i.InventoryTypeId == 2 && i.WarehouseId == warehouse).Select(i => new { i.Quantity }).ToArray();
                        if (SaleItemsQuantity.Count() > 0)
                        {
                            home.SaleItemsQuantity = SaleItemsQuantity.Sum(i => i.Quantity);
                        }
                        var warestock = db.WarehouseStocks.Where(m=>m.WarehouseId == warehouse).Select(i => new { i.RemainingQuantity }).ToArray();
                        if (warestock.Count() > 0)
                        {
                            home.WarestockRemaining = warestock.Sum(i => i.RemainingQuantity);
                        }
                    

                        var stocksadjustedItemsQuantity = db.ProductStocks.Where(i => i.InventoryTypeId == 6 && i.WarehouseId == warehouse).Select(i => new { i.Quantity }).ToArray();
                        if (stocksadjustedItemsQuantity.Count() > 0)
                        {
                            home.stocksadjustedItemsQuantity = stocksadjustedItemsQuantity.Sum(i => i.Quantity);
                        }

                        var expense = db.Expenses.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.WarehouseId == warehouse).Select(i => new { i.Amount }).ToArray();

                        // var PurchaseItemsQuantity1 = db.Purchases.Where(i => i.InventoryTypeId == 1 && i.WarehouseId == warehouse).OrderByDescending(o => o.DateAdded).First().Quantity;/*.Select(i => new { i.Quantity }).ToArray();*/

                        var PurchaseItemsQuantity = db.Purchases.Where(i => i.InventoryTypeId == 1 && i.WarehouseId == warehouse).Select(i => new { i.Quantity }).ToArray();
                        if (PurchaseItemsQuantity.Count() > 0)
                        {
                            home.PurchaseItemsQuantity = PurchaseItemsQuantity.Sum(i => i.Quantity);
                        }
                        var due = db.DuePayments.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear) && i.WarehouseId == warehouse).Select(i => new { i.DueAmount, i.IsReturn }).ToArray();

                        home.Expense = expense.Sum(i => i.Amount);
                        home.DueGiven = due.Where(i => i.IsReturn == false).Sum(i => i.DueAmount);
                        home.DueReturn = due.Where(i => i.IsReturn == true).Sum(i => i.DueAmount);
                    }



                }
                catch (Exception ex)
                {
                    Helper.WriteError(ex, ex.Message);
                }
            }

            return View(home);

        }


        public class DateTable
        {
            public DateTime DateAdded { get; set; }
        }


        public JsonResult LineChart(int lastDay)
        {


            // forgot above all code if you bind this line chart form your database table

            List<GraphData> dataList = new List<GraphData>();

            var LastDays = DateTime.Now.Date.AddDays(-lastDay);
            SIContext db = new SIContext();
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            /////listDateTable just add your table where have date field like db.User
            //if (warehouse== 1)
            //{
            //    var LastRegister = db.Sales.Where(i => i.DateAdded >= LastDays).ToArray();
            //}
            //else
            //{
                var LastRegister = db.Sales.Where(i => i.DateAdded >= LastDays && i.WarehouseId==warehouse).ToArray();
            //}
        

            for (int i = 0; i < lastDay; i++)
            {
                var dateDynamic = DateTime.Now.Date.AddDays(-i);
                int year = dateDynamic.Year;
                int month = dateDynamic.Month;
                int day = dateDynamic.Day;

                DateTime newDate = new DateTime(year, month, day);
                var hav = LastRegister.Where(j => j.DateAdded.Value.Date == newDate.Date);
                if (hav.Count() > 0)
                {
                    GraphData gdata = new GraphData();
                    gdata.label = newDate.ToString("yyyy-MM-dd");
                    gdata.value = hav.Sum(k => k.Quantity);
                    dataList.Add(gdata);
                }
                else
                {
                    GraphData gdata = new GraphData();
                    gdata.label = newDate.ToString("yyyy-MM-dd");
                    gdata.value = 0;
                    dataList.Add(gdata);
                }

            }

            return Json(dataList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult mypieChart()
        {
            SIContext db = new SIContext();
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            DateTime tDate = DateTime.Now;
        //  var dateDynamic = DateTime.Now.Da;
            List<GraphData> dataList = new List<GraphData>();
            int year = tDate.Year;
            int month = tDate.Month;
            int day = tDate.Day;

            DateTime newDate = new DateTime(year, month, day);
          
                var LastRegister = db.Sales.Where(i => (i.DateAdded.Value.Day == newDate.Day && i.DateAdded.Value.Month == newDate.Month && i.DateAdded.Value.Year == newDate.Year) && i.WarehouseId == warehouse).ToArray();

                var groupByProductName = LastRegister
                                .GroupBy(i => i.Product_ProductId.Name)
                                .Select(group => new
                                {
                                    ProductName = group.Key,
                                    TotalQuantity = group.Sum(item => item.Quantity)
                                })
                                .ToList();


                var hav = LastRegister.Where(i => (i.DateAdded.Value.Day == newDate.Day && i.DateAdded.Value.Month == newDate.Month && i.DateAdded.Value.Year == newDate.Year));

                foreach (var items in groupByProductName)
                {
                    GraphData gdata = new GraphData();
                    gdata.label = items.ProductName;
                    gdata.value = items.TotalQuantity;

                    dataList.Add(gdata);
                }




            var dataList1 = dataList;

            // Sort the list by Size property in ascending order
            var sortedDataList = dataList.OrderBy(item => item.value).ToList();


            // var data = db.Sales
            //.Where(i => (i.DateAdded.Value.Day == tDay && i.DateAdded.Value.Month == tMonth && i.DateAdded.Value.Year == tYear))
            //.GroupBy(i => i.Product_ProductId.Name)
            //.Select(g => new { ProductName = g.Key, Quantity = g.Sum(i => i.Quantity) })
            //.ToList();

            // Convert data to JSON
            //var jsonData = JsonConvert.SerializeObject(data);

            //   ViewBag.Data = jsonData;

            return Json(sortedDataList, JsonRequestBehavior.AllowGet);
        }

        private class GraphData
        {
            public string label { get; set; }
            public decimal value { get; set; }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
