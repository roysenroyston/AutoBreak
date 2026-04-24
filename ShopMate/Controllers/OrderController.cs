using Newtonsoft.Json;
using ShopMate.ModelDto;
using ShopMate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebErrorLogging.Utilities;
using System.Security.Claims;
using System.Threading;


namespace ShopMate.Controllers
{
    public class OrderController : BaseController
    {
        string userId = Env.GetUserInfo("name");
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StockShippingOrder()
        {
            return View();
        }
        //public ActionResult PurchaseOrder()
        //{
        //    return View();
        //}
        // GET Order/GetGrid
        public ActionResult GetGrid()
        {
            var tak = db.Orders.ToArray();

            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),

            Convert.ToString(c.goods),
            Convert.ToString(c.supplier),
            Convert.ToString(c.purchasedate)

            };

            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult PurchaseOrderGetGrid()
        //{
        //    int warehouses = int.Parse(Env.GetUserInfo("WarehouseId"));
        //    var warehouse = db.Warehouses.ToArray();
        //    var user = db.Users.ToArray();
        //    var tak = db.Purchases.Where(i => i.WarehouseId == warehouses).ToArray();


        //    var result = from c in tak
        //                 select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
        //    Convert.ToString(c.User_VendorUserId.UserName),
        //    Convert.ToString(c.Product_ProductId.Name),
        //    Convert.ToString(c.Quantity),
        //    Convert.ToString(c.ReturnedQuantity),
        //    Convert.ToString(c.UnitPrice),
        //    Convert.ToString(c.TotalAmount),
        //    Convert.ToString(c.DateAdded),
        //   Convert.ToString(user.FirstOrDefault(i=>i.Id==c.AddedBy).UserName),
        //    Convert.ToString(warehouse.FirstOrDefault(i=>i.Id==c.WarehouseId).Name),
        //    Convert.ToString(c.InventoryTypeId),

        //    };

