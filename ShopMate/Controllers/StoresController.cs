using ShopMate.ModelDto;
using ShopMate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Threading;
using ExcelDataReader;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using static ShopMate.Controllers.posController;
using WebErrorLogging.Utilities;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace ShopMate.Controllers
{
    public class StoresController : Controller
    {
        int userId = Convert.ToInt32(Env.GetUserInfo("userid"));
        // GET: Stores
        public ActionResult Index()
        {
            return View();
        }
        private SIContext db = new SIContext();
        public ActionResult GetManufacturedGrid()
        {
            return View();
        }

        public ActionResult GetManufacturedGridData()
        {
            var tak = db.ManufacturingMaterials.ToArray();

            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            /*Convert.ToString(c.RawMaterialsId),*/
            Convert.ToString(db.Products.FirstOrDefault(i => i.Id == (c.FinishedGoodsId)).Name),
            //Convert.ToString(c.fi)
            Convert.ToString(c.CutSheet),
            Convert.ToString(c.Remarks),
                  Convert.ToString(db.Users.FirstOrDefault(i => i.Id == (c.AddedBy)).FullName),
            Convert.ToString(c.DateAdded),      
           //  Convert.ToString(c.Approved),

};
            //return View(result);
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetManufacturedItems(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var ObjManufactured = db.Manufacturing.Where(i => i.ManufacturingMaterialId == (id) && i.IsRemoved == false).ToArray();
                //var objQty = db.ManufacturingMaterials.FirstOrDefault(i => i.Id == id);
                var objQty = db.ManufacturingMaterials.FirstOrDefault(i => i.Id == (id));

                //List<ManufacturingMaterial> lstmaterials = new List<ManufacturingMaterial>();
                ManufacturingMaterial materials = new ManufacturingMaterial();
                materials.Id = objQty.Id;
                materials.FinishedGoodsName = objQty.FinishedGoodsName;
                materials.FinishedGoodsQuantity = objQty.FinishedGoodsQuantity;
                //lstmaterials.Add(materials);

                List<Manufacturing> lstItem = new List<Manufacturing>();
                foreach (var item in ObjManufactured)
                {
                    Manufacturing ManufacturedItem = new Manufacturing();

                    ManufacturedItem.RawMaterialsId = item.RawMaterialsId;
                    ManufacturedItem.RawMaterialsname = db.RawMaterial.FirstOrDefault(i => i.Id == item.RawMaterialsId).Name;
                    ManufacturedItem.Quantity = item.Quantity;
                    //ManufacturedItem.Description = item.Description;
                    ManufacturedItem.UnitPrice = item.UnitPrice;
                    ManufacturedItem.UnitTotal = item.UnitTotal;
                    //QuotationItem.TaxId = item.TaxId;
                    //QuotationItem.QuotationId = item.QuotationId;
                    //QuotationItem.totalAmountWithTax = item.TotalAmountWithTax;
                    //QuotationItem.saleOrderId = item.SaleOrderId;
                    //ManufacturedItem.Name = db.Products.FirstOrDefault(i => i.Id == item.ProductId).Name;
                    lstItem.Add(ManufacturedItem);
                }
                if (ObjManufactured == null && objQty==null)
                {
                    return HttpNotFound();
                }
                var result = JsonConvert.SerializeObject(new { data = lstItem, ngoni = db.ManufacturingMaterials.FirstOrDefault(i => i.Id == id) }, Formatting.Indented,
                               new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                               });
               
                return Json(result,  JsonRequestBehavior.AllowGet);
                //return Json(new { saleorderItems = ObjSaleOrdertems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult FinishedGoodsGetDetails(int? id)
        {
        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Objfinished = db.ManufacturingMaterials.Where(i => i.Id == id && i.Approved == false).ToArray();
            //var objFinishedQty = db.ManufacturingMaterials.Where(i => i.Id == ( ).to
            if (Objfinished == null)
            {
                return HttpNotFound();
            }
            return Json(new { salesorders = Objfinished  }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RawMaterials ObjRawMaterial = db.RawMaterial.Find(id);
            if (ObjRawMaterial == null)
            {
                return HttpNotFound();
            }
            return Json(new { Name = ObjRawMaterial.Name, CostPrice = ObjRawMaterial.CostPrice}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetGrid()
        {
            var tak = db.RawMaterialStocks.Where(i => i.Description == "Raw Materials Purchase"). ToArray();
            //var tak = db.Store.ToArray();
            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
                         
                         Convert.ToString(db.RawMaterial.FirstOrDefault(r => r.Id== c.RawMaterialsId).Name),
                          Convert.ToString(c.PurchasePrice),
               Convert.ToString(c.Quantity),
            Convert.ToString(c.TotalPurchaseAmount),
            Convert.ToString(c.Description),
            Convert.ToString(db.Warehouses.FirstOrDefault(i => i.Id == c.WarehouseId).Name)
            //var tak = db.Store.ToArray();

            //var result = from c in tak
            //             select new string[] { c.Id.ToString(), /*Convert.ToString(c.Id),*/
            //                 Convert.ToString(db.RawMaterial.FirstOrDefault(i => i.Id == c.Id).Name),
            //                 Convert.ToString(db.Users.FirstOrDefault(i => i.Id == c.AddedBy).UserName),
            //Convert.ToString(c.purchasedate),
            //Convert.ToString(db.RawMaterialStocks.FirstOrDefault(i => i.Id == c.Id).Quantity),
            //Convert.ToString(db.Warehouses.FirstOrDefault(i => i.Id == c.WarehouseId).Name)



             };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Finished()
        {
            return View();
        }
        public ActionResult GetFinishedGrid()
        {

            var tak = db.FinishedGoods.ToArray();
            var prd = db.Products;
          //  var mer = db.ManufacturingMaterials
            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
                         Convert.ToString(c.FinishedProduct),
            Convert.ToString(c.FinishedQty),
            //Convert.ToString(c.CostPrice),
            Convert.ToString(c.finisheddate),
               Convert.ToString(db.Users.FirstOrDefault(i => i.Id == c.AddedBy).FullName ),         
             Convert.ToString(db.Warehouses.FirstOrDefault(i=>i.Id==c.WarehouseId).Name),
               Convert.ToString( db.ManufacturingMaterials.FirstOrDefault( m => m.Id == c.manufacturingId).Approved),





             };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        //GET: Stores/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult FinishedgoodsApproval(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinishedGoods ObjFinishedGoods = db.FinishedGoods.Find(id);
            if (ObjFinishedGoods == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.FinishedGoods, "Id", "Name", ObjFinishedGoods.Id);
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "Id", "Name");
            //IEnumerable<FinishedGoods> finishedGoods = db.FinishedGoods.Where(qt => qt.Id.Equals(ObjFinishedGoods.Id));
            //ObjFinishedGoods. = finishedGoods;
            return View(ObjFinishedGoods);
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult FinishedgoodsApproval(FinishedGoods ObjFinishedGoods)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
            var productId = db.Products.FirstOrDefault(i => i.Name == ObjFinishedGoods.FinishedProduct);
            var whseproductId = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == productId.Id);
            //int warehouseId = ;
            int WarehouseId = (int)db.Users.Where(t => t.Id == AddedBy).FirstOrDefault().WarehouseId;
            string result = "Error! Finished Goods Not Approved: please start again!";
            try
            {


                if (ModelState.IsValid)
                {
                    var ObjWarehouseStock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == whseproductId.ProductId && i.WarehouseId == WarehouseId);
                    var SelectedProduct = db.Products.FirstOrDefault(i => i.Id == productId.Id);
                    //ObjWarehouseStock.WarehouseId = 1;
                    if (ObjWarehouseStock.WarehouseId == 1)
                    {                    
                        SelectedProduct.RemainingQuantity = SelectedProduct.RemainingQuantity - ObjFinishedGoods.FinishedQty;//dispatch
                        ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity + ObjFinishedGoods.FinishedQty;// other warestocks
                        db.Entry(ObjWarehouseStock).State = EntityState.Modified;
                        db.Entry(SelectedProduct).State = EntityState.Modified;
                        db.SaveChanges();
                     }
                    else
                    {
                        SelectedProduct.RemainingQuantity = SelectedProduct.RemainingQuantity - ObjFinishedGoods.FinishedQty;//dispatch
                        ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity + ObjFinishedGoods.FinishedQty;// other warestocks
                        db.Entry(ObjWarehouseStock).State = EntityState.Modified;
                        db.Entry(SelectedProduct).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    ObjFinishedGoods.AddedBy = AddedBy;                  
                    db.Entry(ObjFinishedGoods).State = EntityState.Modified;
                    db.SaveChanges(userId);

                 

                    sb.Append("Submitted");
                    return Content(sb.ToString());

                    //sb.Append("Sumitted");
                    result = "Done";
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
            return Content(sb.ToString());
        }

        public ActionResult FinishedGoods()
        {
            
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "Id", "Name");
            ViewBag.RawMaterialId = new SelectList(db.Products.Where(p => p.ProductType == "INTERNAL"), "Id", "Name" );
            ViewBag.CutSheet = new SelectList(db.ManufacturingMaterials.Where(p => p.Approved == false), "Id", "CutSheet");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FinishedGoods(FinishedGoods ObjPurchase, finishedItem[] product, int? storesId, int? FinishedGoodsQty, string FinishedGoodsName )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! Raw materials Not Saved: please start again!";
            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
            //int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string retVal = "";
            // Get the claims values
            int warehouse = Int16.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.Actor)
                               .Select(c => c.Value).SingleOrDefault());
            var productId = db.Products.FirstOrDefault(i => i.Name == FinishedGoodsName);
            var selectedProduct = db.Products.FirstOrDefault(i => i.Id == productId.Id);
            var warehousestock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == productId.Id && i.WarehouseId == warehouse);      
             ManufacturingMaterial roysen = db.ManufacturingMaterials.Where(n => n.Id == storesId).FirstOrDefault();
            if (roysen.FinishedGoodsQuantity < FinishedGoodsQty)
            {
                //sb.Append("it cant my fre");
                //result = "error";
                //return Content(sb.ToString());
                //retVal = "Order has been Received or Deleted!!";
                //return Json(retVal, JsonRequestBehavior.AllowGet);
                //retVal.Add(new SaleReturn { msg = "error:" + ex.Message, value = 0 });
                //sb.Append("Sumitted");
                result = "error";
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            try
            {

                if (ModelState.IsValid)
                {
                    ObjPurchase.FinishedProduct = FinishedGoodsName;
                    ObjPurchase.FinishedQty = (int)FinishedGoodsQty;
                    ObjPurchase.AddedBy = AddedBy;
                    ObjPurchase.finisheddate = DateTime.Now;
                    ObjPurchase.WarehouseId = warehouse;                 
                    ObjPurchase.Approved = false;
                    ObjPurchase.manufacturingId = storesId;
                    db.FinishedGoods.Add(ObjPurchase);
                    db.SaveChanges(userId);

                    int vendorLedger = 0;

                    var LedgerA = db.LedgerAccounts.FirstOrDefault(i => i.Name.Trim() == "Raw Materials");
                    if (LedgerA != null)
                    {
                        vendorLedger = LedgerA.Id;
                    }
                    else
                    {
                        LedgerAccount la = new LedgerAccount();
                        la.Name = "Raw Materials";
                        la.ParentId = 2;
                        la.AddedBy = AddedBy;
                        la.DateAdded = DateTime.Now;
                        db.LedgerAccounts.Add(la);
                        db.SaveChanges(userId);

                        vendorLedger = la.Id;
                    }

                    Transaction tr = new Transaction();
                    tr.AddedBy = AddedBy;
                    tr.DebitLedgerAccountId = vendorLedger;
                    //tr.DebitAmount = ObjPurchase.CostPrice;
                    tr.CreditLedgerAccountId = db.LedgerAccounts.FirstOrDefault(i => i.Name == "Finished Goods").Id;
                    //tr.CreditAmount = ObjPurchase.CostPrice;
                    tr.DateAdded = DateTime.Now;
                    tr.Remarks = "Finished Goods From Manafacturing,Manafacturing Account credit and Product Stock account debit";
                    tr.Other = null;
                    tr.PurchaseOrSale = "Purchase";
                    tr.PurchaseIdOrSaleId = ObjPurchase.Id;
                    tr.WarehouseId = warehouse;
                    db.Transactions.Add(tr);


                    db.SaveChanges(userId);
                    foreach (var item in product)
                    {
                        var roy = db.RawMaterial.FirstOrDefault(i => i.Name == item.description);
                        

                        finishedItem its = new finishedItem();
                        its.dateadded = DateTime.Now;
                        its.description = item.description;
                        its.finishedgoods = ObjPurchase;
                        its.InventoryTypeId = db.InventoryTypes.FirstOrDefault(i => i.Name == "Finished Goods").Id;
                        its.ProductId = roy.Id;
                        //its.ProductStockId = ps.Id;
                        its.Quantity = item.Quantity;
                        //its.Total = ps.TotalSaleAmountWithTax;
                        its.TransactionId = tr.Id;
                        its.unitprice = item.unitprice;
                        its.WarehouseId = warehouse;
                        its.cutsheet = item.cutsheet;                        
                        db.FinishedItems.Add(its);
                        db.SaveChanges(userId);

                        
                                         
                    }
                    FinishedGoods complete1 = db.FinishedGoods.Where(n => n.manufacturingId == storesId).FirstOrDefault();
                    ManufacturingMaterial complete = db.ManufacturingMaterials.Where(n => n.Id == storesId).FirstOrDefault();
        
                    if (complete.FinishedGoodsQuantity == FinishedGoodsQty || complete.FinishedGoodsQuantity == 0)
                    {
                        if ( complete1.manufacturingId == storesId)
                        {
                            complete1.Approved = true;
                            db.Entry(complete1).State = EntityState.Modified;
                            db.SaveChanges(userId);
                        }

                        complete.Approved = true;
                        db.Entry(complete).State = EntityState.Modified;
                        db.SaveChanges(userId);
                    }

                    complete.FinishedGoodsQuantity = (int)(product[0].Qty - FinishedGoodsQty);
                    db.Entry(complete).State = EntityState.Modified;
                    db.SaveChanges(userId);

                    //selectedProduct.RemainingQuantity =  selectedProduct.RemainingQuantity + ObjPurchase.FinishedQty;
                    warehousestock.RemainingQuantity = warehousestock.RemainingQuantity +(decimal) FinishedGoodsQty;
                        db.Entry(warehousestock).State = EntityState.Modified;
                        db.SaveChanges(userId);

                  

                    sb.Append("Sumitted");
                    result = "Done";
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
                sb.Append("Error :" + ex.Message);
            }

            return Content(sb.ToString());

        }

        //private void ManufacturedItems(decimal manufacturedItemsUsed, finishedItem finishedItem)
        //{
        //    List<FinishedManufacturedItem> rawMaterialsUsed = new List<FinishedManufacturedItem>();
        //    var miu = db.Manufacturing.ToArray();
        //    var numQuery =
        //     from num in miu
        //     where num.Remaining > 0
        //     select num;

        //    do
        //    {
        //        decimal itemsR;
        //        foreach (var item in numQuery)
        //        {
        //            FinishedManufacturedItem fim = new FinishedManufacturedItem();
        //            if (item.Remaining >= manufacturedItemsUsed)
        //            {
        //                itemsR = manufacturedItemsUsed;
        //                fim.ManufacturingId = item.Id;
        //                fim.finishedItemId = finishedItem.Id;
        //                fim.Quantity = manufacturedItemsUsed;
        //                // subtract manufacturedItemsUsed from Remaining
        //                //build object to save
        //                // save db FiishedItemsManufactured
        //                //Adjust remaining in db MAnufactured
        //                rawMaterialsUsed.Add(fim);
        //                //Subtract remaining from manufacturedItemsUsed
        //                item.Remaining = item.Remaining - itemsR;
        //            }
        //            else
        //            {
        //                itemsR = item.Remaining;
        //                fim.ManufacturingId = item.Id;
        //                fim.finishedItemId = finishedItem.Id;
        //                fim.Quantity = item.Remaining;
        //                //build object to save quantity == remaining
        //                // save db FinishedItemsManufactured
        //                //Adjust remaining in db Manufactured by subtracting remaining
        //                //Subtract remaining from manufacturedItemsUsed
        //                rawMaterialsUsed.Add(fim);
        //                //Subtract remaining from manufacturedItemsUsed
        //                item.Remaining = item.Remaining - itemsR;
        //            }
        //            manufacturedItemsUsed -= itemsR;
        //        }


        //    }
        //    while (manufacturedItemsUsed > 0);
        //    foreach (var item in rawMaterialsUsed)

        //    {
        //        db.FinishedManufacturedItems.Add(item);
        //        db.SaveChanges(userId);
        //    }
        //    //throw new NotImplementedException();
        //}

        public ActionResult Create()
        {
            ViewBag.VendorUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Supplier"), "Id", "UserName");
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "Id", "Name");
            ViewBag.RawMaterialId = new SelectList(db.RawMaterial, "Id", "Name");
            return View();
        }


        // POST: Stores/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Stores Objstores, StoresMaterials[] materials, int VendorUserId)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! Raw materials Not Saved: please start again!";
            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
             if(materials == null)
            {
                result = "Add Raw Materials Items First!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    Objstores.VendorUserId = VendorUserId;
                    Objstores.AddedBy = AddedBy;
                    Objstores.purchasedate = DateTime.Now;
                    Objstores.WarehouseId = warehouse;
                    db.Store.Add(Objstores);
                    db.SaveChanges(userId);

                    int vendorLedger = 0;

                    var LedgerA = db.LedgerAccounts.FirstOrDefault(i => i.Name.Trim() == "Raw Materials");
                    if (LedgerA != null)
                    {
                        vendorLedger = LedgerA.Id;
                    }
                    else
                    {
                        LedgerAccount la = new LedgerAccount();
                        la.Name = "Raw Materials";
                        //la.ParentId = 2;
                        la.ParentId = db.LedgerAccounts.FirstOrDefault(i => i.Name == ("Raw Materials")).Id;
                        la.AddedBy = AddedBy;
                        la.DateAdded = DateTime.Now;
                        db.LedgerAccounts.Add(la);
                        db.SaveChanges(userId);

                        vendorLedger = la.Id;
                    }

                    Transaction tr = new Transaction();
                    tr.AddedBy = AddedBy;
                    tr.DebitLedgerAccountId = vendorLedger;
                    tr.DebitAmount = Objstores.totalprice;
                    tr.CreditLedgerAccountId = db.LedgerAccounts.FirstOrDefault(i => i.Name == ("Raw Materials")).Id;
                    tr.CreditAmount = Objstores.totalprice;
                    tr.DateAdded = DateTime.Now;
                    tr.Remarks = "Purchase raw Materials, Bank Account credit and Expense account debit";
                    tr.Other = null;
                    tr.PurchaseOrSale = "Purchase";
                    tr.PurchaseIdOrSaleId = Objstores.Id;
                    tr.WarehouseId = warehouse;
                    tr.IsFormal =true;
                    db.Transactions.Add(tr);


                    db.SaveChanges(userId);
                    foreach (var item in materials)
                    {
                        //var selectedProduct = db.Products.FirstOrDefault(i => i.Id == item.ProductId);                        
                        var selectedRawMaterial = db.RawMaterial.FirstOrDefault(i => i.Id == item.RawMaterialsId);
                        var selectedTax = db.Taxs.FirstOrDefault(i => i.Id == selectedRawMaterial.TaxId);

                        RawMaterials ObjPurchase = db.RawMaterial.Where(i => i.Id == item.RawMaterialsId).FirstOrDefault();

                        ObjPurchase.CostPrice = Convert.ToString(item.unitprice);

                        db.Entry(ObjPurchase).State = EntityState.Modified;
                        db.SaveChanges(userId);


                        RawMaterialStock ps = new RawMaterialStock();
                        ps.RawMaterialsId = item.RawMaterialsId;
                        ps.Quantity = item.Quantity;
                        ps.Description = "Raw Materials Purchase";
                        ps.AddedBy = AddedBy;
                        ps.PurchasePrice = item.unitprice;
                        ps.TotalPurchaseAmount = item.Total;
                        ps.DateAdded = DateTime.Now;
                        decimal TaxAmount = 0;
                        decimal vatonreturn = 0;
                        if (selectedTax.Other == "GST")
                        {
                            decimal taxSplit = selectedTax.TaxRate / 2;
                            ps.CGST = selectedRawMaterial.TaxId;
                            ps.CGST_Rate = taxSplit;
                            ps.SGST = selectedRawMaterial.TaxId;
                            ps.SGST_Rate = taxSplit;

                            TaxAmount = ((selectedTax.TaxRate) / (115)) * ps.TotalPurchaseAmount;
                            vatonreturn = ((selectedTax.TaxRate) / (100)) * ps.TotalPurchaseAmount;
                            //  TaxAmount = (15 /115) * ps.TotalPurchaseAmount;

                        }
                        else if (selectedTax.Other == "IGST")
                        {
                            ps.IGST = selectedRawMaterial.TaxId;
                            ps.IGST_Rate = selectedTax.TaxRate;
                            //TaxAmount = (15 / 115) * ps.TotalPurchaseAmount;
                            TaxAmount = ((selectedTax.TaxRate) / (115)) * ps.TotalPurchaseAmount;
                            vatonreturn = ((selectedTax.TaxRate) / (100)) * ps.TotalPurchaseAmount;
                        }
                        else if (selectedTax.Other == "Other")
                        {
                            ps.TaxId = selectedRawMaterial.TaxId;
                            ps.OtherTaxValue = selectedTax.TaxRate;
                            //TaxAmount = (15 / 115) * ps.TotalPurchaseAmount;
                            TaxAmount = ((selectedTax.TaxRate) / (115)) * ps.TotalPurchaseAmount;
                            vatonreturn = ((selectedTax.TaxRate) / (100)) * ps.TotalPurchaseAmount;
                        }                        
                        ps.TotalPurchaseAmountWithTax = (ps.TotalPurchaseAmount + TaxAmount);//+ TaxAmount
                        ps.TaxAmount = TaxAmount;
                        ps.TaxId = selectedRawMaterial.TaxId;
                        ps.InventoryTypeId = db.InventoryTypes.FirstOrDefault(i => i.Name == "Purchase").Id;
                        ps.WarehouseId = warehouse;
                        db.RawMaterialStocks.Add(ps);
                        db.SaveChanges(userId);

               //         string constring = System.Configuration.ConfigurationManager.ConnectionStrings["SIConnectionString"]
               //.ConnectionString;


               //         string qury = "UPDATE RawMaterials SET CostPrice='" + item.unitprice + "' WHERE 'Id'='" + selectedRawMaterial.Id + "'";
               //         using (SqlConnection con = new SqlConnection(constring))
               //         {
               //             using (SqlCommand cmd = new SqlCommand(qury, con))
               //             {
               //                 con.Open();
               //                 cmd.ExecuteNonQuery();
               //             }
               //         }

               //         //RawMaterials pr = db.RawMaterial.Where(r => r.Id == item.Id).FirstOrDefault();
               //         RawMaterials pr = new RawMaterials();
               //         pr.Id = selectedRawMaterial.Id;
               //         pr.CostPrice = Convert.ToString(item.unitprice);
               //         db.Entry(pr).State = EntityState.Modified;
               //         db.SaveChanges(userId);

                        StoresMaterials mat = new StoresMaterials();
                        mat.Quantity = item.Quantity;
                        //mat.goods = item.goods;
                        mat.unitprice = item.unitprice;
                        mat.Total = item.Total;
                        mat.InventoryTypeId = db.InventoryTypes.FirstOrDefault(i => i.Name == "Purchase").Id;
                        mat.store = Objstores;
                        mat.RawMaterialsId = item.RawMaterialsId;
                        mat.rawmaterialStockId = ps.Id;
                        mat.TransactionId = tr.Id;
                        db.StoreMaterial.Add(mat);
                        db.SaveChanges();

                        selectedRawMaterial.RemainingQuantity = selectedRawMaterial.RemainingQuantity +item.Quantity;
                        db.Entry(selectedRawMaterial).State = EntityState.Modified;
                        db.SaveChanges(userId);

                    }


                    //  Get Ledger Account

                    //end 

                    //transaction

                    sb.Append("Sumitted");
                    result = "Success! Materials Saved";
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
                sb.Append("Error :" + ex.Message);
            }

            return Content(sb.ToString());

        }
        public ActionResult Print1(int id)
        {
            ManufacturingMaterial stor = db.ManufacturingMaterials.Find(id);
            var mat = db.RawMaterial.ToArray();
            List<Manufacturing> Materials = db.Manufacturing.Where(jm => jm.ManufacturingMaterialId == id).ToList();

            //int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            //var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
            //var jobcard = db.JobCards.FirstOrDefault(i => i.Id == id && i.WarehouseId == warehouse);

            var user = db.Users.FirstOrDefault(i => i.Id == stor.AddedBy);


            if (stor == null)
            {
                return HttpNotFound();
            }
            var finishedproduct = db.Products.FirstOrDefault(i => i.Id == stor.FinishedGoodsId).Name;
            StoresDto job = new StoresDto();
            job.storeId = stor.Id;
            job.strnum = id;
            job.Finishedgoods = finishedproduct;
            job.DateAdded = (DateTime)stor.DateAdded;
            job.remarks = stor.Remarks;
            job.productqty = stor.FinishedGoodsQuantity;
            job.cutsheetNumber = stor.CutSheet;
            job.Totalprice = stor.TotalAmount;

            List<StoreMaterialDto> materialsList = new List<StoreMaterialDto>();

            foreach (var items in Materials)
            {

                StoreMaterialDto dto = new StoreMaterialDto();
                dto.name = mat.FirstOrDefault(i => i.Id == items.RawMaterialsId).Name; ;
                //dto.goods = items.goods;
                dto.Quantity = items.Quantity;
                dto.price = items.UnitPrice;
                dto.total = items.UnitTotal;

                materialsList.Add(dto);
            }

            job.material = materialsList;

            //job.material = materialsList;

            return View(job);


        }


        public ActionResult Print(int id)
        {
            ManufacturingMaterial stor = db.ManufacturingMaterials.Find(id);
            var mat = db.RawMaterial.ToArray();
             List<Manufacturing> Materials = db.Manufacturing.Where(jm => jm.ManufacturingMaterialId == id).ToList();

            //int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            //var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
            //var jobcard = db.JobCards.FirstOrDefault(i => i.Id == id && i.WarehouseId == warehouse);

            var user = db.Users.FirstOrDefault(i => i.Id == stor.AddedBy);


            if (stor == null)
            {
                return HttpNotFound();
            }
            var finishedproduct = db.Products.FirstOrDefault(i => i.Id == stor.FinishedGoodsId).Name;
            StoresDto job = new StoresDto();
            job.storeId = stor.Id;
            job.strnum = id;
            job.Finishedgoods = finishedproduct;
            job.DateAdded = (DateTime)stor.DateAdded;
            job.remarks = stor.Remarks;
            job.productqty = stor.FinishedGoodsQuantity;
            job.cutsheetNumber = stor.CutSheet;
            job.Totalprice = stor.TotalAmount;

            List<StoreMaterialDto> materialsList = new List<StoreMaterialDto>();

            foreach (var items in Materials)
            {

                StoreMaterialDto dto = new StoreMaterialDto();
                dto.name = mat.FirstOrDefault(i => i.Id == items.RawMaterialsId).Name; ;
                //dto.goods = items.goods;
                dto.Quantity = items.Quantity;
                dto.price = items.UnitPrice;
                dto.total = items.UnitTotal;

                materialsList.Add(dto);
            }

            job.material = materialsList;

            //job.material = materialsList;

            return View(job);


        }
        // GET: Stores/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult EditManufactured(int id)
        {
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "Id", "Name");
            ViewBag.FinishedGoods = new SelectList(db.Products.Where(p => p.ProductType == "INTERNAL"), "Id", "Name");
            ViewBag.CutSheet = new SelectList(db.ManufacturingMaterials.Where(p => p.Approved == false), "Id", "CutSheet");
            ViewBag.RawMaterials = new SelectList(db.RawMaterial, "Id", "Name");

            return View();

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditManufactured(int id, Manufacturing[] products, string FinishedGoodsName, decimal Finishedqty, string Cutsheet)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
           // ManufacturingMaterial stor = db.ManufacturingMaterials.Find(id);
            var mat = db.RawMaterial.ToArray();
            List<Manufacturing> Materials = db.Manufacturing.Where(jm => jm.ManufacturingMaterialId == id).ToList();
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));

            foreach (Manufacturing item in Materials)
            {
                Manufacturing ObjPurchase = db.Manufacturing.Where(i => i.Id == item.Id && i.IsRemoved == false).FirstOrDefault();
  
                ObjPurchase.IsRemoved = true;
                
                db.Entry(ObjPurchase).State = EntityState.Modified;
                db.SaveChanges(userId);
            }

            foreach (Manufacturing item in products)
            {
                int isAvailble = db.Manufacturing.Where(i => i.RawMaterialsId == item.RawMaterialsId).Count();

                if (isAvailble == 1)
                {
                    Manufacturing ObjPurchase = db.Manufacturing.Where(i => i.RawMaterialsId == item.RawMaterialsId).FirstOrDefault();
                    ObjPurchase.IsRemoved = false;

                    db.Entry(ObjPurchase).State = EntityState.Modified;
                    db.SaveChanges(userId);
                }
                else
                {
                 
                    Manufacturing manu = new Models.Manufacturing();
                    manu.RawMaterialsId = item.RawMaterialsId;
                    manu.RawMaterialsname = item.RawMaterialsname;
                    manu.Quantity = item.Quantity;
                    //ObjPurchase.OutputDescription = item.OutputDescription;
                    //ObjPurchase.OutputQuantity = item.OutputQuantity;
                    manu.UnitPrice = item.UnitPrice;
                    manu.WarehouseId = warehouse;
                    manu.AddedBy = AddedBy;
                    manu.UnitTotal = (item.UnitPrice * item.Quantity);
                    manu.DateAdded = DateTime.Now;
                    manu.CutSheet = Materials[0].CutSheet;
                    manu.ManufacturingMaterialId = Materials[0].ManufacturingMaterialId;
                    manu.InventoryTypeId = db.InventoryTypes.FirstOrDefault(i => i.Name == "Raw Materials Out").Id;
                    manu.IsRemoved = false;
                    db.Manufacturing.Add(manu);
                    db.SaveChanges(userId);

                }


                    
            }

            string retVal = "";
            retVal = "Success";
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }
        public class Cart1
        {
            public int RawMaterialsId { get; set; }
            public string RawMaterialsname { get; set; }
            public decimal Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get; set; }
            public string CutSheet { get; set; }


        }
        // manufacturing
        public class Cart
        {
            public int RawMaterialsId { get; set; }
            public decimal Quantity { get; set; }
            public decimal unitprice { get; set; }
           
            public decimal Total { get; set; }
        }
        public ActionResult GetRawMaterialManufuctre()
        {
            var tak = db.RawMaterial.OrderBy(i => i.Name).ToArray();

            var result = from c in tak
                         select new string[] {
            Convert.ToString(c.Name.Replace("'","")),
            Convert.ToString(c.Id) ,
            Convert.ToString(c.RemainingQuantity) ,
           // Convert.ToString(db.Taxs.FirstOrDefault(i=>i.Id== c.TaxId).TaxRate),
             
             };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Manufacture()
        {
            ViewBag.FinishedGoods = new SelectList(db.Products.Where(i => i.ProductType =="INTERNAL" ), "Id","Name");
            ViewBag.PaymentModeId = new SelectList(db.PaymentModes, "Id", "Name", 1);
            ViewBag.RawMaterialId = new SelectList(db.RawMaterial, "Id", "Name");

            StringBuilder sbMoreTax = new StringBuilder();
            var tax = db.Taxs.Where(i => i.Other == "Tax").ToArray();
            foreach (var item in tax)
            {
                sbMoreTax.Append("<option value=\"" + item.Name + "\">" + item.Name + "</option>");
            }

            ViewBag.moreTax = sbMoreTax.ToString();
        

            return View();
        }
        public JsonResult AddToManufucturing(List<Cart> materials, string Cutsheet, int FinishedGoods, string Remarks, decimal totalAmount,bool Approved, int FinishedProductQuantity)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            List<SaleReturn> retVal = new List<SaleReturn>();
            string result = "Error! Raw materials Not Saved to Manufacturing: please start again!";
            try
            {
                foreach (var items in materials)
                {
                    var warehousestock = db.RawMaterial.FirstOrDefault(i => i.Id == items.RawMaterialsId );
                    if (items.Quantity > warehousestock.RemainingQuantity)
                    {
                        result = "Not enough Raw Materials to perfom action";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                }

                int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
                int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
                ManufacturingMaterial Manufactmaterials = new Models.ManufacturingMaterial();
                Manufactmaterials.FinishedGoodsId = FinishedGoods;
                Manufactmaterials.FinishedGoodsName = db.Products.FirstOrDefault(i => i.Id == FinishedGoods).Name;
                Manufactmaterials.CutSheet = Cutsheet;
                Manufactmaterials.Remarks = Remarks;
                Manufactmaterials.Approved = Approved;
                Manufactmaterials.DateAdded = DateTime.Now;
                Manufactmaterials.AddedBy = AddedBy;
                Manufactmaterials.FinishedGoodsQuantity = FinishedProductQuantity;
                Manufactmaterials.TotalAmount = totalAmount;
                db.ManufacturingMaterials.Add(Manufactmaterials);
                db.SaveChanges(userId);



                try
                {
                    //selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity - ps.Quantity;

                    foreach (var item in materials)
                    {
                        var selectedProduct = db.RawMaterial.FirstOrDefault(i => i.Id == item.RawMaterialsId);
                        var selectedRawinstores = db.Store.FirstOrDefault(i => i.Id == item.RawMaterialsId);
                        var rawMaterialPrice = db.RawMaterialStocks.FirstOrDefault(i => i.RawMaterialsId == item.RawMaterialsId).PurchasePrice;
                        //var rawMaterialPrice =   db.RawMaterial
                        //     .Where(x => x.Id == item.RawMaterialsId)
                        //     .OrderByDescending(x => x.Id)
                        //     .First().RemainingQuantity;

                        Manufacturing ObjPurchase = new Models.Manufacturing();

                        ObjPurchase.RawMaterialsId = item.RawMaterialsId;
                        ObjPurchase.RawMaterialsname = db.RawMaterial.FirstOrDefault(n => n.Id == item.RawMaterialsId).Name;
                        ObjPurchase.Quantity = item.Quantity;
                        //ObjPurchase.OutputDescription = item.OutputDescription;
                        //ObjPurchase.OutputQuantity = item.OutputQuantity;
                        ObjPurchase.UnitPrice = item.unitprice;
                        ObjPurchase.WarehouseId = warehouse;
                        ObjPurchase.AddedBy = AddedBy;
                        ObjPurchase.UnitTotal = (item.unitprice * item.Quantity);
                        ObjPurchase.CutSheet = Cutsheet;
                        ObjPurchase.IsRemoved = false;
                        ObjPurchase.DateAdded = DateTime.Now;
                        ObjPurchase.InventoryTypeId = db.InventoryTypes.FirstOrDefault(i => i.Name == "Raw Materials Out").Id;
                        ObjPurchase.ManufacturingMaterialId = Manufactmaterials.Id;
                        db.Manufacturing.Add(ObjPurchase);
                        db.SaveChanges(userId);

                        RawMaterialStock ps = new RawMaterialStock();
                        ps.RawMaterialsId = ObjPurchase.RawMaterialsId;
                        ps.Quantity = ObjPurchase.Quantity;
                        ps.Description = "Raw materials manufactured";
                        ps.AddedBy = AddedBy;
                        ps.DateAdded = DateTime.Now;
                        ps.InventoryTypeId = db.InventoryTypes.FirstOrDefault(i => i.Name == "Raw Materials Out").Id;
                        ps.PurchasePrice = item.unitprice;
                        ps.TotalPurchaseAmount = rawMaterialPrice * ObjPurchase.Quantity;



                        ps.WarehouseId = warehouse;
                        db.RawMaterialStocks.Add(ps);
                        db.SaveChanges(userId);

                        //end

                        //Get Ledger Account
                        int vendorLedger = 0;

                        var LedgerA = db.LedgerAccounts.FirstOrDefault(i => i.Name.Trim() == "Raw Materials");
                        if (LedgerA != null)
                        {
                            vendorLedger = LedgerA.Id;
                        }
                        else
                        {
                            LedgerAccount la = new LedgerAccount();
                            la.Name = "Raw Materials";
                            la.ParentId = db.LedgerAccounts.FirstOrDefault(i => i.Name == ("Raw Materials")).Id;
                            la.AddedBy = AddedBy;
                            la.DateAdded = DateTime.Now;
                            db.LedgerAccounts.Add(la);
                            db.SaveChanges(userId);

                            vendorLedger = la.Id;
                        }
                        //end 

                        // transaction
                        Transaction tr = new Transaction();
                        tr.AddedBy = AddedBy;
                        tr.DebitLedgerAccountId = vendorLedger;
                        tr.DebitAmount = ObjPurchase.Quantity * ps.PurchasePrice;
                        tr.CreditLedgerAccountId = db.LedgerAccounts.FirstOrDefault(i => i.Name == ("Raw Materials")).Id;
                        tr.CreditAmount = ObjPurchase.Quantity * ps.PurchasePrice;
                        tr.DateAdded = DateTime.Now;
                        tr.Remarks = "Deduct to manufacturing , Raw Materials Account credit and Manufacturing account debit";
                        tr.Other = null;
                        tr.PurchaseOrSale = "ManufacturingInput";
                        // tr.PurchaseIdOrSaleId = Objstores.Id;
                        tr.WarehouseId = warehouse;
                        tr.IsFormal = true;
                        db.Transactions.Add(tr);
                        db.SaveChanges(userId);
                        sb.Append("Sumitted");
                        result = "Success! Materials Saved";

                        //end

                        selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity - ps.Quantity;
                        selectedProduct.RemainingAmount = selectedProduct.RemainingAmount - ps.Quantity * ps.PurchasePrice;
                        db.Entry(selectedProduct).State = EntityState.Modified;
                        db.Entry(ObjPurchase).State = EntityState.Modified;
                        db.SaveChanges(userId);


                    }
                    retVal.Add(new SaleReturn { msg = "Done", value = 0 });
                    sb.Append("Sumitted");
                    //result = "Done";
                    return Json(result, JsonRequestBehavior.AllowGet); 


                }
                catch (Exception ex)
                {
                    sb.Append(sb.Append("Error :" + ex.Message));
                    retVal.Add(new SaleReturn { msg = "error:" + ex.Message, value = 0 });
                }


            }
            catch (Exception ex)
            {
                retVal.Add(new SaleReturn { msg = "error:" + ex.Message, value = 0 });
            }

            //return Json(retVal, JsonRequestBehavior.AllowGet);
            sb.Append("Sumitted");
            result = "Success! Materials Saved";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // POST: Stores/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stores ObjPurchase = db.Store.Find(id);
            if (ObjPurchase == null)
            {
                return HttpNotFound();
            }
            return View(ObjPurchase);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {


                Stores ObjStores = db.Store.Find(id);

                StoresMaterials ObjStoreItems = db.StoreMaterial.FirstOrDefault(i => i.store == ObjStores);

                // Invoice ObjInvoice = db.Invoices.FirstOrDefault(i => i.Id == ObjInvoiceItems.InvoiceId);

                RawMaterialStock ObjProductStock = db.RawMaterialStocks.FirstOrDefault(i => i.Id == ObjStoreItems.rawmaterialStockId);

                Transaction ObjTransaction = db.Transactions.FirstOrDefault(i => i.Id == ObjStoreItems.TransactionId);


                if (ObjStoreItems.InventoryTypeId == 7)
                {
                    var selectedProduct = db.RawMaterial.FirstOrDefault(i => i.Id == ObjStoreItems.RawMaterialsId);
                    selectedProduct.RemainingQuantity = selectedProduct.RemainingQuantity - ObjStoreItems.Quantity;
                    selectedProduct.RemainingAmount = selectedProduct.RemainingAmount - (ObjStoreItems.Total);

                    db.Entry(selectedProduct).State = EntityState.Modified;
                    db.SaveChanges(userId);
                }


                db.RawMaterialStocks.Remove(ObjProductStock);

                db.Transactions.Remove(ObjTransaction);

                db.StoreMaterial.Remove(ObjStoreItems);
                db.Store.Remove(ObjStores);


                db.SaveChanges(userId);

                try
                {
                    //if double antry of purchase or purchase retrun in transaction 
                    Transaction ObjTran2 = db.Transactions.FirstOrDefault(i => i.PurchaseOrSale == "Purchase" && i.PurchaseIdOrSaleId == id);
                    db.Transactions.Remove(ObjTran2);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                }

                sb.Append("Sumitted");
                return Content(sb.ToString());

            }
            catch (Exception ex)
            {
                sb.Append("Error :" + ex.Message);
            }

            return Content(sb.ToString());

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

                //foreach (Stores product in fileData)
                    foreach (Product stores in fileData)
                {
                    Stores updateStores = db.Store.Where(i => i.Id == stores.Id).FirstOrDefault();

                    //updateStores.Id = stores.Id;
                    updateStores.Name = stores.Name;
                    //updateStores.ProductCategory = stores.ProductCategory;
                    updateStores.BarCode = stores.BarCode;
                    //updateStores.SalePrice = product.SalePrice;
                    //updateProduct.RtgsPrice = product.RtgsPrice;
                    updateStores.PurchasePrice = stores.PurchasePrice;
                    updateStores.RemainingQuantity = stores.RemainingQuantity;
                    db.Entry(updateStores).State = EntityState.Modified;
                    db.SaveChanges();

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

        private List<Product> GetDataFromCSVFile(Stream stream)
        {
            var empList = new List<Product>();
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
                                //ProductCategory = Convert.ToString(objDataRow["ProductCategory"].ToString()),
                                BarCode = Convert.ToString(objDataRow["Bar Code"].ToString()),
                                PurchasePrice = Convert.ToDecimal(objDataRow["Purchase Price"].ToString()),
                                RemainingQuantity = Convert.ToDecimal(objDataRow[" RemainingQuantity"].ToString()),

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
    }
}
