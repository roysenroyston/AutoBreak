using Newtonsoft.Json;
using ShopMate.ModelDto;
using ShopMate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;

using System.Web.Mvc;
using WebErrorLogging.Utilities;

namespace ShopMate.Controllers
{
    public class DispatchController : Controller
    {
        SIContext db = new SIContext();
        string userId = Env.GetUserInfo("name");

        // GET: Dispatch
        public ActionResult Index()
        {
            return View();
        }
        // GET Dispatch/GetGrid
        public ActionResult GetGrid()
        {
            var tak = db.Dispatches.ToArray();
            var users = db.Users.ToArray();
            var ware = db.Warehouses.ToArray();
            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.DispatchTo),
            Convert.ToString(c.AddedBy) ,
            //Convert.ToString(users.FirstOrDefault(i=>i.Id==c.AddedBy).UserName) ,
            Convert.ToString(c.invoiceNo),
                             //Convert.ToString(ware.FirstOrDefault(i=>i.Id==c.WarehouseId).Name),
                             Convert.ToString(c.WarehouseId) ,
                             Convert.ToString(c.DateAdded) ,

             };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }

        // GET: Dispatch/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dispatch ObjDispatch = db.Dispatches.Find(id);
            if (ObjDispatch == null)
            {
                return HttpNotFound();
            }
            return View(ObjDispatch);
        }

        public ActionResult GetDispatchItems(string id)
        {
            int Id = 0;
            int CustomerId = 0;
            bool IsFormal = true;
            try
            {
                if (id == null)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }
                else
                {
                    string[] broken_str = id.Split(',');
                    Id = int.Parse(broken_str[0]);
                    CustomerId = int.Parse(broken_str[1].Trim());
                }
                var vatNumber = db.Users.FirstOrDefault(i => i.Id == CustomerId).vatNumber;
                if (vatNumber == null)
                {
                    IsFormal = false;
                }
                if (IsFormal)
                {

                }
                var ObjInvoicetems = db.InvoiceItemss.Where(i => i.InvoiceId == (Id)).ToArray();
                var ObjInformalInvoicetems = db.InvoiceItemss.Where(i => i.InformalInvoiceId == Id).ToArray();
                //List<DNoteMaterial> objDNoteMAterial = new List<DNoteMaterial>();
                List<DispatchMaterials> lstDispatchItem = new List<DispatchMaterials>();
                if (IsFormal)
                {
                    foreach (var item in ObjInvoicetems)
                    {
                        //DNoteMaterial InvoiceItem = new DNoteMaterial();
                        //InvoiceItem.ProductId = item.ProductId;
                        ////InvoiceItem. = item.ProductId;
                        //InvoiceItem.Description = db.Products.FirstOrDefault(i => i.Id == item.ProductId).Name + " " + db.Products.FirstOrDefault(i => i.Id == item.ProductId).ProductDescription;
                        //InvoiceItem.Quantity = item.Quantity;
                        ////InvoiceItem.Name = db.Products.FirstOrDefault(i => i.Id == item.ProductId).Name;
                        //lstDispatchItem.Add(InvoiceItem);

                        DispatchMaterials DispatchItem = new DispatchMaterials();
                        DispatchItem.ProductId = item.ProductId;
                        //db.Products.FirstOrDefault(i => i.Id == (item.ProductId)).Name;
                        DispatchItem.Name = item.Product_ProductId.Name;
                        DispatchItem.Quantity = item.RemainingQuantity;
                        DispatchItem.Description = item.Product_ProductId.ProductDescription;
                        //DispatchItem.taxAmount = item.TaxAmount;
                        //DispatchItem.totalAmountWithTax = item.TotalAmountWithTax;
                        //DispatchItem.saleOrderId = item.SaleOrderId;
                        DispatchItem.Description = db.Products.FirstOrDefault(i => i.Id == item.ProductId).ProductDescription;
                        lstDispatchItem.Add(DispatchItem);
                    }
                }
                else
                {
                    foreach (var items in ObjInformalInvoicetems)
                    {
                        DispatchMaterials DispatchItem = new DispatchMaterials();
                        DispatchItem.ProductId = items.ProductId;
                        //db.Products.FirstOrDefault(i => i.Id == (item.ProductId)).Name;
                        DispatchItem.Name = items.Product_ProductId.Name;
                        DispatchItem.Quantity = items.RemainingQuantity;
                        DispatchItem.Description = items.Product_ProductId.ProductDescription;
                        //DispatchItem.taxAmount = item.TaxAmount;
                        //DispatchItem.totalAmountWithTax = item.TotalAmountWithTax;
                        //DispatchItem.saleOrderId = item.SaleOrderId;
                        DispatchItem.Description = db.Products.FirstOrDefault(i => i.Id == items.ProductId).ProductDescription;
                        lstDispatchItem.Add(DispatchItem);
                    }
                }

                if (ObjInvoicetems == null)
                {
                    return HttpNotFound();
                }
                ////var result = JsonConvert.SerializeObject(objDNoteMAterial, Formatting.Indented,
                ////               new JsonSerializerSettings
                ////               {
                ////                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                ////               });

                //////return Json(result, JsonRequestBehavior.AllowGet);
                ////return Json(new { InvoiceItems = objDNoteMAterial }, JsonRequestBehavior.AllowGet);
                var result = JsonConvert.SerializeObject(lstDispatchItem, Formatting.Indented,
                              new JsonSerializerSettings
                              {
                                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                              });

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        // GET: Dispatch/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.Dispatches, "Id", "Name");
            ViewBag.CustomerUserId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Customer"), "Id", "FullName");
            ViewBag.userId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Admin"), "Id", "UserName");
            ViewBag.customerId = new SelectList(db.Users.Where(i => i.Role_RoleId.RoleName == "Customer"), "Id", "UserName");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Create(Dispatch objDispatch, DispatchMaterials[] dispatchmaterials)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string result = "Error! Dispatch  Is Not Complete!";
            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            int AddedBy = Convert.ToInt32(Env.GetUserInfo("userid"));

            try
            {
                if (ModelState.IsValid)
                {
                    objDispatch.DateAdded = DateTime.Now;
                    objDispatch.WarehouseId = warehouse;
                    objDispatch.AddedBy = AddedBy;
                    db.Dispatches.Add(objDispatch);

                    foreach (var item in dispatchmaterials)
                    {
                        var product = db.Products.FirstOrDefault(i => i.Id == item.ProductId);
                        var warehouseStock = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == item.ProductId && i.WarehouseId == warehouse);

                    }
                    db.SaveChanges();
                    foreach (var quoteItems in dispatchmaterials)
                    {
                        if (db.Users.FirstOrDefault(i => i.Id == objDispatch.CustomerUserId).vatNumber == null)
                        {
                            var selectedInvoice = db.InformalInvoices.FirstOrDefault(i => i.Id == objDispatch.invoiceNo);
                            var selectedInformalInvoiceItem = db.InvoiceItemss.FirstOrDefault(i => i.ProductId == quoteItems.ProductId && i.InformalInvoiceId == objDispatch.invoiceNo);
                            var selectedWarehouseStockItem = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == selectedInformalInvoiceItem.ProductId && i.WarehouseId == warehouse);
                            if (selectedInformalInvoiceItem.RemainingQuantity >= quoteItems.Quantity)
                            {
                                DispatchMaterials Items = new DispatchMaterials();
                                Items.ProductId = selectedInformalInvoiceItem.ProductId;
                                Items.Quantity = quoteItems.Quantity;
                                Items.Description = quoteItems.Description;
                                Items.DispatchId = objDispatch.Id;
                                Items.DateAdded = DateTime.Now;
                                Items.AddedBy = AddedBy;
                                Items.WarehouseId = warehouse;
                                db.Dispatchmaterial.Add(Items);
                                //selectedInformalInvoiceItem.RemainingQuantity = selectedInformalInvoiceItem.RemainingQuantity - quoteItems.Quantity;
                                selectedWarehouseStockItem.RemainingQuantity = selectedWarehouseStockItem.RemainingQuantity - quoteItems.Quantity;
                                db.Entry(selectedWarehouseStockItem).State = EntityState.Modified;
                                db.Entry(selectedInformalInvoiceItem).State = EntityState.Modified;
                                db.SaveChanges(userId);
                            }
                            var tak = db.InvoiceItemss.Where(i => i.InformalInvoiceId == selectedInvoice.Id && i.RemainingQuantity > 0).ToArray();
                            //if(tak.Length > 0)
                            if (IsNullOrEmpty(tak))
                            {
                                selectedInvoice.IsDispatched = true;
                                db.Entry(selectedInvoice).State = EntityState.Modified;
                                db.SaveChanges(userId);
                            }
                        }
                        else
                        {
                            var selectedInvoice = db.Invoices.FirstOrDefault(i => i.Id == objDispatch.invoiceNo);
                            var selectedInvoiceItem = db.InvoiceItemss.FirstOrDefault(i => i.ProductId == quoteItems.ProductId && i.InvoiceId == objDispatch.invoiceNo);
                            var selectedWarehouseStockItem = db.WarehouseStocks.FirstOrDefault(i => i.ProductId == selectedInvoiceItem.ProductId && i.WarehouseId == warehouse);
                            if (selectedInvoiceItem.RemainingQuantity >= quoteItems.Quantity)
                            {
                                DispatchMaterials Items = new DispatchMaterials();
                                Items.ProductId = selectedInvoiceItem.ProductId;
                                Items.Quantity = quoteItems.Quantity;
                                Items.Description = quoteItems.Description;
                                Items.DispatchId = objDispatch.Id;
                                Items.DateAdded = DateTime.Now;
                                Items.AddedBy = AddedBy;
                                Items.WarehouseId = warehouse;
                                db.Dispatchmaterial.Add(Items);
                                //selectedInvoiceItem.RemainingQuantity = selectedInvoiceItem.RemainingQuantity - quoteItems.Quantity;
                                selectedWarehouseStockItem.RemainingQuantity = selectedWarehouseStockItem.RemainingQuantity - quoteItems.Quantity;
                                db.Entry(selectedWarehouseStockItem).State = EntityState.Modified;
                                db.Entry(selectedInvoiceItem).State = EntityState.Modified;
                                db.SaveChanges(userId);

                                //db.SaveChanges();
                            }
                            var tak = db.InvoiceItemss.Where(i => i.InvoiceId == selectedInvoice.Id && i.RemainingQuantity > 0).ToArray();
                            if (IsNullOrEmpty(tak))
                            {
                                selectedInvoice.IsDispatched = true;
                                db.Entry(selectedInvoice).State = EntityState.Modified;
                                db.SaveChanges(userId);
                            }
                        }
                    }
                    result = "Success! Dispatch Created";
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
        // GET: Dispatch/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dispatch/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                return View();
            }
        }

        // GET: Dispatch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dispatch/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                return View();
            }
        }
        // GET: /DeliveryNote/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        {
            Dispatch ObjDispatch = db.Dispatches.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;

            }

            return View(ObjDispatch);
        }

        // POST: /Dispatch/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        public ActionResult Print(int id)
        {
            Dispatch dispatch = db.Dispatches.Find(id);
            var DispatchItems = db.Dispatchmaterial.Where(q => q.DispatchId == id).ToArray();

            int warehouse = int.Parse(Env.GetUserInfo("WarehouseId"));
            var invoiceFormat = db.InvoiceFormats.FirstOrDefault(i => i.WarehouseId == warehouse);
            //var user = db.Users.FirstOrDefault(i => i.Id == dispatch.DispatchTo);

            if (dispatch == null)
            {
                return HttpNotFound();
            }

            DispatchDto dto = new DispatchDto
            {
                InvoiceNo = dispatch.invoiceNo,
                DispatchedTo = dispatch.DispatchTo,
                CompanyAddress = invoiceFormat.AddressInfo,
                CompanyContact = invoiceFormat.OtherInfo,
                CompanyName = invoiceFormat.CompanyName,
                //CustomerMinerNo = db.Users.fir
                Id = dispatch.Id,
                Logo = invoiceFormat.Logo,
                // ToInfo = user.Address + "<br/> " + user.Mobile + "<br/> " + user.About
            };

            List<DispatchMaterialsDto> itemsList = new List<DispatchMaterialsDto>();

            foreach (var items in DispatchItems)
            {
                DispatchMaterialsDto itemDto = new DispatchMaterialsDto
                {
                    Description = items.Product_ProductId.Name,
                    Quantity = items.Quantity
                };


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
        public static bool IsNullOrEmpty<T>(T[] array)
        {
            return array == null || array.Length == 0;
        }
    }
}