        //    return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetGridStockShippingOrder()
        {
            var tak = db.StockShippingOrders.ToArray();

            var result = from c in tak
                         select new string[] {

                          c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(db.Warehouses.FirstOrDefault(i => i.Id == c.WarehouseFrom).Name),
            Convert.ToString(db.Warehouses.FirstOrDefault(i => i.Id == c.WarehouseTo).Name),
            //Convert.ToString(c.IsDispatched),
            Convert.ToString(c.IsReceived),
            Convert.ToString(c.IsDeleted),
            Convert.ToString(c.DateAdded),
            Convert.ToString(db.Users.FirstOrDefault(i => i.Id ==c.AddedBy).UserName)

            };


            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        //GetStockshippingOrderItems

        public ActionResult GetStockshippingOrderItems(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var ObjSaleOrdertems = db.StockShippingOrderItems.Where(i => i.StockShippingOrderId == (id)).ToArray();
                List<StockShippingOrderItemDto> lstOrderSaleItem = new List<StockShippingOrderItemDto>();
                foreach (var item in ObjSaleOrdertems)
                {
                    StockShippingOrderItemDto orderItem = new StockShippingOrderItemDto();
                    orderItem.ProductId = item.ProductId;
                    orderItem.Name = item.Product_ProductId.Name;
                    orderItem.Description = item.Product_ProductId.ProductDescription;
                    orderItem.Quantity = item.Quantity;
                    orderItem.Status = "Good";

                    lstOrderSaleItem.Add(orderItem);

                }
                if (ObjSaleOrdertems == null)
                {
                    return HttpNotFound();
                }
                var result = JsonConvert.SerializeObject(lstOrderSaleItem, Formatting.Indented,
                               new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                               });

                return Json(result, JsonRequestBehavior.AllowGet);
                //return Json(new { saleorderItems = ObjSaleOrdertems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        // GET: /Order/ModelBindIndex
        public ActionResult ModelBindIndex()
        {
            return View();
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order ObjOrder = db.Orders.Find(id);
          
            if (ObjOrder == null)
            {
                return HttpNotFound();
            }
            return View(ObjOrder);
        }
        public ActionResult ShippingOrderDetails(int? id)
        {
            StockShippingOrder dnote = db.StockShippingOrders.Find(id);
            var DnoteItems = db.StockShippingOrderItems.Where(q => q.StockShippingOrderId == id).ToArray();

            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);

            if (dnote == null)
            {
                return HttpNotFound();
            }

            InternalDNoteDto dto = new InternalDNoteDto();
            dto.WarehouseFrom = db.Warehouses.FirstOrDefault(i => i.Id == dnote.WarehouseFrom).Name;
            dto.WarehouseTo = db.Warehouses.FirstOrDefault(i => i.Id == dnote.WarehouseTo).Name;
            dto.OrderNumber = dnote.Id;
            dto.Date = DateTime.Now;

            List<InternalDNoteMaterialDto> itemsList = new List<InternalDNoteMaterialDto>();

            foreach (var items in DnoteItems)
            {
                InternalDNoteMaterialDto itemDto = new InternalDNoteMaterialDto();
                itemDto.Name = db.Products.FirstOrDefault(i => i.Id == items.ProductId).Name;
                itemDto.Quantity = items.Quantity;
                itemDto.Description = db.Products.FirstOrDefault(i => i.Id == items.ProductId).ProductDescription;

                itemsList.Add(itemDto);
            }

            dto.items = itemsList;
            var ObjOrder = db.StockShippingOrders.Where(i => i.Id == (id) && i.IsReceived == false).ToArray();
            if (ObjOrder == null)
            {
                return HttpNotFound();
            }
            // return View(dto);
            return Json(new { StockShippingOrder = ObjOrder }, JsonRequestBehavior.AllowGet);
        }
        // GET: Order/StockShippingOrderCreate
        public ActionResult StockShippingOrderCreate()
        {
            ViewBag.Warehouse = new SelectList(db.Warehouses, "Id", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");

            return View();
        }
        [HttpGet]
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockShippingOrder dnote = db.StockShippingOrders.Find(id);
            var DnoteItems = db.StockShippingOrderItems.Where(q => q.StockShippingOrderId == id).ToArray();

            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
            if (dnote == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.StockShippingOrders, "Id", "Name", dnote.Id);
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "Id", "Name");
            //IEnumerable<FinishedGoods> finishedGoods = db.FinishedGoods.Where(qt => qt.Id.Equals(ObjFinishedGoods.Id));
            //ObjFinishedGoods. = finishedGoods;
            InternalDNoteDto dto = new InternalDNoteDto();
            dto.WarehouseFrom = db.Warehouses.FirstOrDefault(i => i.Id == dnote.WarehouseFrom).Name;
            dto.WarehouseTo = db.Warehouses.FirstOrDefault(i => i.Id == dnote.WarehouseTo).Name;
            dto.OrderNumber = dnote.Id;
            dto.Date = DateTime.Now;
            List<InternalDNoteMaterialDto> itemsList = new List<InternalDNoteMaterialDto>();

            foreach (var items in DnoteItems)
            {
                InternalDNoteMaterialDto itemDto = new InternalDNoteMaterialDto();
                itemDto.Name = db.Products.FirstOrDefault(i => i.Id == items.ProductId).Name;
                itemDto.Quantity = items.Quantity;
                itemDto.Description = db.Products.FirstOrDefault(i => i.Id == items.ProductId).ProductDescription;

                itemsList.Add(itemDto);
            }

            dto.items = itemsList;

