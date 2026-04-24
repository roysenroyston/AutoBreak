using ShopMate.ModelDto;
using ShopMate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebErrorLogging.Utilities;

namespace ShopMate.Controllers
{
    public class GRVController : Controller
    {
        // GET: GRV
        public ActionResult Index()
        {
            return View();
        }

        //Get: GRV/GetGrid
        public ActionResult GetGrid()
        {
            var tak = db.GRVs.ToArray();

            var result = from c in tak
                         select new string[]
                {
                    c.Id.ToString(), Convert.ToString(c.Id),
                     //Convert.ToString(c.supplier),
                     Convert.ToString(c.receivedby),
                    Convert.ToString(c.OrderNumber),
                    Convert.ToString(c.purchasedate),
                    Convert.ToString(c.Description),
                    Convert.ToString(c.Quantity),
                    //Convert.ToString(c.UnitPrice),
                    //Convert.ToString(c.TotalPrice),
                };

            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        // GET: /GRV/ModelBindIndex
        public ActionResult ModelBindIndex()
        {
            return View();
        }

        // GET: GRV/Details/5
        public ActionResult Details(int? id)
        {
            GRV ObjGRV = db.GRVs.Find(id);
            var materials = db.GRVMaterials.Where(grv => grv.GRVId == id);
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
       
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //GRV ObjGRV = db.GRVs.Find(id);
            //var materials = db.GRVMaterials.Where(grv => grv.GRVId == id);
            if (ObjGRV == null)
            {
                return HttpNotFound();
            }
            //int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            //var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
          




            GRVDto dto = new GRVDto();
            dto.Id = ObjGRV.Id;
            dto.OrderNumber = (int)ObjGRV.OrderNumber;
            dto.purchasedate = ObjGRV.purchasedate;
            dto.receivedby = ObjGRV.receivedby;
            //dto.supplier = ObjGRV.supplier;
            dto.CompanyAddress = invoiceFormat.AddressInfo;
            dto.CompanyContact = invoiceFormat.OtherInfo;
            //dto.Warehouse = db.Warehouses.FirstOrDefault(i => i.Id == warehouse).Name;
            dto.Warehouse = db.Warehouses.FirstOrDefault(i => i.Id == ObjGRV.Warehouse).Name;

            //List<GRVMaterialsDto> materialsDtos = new List<GRVMaterialsDto>();
            List<GRVMaterialsDto> itemsList = new List<GRVMaterialsDto>();

            foreach (var item in materials)
            {
             
                GRVMaterialsDto gRV = new GRVMaterialsDto();
                //gRV.Name = item.Product;
                gRV.Name = db.Products.FirstOrDefault(i => i.Id == item.ProductId).Name;
                gRV.Description = item.Description;
                gRV.Quantity = item.Quantity;
                

                itemsList.Add(gRV);
            }

            dto.GRVMaterials = itemsList;

            return View(dto);
        }


        // GET: GRV/Create
        public ActionResult Create()
        {
            ViewBag.OrderNumber = new SelectList(db.Orders, "Id", "Goods");
            ViewBag.OrderNumber = new SelectList(db.StockShippingOrders.Where(i => i.IsReceived == false && i.IsDeleted == false), "Id", "Id");
            //ViewBag.CustomerUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Supplier"), "Id", "FullName");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "Id", "Name");

            return View();
        }

        // POST: GRV/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult Create(GRV objGRV)
        //{
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {


        //            db.GRVs.Add(objGRV);
        //            db.SaveChanges();

        //            sb.Append("Sumitted");
        //            return Content(sb.ToString());
        //        }
        //        else
        //        {
        //            foreach (var key in this.ViewData.ModelState.Keys)
        //            {
        //                foreach (var err in this.ViewData.ModelState[key].Errors)
        //                {
        //                    sb.Append(err.ErrorMessage + "<br/>");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sb.Append("Error :" + ex.Message);
        //    }

        //    return Content(sb.ToString());
        //}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(GRV objGRV, GRVMaterials[] GRVMaterials, int? OrderNumber)
        {
         
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! Order Is Not Complete!";
            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
          
            //int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
           

            try
            {
                if (OrderNumber == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (ModelState.IsValid)
                {
                    objGRV.purchasedate = DateTime.Now;
                    objGRV.OrderNumber = OrderNumber;
                  
                    //objGRV.receivedby = AddedBy;
                    //objGRV.Warehouse = warehouse;
                    db.GRVs.Add(objGRV);
                    db.SaveChanges();

                    var ObjSaleOrdertems = db.StockShippingOrderItems.Where(i => i.StockShippingOrderId == (OrderNumber)).ToArray();
                    List<StockShippingOrderItemDto> lstOrderSaleItem = new List<StockShippingOrderItemDto>();
                   
                    foreach (var item in GRVMaterials)
                    {

                        GRVMaterials gRVMaterials = new GRVMaterials();
                        gRVMaterials.ProductId = item.ProductId;
                        gRVMaterials.Quantity = item.Quantity;
                        gRVMaterials.Status = item.Status;
                        gRVMaterials.Description = item.Description;
                        gRVMaterials.GRVId = objGRV.Id;

                        db.GRVMaterials.Add(gRVMaterials);
                        db.SaveChanges();

                        //StockShippingOrderItemDto orderItem = new StockShippingOrderItemDto();
                        //orderItem.ProductId = item.ProductId;
                        //orderItem.Name = item.Name;
                        //orderItem.Description = item.Description;
                        //orderItem.Quantity = item.Quantity;
                        //orderItem.Status = "Good";

                        //lstOrderSaleItem.Add(orderItem);
                        //db.SaveChanges();



                        var ObjWarehouseStock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == item.ProductId && i.WarehouseId == objGRV.Warehouse);
                        var SelectedProduct = db.Products.FirstOrDefault(i => i.Id == item.ProductId);
                        var selectedStockShippingOrder = db.StockShippingOrders.FirstOrDefault(i => i.Id == OrderNumber);
                        if (ObjWarehouseStock.WarehouseId == 1)
                        {
                            //SelectedProduct.RemainingQuantity = SelectedProduct.RemainingQuantity - item.Quantity;//dispatch
                            ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity + item.Quantity;// other warestocks
                            db.Entry(ObjWarehouseStock).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else //if (ObjWarehouseStock.WarehouseId == 5 || ObjWarehouseStock.WarehouseId == 6)
                        {                         
                            ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity + item.Quantity;
                            db.Entry(ObjWarehouseStock).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        //SelectedProduct.RemainingQuantity = SelectedProduct.RemainingQuantity - item.Quantity;//dispatch
                        //ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity + item.Quantity;// other warestocks
                        //db.Entry(ObjWarehouseStock).State = EntityState.Modified;
                        //db.SaveChanges();
                        //var SelectedProduct = db.Products.FirstOrDefault(i => i.Id == item.ProductId);
                        ////SelectedProduct.RemainingQuantity = SelectedProduct.RemainingQuantity + item.Quantity;
                        //SelectedProduct.RemainingAmount = SelectedProduct.RemainingAmount + (item.Quantity * SelectedProduct.SalePrice);
                        //db.Entry(SelectedProduct).State = EntityState.Modified;
                        //db.SaveChanges();
                    }

                    if (objGRV.OrderNumber > 0)
                    {
                        var selectedOrder = db.StockShippingOrders.FirstOrDefault(i => i.Id == objGRV.OrderNumber);
                        selectedOrder.IsReceived = true;
                        db.Entry(selectedOrder).State = EntityState.Modified;
                        db.SaveChanges();
                    
                    }
                    result = "Success! GRV Is Completed!";
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

        // GET: GRV/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GRV ObjGRV = db.GRVs.Find(id);
            if (ObjGRV == null)
            {
                return HttpNotFound();
            }

            return View(ObjGRV);
        }

        // POST: GRV/Edit/5
        [HttpPost]
        public ActionResult Edit(GRV ObjGRV)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjGRV).State = EntityState.Modified;
                    db.SaveChanges();

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

        public ActionResult print(int id,int? OrderNumber)
        {
            //GRV gnote = db.GRVs.Find(id);
            //var grvitems = db.GRVMaterials.Where(q => q.GRVId == id).ToArray();
            //StockShippingOrder dnote = db.StockShippingOrders.Find(id);
            //var DnoteItems = db.StockShippingOrderItems.Where(q => q.StockShippingOrderId == id).ToArray();
            GRV gnote = db.GRVs.Where(t => t.Id == id).FirstOrDefault();
            var grvitems = db.GRVMaterials.Where(q => q.GRVId == id).ToArray();
            
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
            var materials = db.GRVMaterials.Where(grv => grv.GRVId == id);
            
            if (gnote == null )
            {
                return HttpNotFound();
            }

         
            GRVDto dto = new GRVDto();
            //dto.Date = DateTime.Now;
            dto.receivedby = gnote.receivedby;
            dto.Warehouse = db.Warehouses.FirstOrDefault(i => i.Id == gnote.Warehouse).Name;
            var ObjSaleOrdertems = db.StockShippingOrderItems.Where(i => i.StockShippingOrderId == (id)).ToArray();
    
            List<GRVMaterialsDto> itemsList = new List<GRVMaterialsDto>();

            foreach (var items in grvitems)
            {
                GRVMaterialsDto itemDto = new GRVMaterialsDto();
                itemDto.Name = db.Products.FirstOrDefault(i => i.Id == items.ProductId).Name;
                itemDto.Description = db.Products.FirstOrDefault(i => i.Id == items.ProductId).ProductDescription;
                itemDto.Quantity = items.Quantity;
               
                //itemDto.Status = items.Status;

                itemsList.Add(itemDto);
            }

            dto.GRVMaterials = itemsList;

            return View(dto);

        }

      
        // GET: GRV/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GRV ObjGRV = db.GRVs.Find(id);
            var materials = db.GRVMaterials.Where(grv => grv.GRVId == id);
            if (ObjGRV == null)
            {
                return HttpNotFound();
            }

            GRVDto dto = new GRVDto();
            dto.Id = ObjGRV.Id;
            //dto.OrderNumber = (int)ObjGRV.OrderNumber;
            dto.purchasedate = ObjGRV.purchasedate;
            dto.receivedby = ObjGRV.receivedby;
            //dto.supplier = ObjGRV.supplier;

            List<GRVMaterialsDto> materialsDtos = new List<GRVMaterialsDto>();

            foreach (var item in materials)
            {
                GRVMaterialsDto gRV = new GRVMaterialsDto();
                gRV.Description = item.Description;
                gRV.Quantity = item.Quantity;
                gRV.Id = item.Id;

                materialsDtos.Add(gRV);
            }

            dto.GRVMaterials = materialsDtos;

            return View(dto);

        }
      

        // POST: GRV/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {

                GRV ObjGRV = db.GRVs.Find(id);
                db.GRVs.Remove(ObjGRV);
                db.SaveChanges();

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

        // GET: /GRV/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            GRV ObjGRV = db.GRVs.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;

            }

            return View(ObjGRV);
        }

        // POST: /GRV/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(GRV ObjGRV)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjGRV).State = EntityState.Modified;
                    db.SaveChanges();

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