            return View(dto);
        


        }
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string retVal = "";
            try
            {
                var itemdeleted = db.StockShippingOrders.FirstOrDefault(i => i.Id == id);
               // var itemRecieved = db.StockShippingOrders.
                if (itemdeleted.IsDeleted == true || itemdeleted.IsReceived == true )
                {
             
                    retVal = "Order has been Received or Deleted!!";
                    return Json(retVal, JsonRequestBehavior.AllowGet);
                }

                StockShippingOrder dnote = db.StockShippingOrders.Find(id);
                var DnoteItems = db.StockShippingOrderItems.Where(q => q.StockShippingOrderId == id).ToArray();

                int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
                var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);

                if (dnote == null)
                {
                    return HttpNotFound();
                }

                InternalDNoteDto dto = new InternalDNoteDto();
                dto.WarehouseFrom = db.Warehouses.FirstOrDefault(i => i.Id == dnote.WarehouseFrom).Name;
                dto.WarehouseTo = db.Warehouses.FirstOrDefault(i => i.Id == dnote.WarehouseTo).Name;
                dto.OrderNumber = dnote.Id;
                dto.Date = DateTime.Now;

                List<InternalDNoteMaterialDto> itemsList = new List<InternalDNoteMaterialDto>();

                foreach (var items in DnoteItems)
                {
                    InternalDNoteMaterialDto itemDto = new InternalDNoteMaterialDto();
                    itemDto.Name = db.Products.FirstOrDefault(i => i.Id == items.ProductId).Name;
                    itemDto.Quantity = items.Quantity;
                    itemDto.Description = db.Products.FirstOrDefault(i => i.Id == items.ProductId).ProductDescription;

                    var StkItem = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == items.ProductId && i.WarehouseId == warehouse);
                
                    itemsList.Remove(itemDto);
                    StkItem.RemainingQuantity = StkItem.RemainingQuantity + items.Quantity;
                    db.Entry(StkItem).State = EntityState.Modified;
                    db.SaveChanges(userId);

                    

                }
                StockShippingOrder complete = db.StockShippingOrders.Where(n => n.Id == id).FirstOrDefault();
               // complete.IsReceived = true;
                complete.IsDeleted = true;
                db.Entry(complete).State = EntityState.Modified;
                db.SaveChanges(userId);

                //dto.items = itemsList;

       
                retVal = "Submitted!!";
                return Json(retVal, JsonRequestBehavior.AllowGet);
                //sb.Append("Sumitted");
                //  return Content(sb.ToString());
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }

            return Content(sb.ToString());

        }
        [HttpPost]
        public ActionResult StockShippingOrderEdit()
        {
            ViewBag.Warehouse = new SelectList(db.Warehouses, "Id", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult StockShippingOrderCreate(StockShippingOrder ObjOrder, OrderMat[] OrderMaterials)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (var ngoni in OrderMaterials)
            {

                string result = "Error! Order Is Not Complete!";
                int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));      
                //Get the current claims principal
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                // Get the claims values
                int warehouse = Int16.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.Actor)
                                   .Select(c => c.Value).SingleOrDefault());
                var product = db.Products.FirstOrDefault(i => i.Id == ngoni.ProductId);
                var warehouseStock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == ngoni.ProductId && i.WarehouseId == warehouse);
                if (warehouseStock.RemainingQuantity < ngoni.Quantity && product.RemainingQuantity < ngoni.Quantity)
                {

                    //sb.Append("Add stock to perform this Shipping");
                    //sb.Append("error");
                    //sb.Append("Not enough stock for " + product.Name + " to Shop");

                   // return Content(sb.ToString());
                    return Content(sb.ToString());
                    //sb.Append("not enough stock");

                }
            
                else
                {


                    try
                    {

                        if (ModelState.IsValid)
                        {
                            ObjOrder.DateAdded = DateTime.Now;
                            ObjOrder.DateModified = DateTime.Now;
                            ObjOrder.AddedBy = AddedBy;
                            ObjOrder.WarehouseId = warehouse;
                            if (ObjOrder.WarehouseFrom == ObjOrder.WarehouseTo)
                            {
                                sb.Append("Warehouse From and Warehouse To cannot be the same!!");

                                return Content(sb.ToString());
                            }
                            db.StockShippingOrders.Add(ObjOrder);
                            db.SaveChanges();

                            foreach (var item in OrderMaterials)
                            {
                                StockShippingOrderItem materials = new StockShippingOrderItem();
                                var ObjWarehouseStock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == item.ProductId && i.WarehouseId == warehouse);

                                if (ObjWarehouseStock.RemainingQuantity < item.Quantity)
                                {
                                    return Content(sb.ToString());
                                }

                                //db.SaveChanges();

                                //if (ObjOrder.WarehouseTo == 1)
                                //{
                                //    var wareto = db.WarehouseStocks.FirstOrDefault(w => w.Id==1);
                                //    wareto.RemainingQuantity = ObjWarehouseStock.RemainingQuantity + item.Quantity;
                                //    db.Entry(ObjWarehouseStock).State = EntityState.Modified;
                                //    db.SaveChanges();                         
                                //}
                                //else
                                //{
                                //} 
                                //if (ObjWarehouseStock.RemainingQuantity < item.Quantity)
                                //{
                                //    sb.Append("not enough stock");
                                //}
                                else
                                {
                                    materials.StockShippingOrderId = ObjOrder.Id;
                                    materials.ProductId = item.ProductId;
                                    materials.Quantity = item.Quantity;
                                    db.StockShippingOrderItems.Add(materials);
                                    ObjWarehouseStock.RemainingQuantity = ObjWarehouseStock.RemainingQuantity - item.Quantity;
                                    db.Entry(ObjWarehouseStock).State = EntityState.Modified;
                                    db.SaveChanges();
                                }

                            }

                            result = "Success! Order Is Completed!";
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
                }
            }



            return Content(sb.ToString());
        }
        //public ActionResult PurchaseOrderCreate()
        //{
        //    ViewBag.VendorUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Supplier"), "Id", "UserName");
        //    ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");

        //    return View();
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult PurchaseOrderCreate(PurchaseOrder ObjOrder, OrderMat[] OrderMaterials)
        //{
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    string result = "Error! Order Is Not Complete!";
        //    int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));
        //    int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            ObjOrder.DateAdded = DateTime.Now;
        //            ObjOrder.DateModified = DateTime.Now;
        //            ObjOrder.AddedBy = AddedBy;
        //            ObjOrder.WarehouseId = warehouse;

        //            db.PurchaseOrders.Add(ObjOrder);
        //            db.SaveChanges();
        //            foreach (var item in OrderMaterials)
        //            {
        //                PurchaseOrderItem materials = new PurchaseOrderItem();

        //                materials.PurchaseOrderId = ObjOrder.Id;
        //                materials.ProductId = item.ProductId;
        //                materials.Quantity = item.Quantity;
        //                db.PurchaseOrderItems.Add(materials);
        //                db.SaveChanges();
        //            }


        //            result = "Success! Order Is Completed!";
        //            return Json(result, JsonRequestBehavior.AllowGet);
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
        //        Helper.WriteError(ex, ex.Message);
        //        sb.Append("Error :" + ex.Message);
        //    }

        //    return Content(sb.ToString());
        //}

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.Purchases, "Id", "Name");
            ViewBag.userId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Supplier"), "Id", "UserName");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Order ObjOrder, OrderMaterials[] OrderrMaterials, bool IsDispatched)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! Order Is Not Complete!";
            try
            {
                if (ModelState.IsValid)
                {
                    ObjOrder.purchasedate = DateTime.Now;
                    db.Orders.Add(ObjOrder);
                    db.SaveChanges();
                    foreach (var item in OrderrMaterials)
                    {
                        OrderMaterials materials = new OrderMaterials();
                        materials.Description = item.Description;
                        materials.Order = ObjOrder;
                        materials.OrderId = ObjOrder.Id;
                        materials.Quantity = item.Quantity;
                        materials.IsDispatched = item.IsDispatched;
                        db.OrderMaterial.Add(materials);
                        db.SaveChanges();
                    }


                    result = "Success! Order Is Completed!";
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

        // POST: /Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult Create(Order ObjOrder)
        //{
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {


        //            db.Orders.Add(ObjOrder);
        //            db.SaveChanges(userId);

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
        // GET: Order/Dispatch/5
        //public ActionResult Dispatch(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    StockShippingOrder ObjOrder = db.StockShippingOrders.Find(id);
        //    if (ObjOrder == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(ObjOrder);
        //}

        public ActionResult Dispatch(int? id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           
            StockShippingOrder ObjOrder = db.StockShippingOrders.Find(id);
            var ObjOrderItems = db.StockShippingOrderItems.Where(i => i.StockShippingOrderId == ObjOrder.Id).ToArray();
            

            if (ObjOrder.IsDispatched == true)
            {
                sb.Append("Stock Order has already been dispatched!!");

                return Content(sb.ToString());
            }
            
            foreach (var item in ObjOrderItems)
            {
                var product = db.Products.FirstOrDefault(i => i.Id == item.ProductId);
                var selectedObjOrder = db.Orders.FirstOrDefault(i => i.Id == ObjOrder.Id);
                if (product.RemainingQuantity < item.Quantity)
                {
                    sb.Append("Not enough stock for " + product.Name + " to Dispatch");

                    return Content(sb.ToString());
                }
                else
                {
                    var StkItem = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == item.ProductId && i.WarehouseId == ObjOrder.Warehouse);
                    StkItem.RemainingQuantity = StkItem.RemainingQuantity - item.Quantity;
                    db.Entry(StkItem).State = EntityState.Modified;
                    sb.Append("Dispatch successful");
                    

                }

            }
           
            ObjOrder.IsDispatched = true;
           
            db.SaveChanges(userId);
           

            return Content(sb.ToString());
            //return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order ObjOrder = db.Orders.Find(id);
            if (ObjOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Orders, "Id", "Name", ObjOrder.Id);

            return View(ObjOrder);
        }

        // POST: Order/Edit/5        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Order ObjOrder)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(ObjOrder).State = EntityState.Modified;
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

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order ObjOrder = db.Orders.Find(id);
            if (ObjOrder == null)
            {
                return HttpNotFound();
            }
            return View(ObjOrder);
        }

        // POST: /Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {

                Order ObjOrder = db.Orders.Find(id);
                db.Orders.Remove(ObjOrder);
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

        // GET: /Order/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            Order ObjOrder = db.Orders.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;
                ViewBag.ParentId = new SelectList(db.Orders, "Id", "MenuText", ObjOrder.Id);

            }

            return View(ObjOrder);
        }

        // POST: /Order/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(Order ObjOrder)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Entry(ObjOrder).State = EntityState.Modified;
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

        public ActionResult print(int id)
        {
            StockShippingOrder dnote = db.StockShippingOrders.Find(id);
            var DnoteItems = db.StockShippingOrderItems.Where(q => q.StockShippingOrderId == id).ToArray();

            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);

            if (dnote == null)
            {
                return HttpNotFound();
            }

            InternalDNoteDto dto = new InternalDNoteDto();
            dto.WarehouseFrom = db.Warehouses.FirstOrDefault(i => i.Id == dnote.WarehouseFrom).Name;
            dto.WarehouseTo = db.Warehouses.FirstOrDefault(i => i.Id == dnote.WarehouseTo).Name;
            dto.OrderNumber = dnote.Id;
            dto.Date = DateTime.Now;

            List<InternalDNoteMaterialDto> itemsList = new List<InternalDNoteMaterialDto>();

            foreach (var items in DnoteItems)
            {
                InternalDNoteMaterialDto itemDto = new InternalDNoteMaterialDto();
                itemDto.Name = db.Products.FirstOrDefault(i => i.Id == items.ProductId).Name;
                itemDto.Quantity = items.Quantity;
                itemDto.Description = db.Products.FirstOrDefault(i => i.Id == items.ProductId).ProductDescription;

                itemsList.Add(itemDto);
            }

            dto.items = itemsList;

            return View(dto);

        }

        public class OrderMat
        {

            public int ProductId { get; set; }

            public decimal Quantity { get; set; }


            public string Name { get; set; }


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
