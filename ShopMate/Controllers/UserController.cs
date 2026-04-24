using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ShopMate.Models;
using System.Data.SqlClient;
using WebErrorLogging.Utilities;
using BCrypt.Net;

namespace ShopMate.Controllers
{
    public class UserController : BaseController
    {
        //int userId = Convert.ToInt32(Env.GetUserInfo("userid"));
        string userId = Env.GetUserInfo("name");
        string wareid = Env.GetUserInfo("WarehouseId");
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetUserDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User ObjUser = db.Users.Find(id);
            if (ObjUser == null)
            {
                return HttpNotFound();
            }
            //return Json(new { price = ObjProduct.SalePrice, description = ObjProduct.ProductDescription, Name = ObjProduct.Name, taxId = ObjProduct.TaxId, remainingquantity = ObjProduct.RemainingQuantity }, JsonRequestBehavior.AllowGet);
            return Json(new { name = ObjUser.FullName, vatNumber = ObjUser.vatNumber,  }, JsonRequestBehavior.AllowGet);
        }
        // GET User/GetGrid
        public ActionResult UserProfile(int? id)
        {
            var myid = db.Users.FirstOrDefault(n => n.UserName == userId).Id;
            id = myid;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User ObjUser = db.Users.Find(id);
            if (ObjUser == null)
            {
                return HttpNotFound();
            }
            return View(ObjUser);
        }
        public ActionResult GetGrid()
        {


            //var tak = db.Users.Where(i =>i.Role_RoleId.RoleName == "Admin" || i.Role_RoleId.RoleName == "Manager" || i.Role_RoleId.RoleName == "SaleMan" || i.Role_RoleId.RoleName == "Supervisor").ToArray();
            var tak = db.Users.Where(i => i.CanLogin == true).ToArray();
            var userwarehouse = db.Users.FirstOrDefault(n => n.UserName == userId).WarehouseId;
            if (userId == "Zimhope")
            {
                var result = from c in tak
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.UserName), 
           // Convert.ToString(c.Password), 
            Convert.ToString(c.FullName),
            Convert.ToString(c.Mobile),
            Convert.ToString(c.Address),
             Convert.ToString(db.Warehouses.FirstOrDefault(m=> m.Id ==c.WarehouseId).Name),
            Convert.ToString(c.About),
            Convert.ToString(c.Role_RoleId.RoleName),
            Convert.ToString(c.JoinDate),
            Convert.ToString(c.IsActive),
            Convert.ToString(c.CanLogin),
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = from c in tak.Where(i=>i.WarehouseId==userwarehouse)
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.UserName), 
           // Convert.ToString(c.Password), 
            Convert.ToString(c.FullName),
            Convert.ToString(c.Mobile),
            Convert.ToString(c.Address),
            Convert.ToString(c.About),
            Convert.ToString(c.Role_RoleId.RoleName),
            Convert.ToString(c.JoinDate),
            Convert.ToString(c.IsActive),
            Convert.ToString(c.CanLogin),
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult Artisan()
        {
            return View();
        }
        public ActionResult GetGridArtisan()
        {
            var tak = db.Users.Where(i => i.Role_RoleId.RoleName == "Artisan").ToArray();

            var result = from c in tak
                         select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.UserName),
            Convert.ToString(c.vatNumber),
            Convert.ToString(c.FullName),
            Convert.ToString(c.Mobile),
            Convert.ToString(c.Address),
            Convert.ToString(c.About),
            Convert.ToString(c.Role_RoleId.RoleName),
            Convert.ToString(c.JoinDate),
            Convert.ToString(c.IsActive),
            Convert.ToString(c.CanLogin),
             };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateVendor()
        {
            var userCustomers = db.Users.FirstOrDefault(n => n.UserName == userId).WarehouseId;
            if (userId == "ZImhope")
            {
                  ViewBag.WareHouse = new SelectList(db.Warehouses, "Id", "Name");
                ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName");
            }
            else
            {
                ViewBag.WareHouse = new SelectList(db.Warehouses.Where(n => n.Id == userCustomers), "Id", "Name");
                ViewBag.RoleId = new SelectList(db.Roles.Where(n => n.Id != 1), "Id", "RoleName");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateVendor(User ObjUser)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int warehouse = Convert.ToInt32(wareid);
            try
            {
                if (ModelState.IsValid)
                {
                    ObjUser.WarehouseId = warehouse;
                    ObjUser.CanLogin = false;
                    db.Users.Add(ObjUser);
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

        public ActionResult EditCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User ObjUser = db.Users.Find(id);
            if (ObjUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", ObjUser.RoleId);

            return View(ObjUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditCustomer(User ObjUser)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {
                    string password = ObjUser.Password;

                    string passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);

                    // ObjUser.JoinDate = DateTime.Now;
                    ObjUser.Password = hashedPassword;
                    //ObjUser.RoleId = 3;


                    db.Entry(ObjUser).State = EntityState.Modified;
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

        //get : user/editsupplier
        public ActionResult EditSupplier(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User ObjUser = db.Users.Find(id);
            if (ObjUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles.Where(n => n.Id == 3), "Id", "RoleName", ObjUser.RoleId);

            return View(ObjUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditSupplier(User ObjUser)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {
                    string password = ObjUser.Password;

                    string passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);

                    // ObjUser.JoinDate = DateTime.Now;
                    ObjUser.Password = hashedPassword;
                    ObjUser.RoleId = 3;


                    db.Entry(ObjUser).State = EntityState.Modified;
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



        //vendor
        public ActionResult CreateArtisan()
        {
            if (userId == "ZImhope")
            {
                ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName");
            }
            else
            {
                ViewBag.RoleId = new SelectList(db.Roles.Where(n => n.Id != 1), "Id", "RoleName");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateArtisan(User ObjUser)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                if (ModelState.IsValid)
                {


                    db.Users.Add(ObjUser);
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
        //get create customer
        public ActionResult CreateCustomer()
        {
            var userCustomers = db.Users.FirstOrDefault(n => n.UserName == userId).WarehouseId;
            if (userId == "ZImhope")
            {
                ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName");
                ViewBag.WareHouse = new SelectList(db.Warehouses, "Id", "Name");
            }
            else
            {
                ViewBag.WareHouse = new SelectList(db.Warehouses.Where(n => n.Id == userCustomers), "Id", "Name");
                ViewBag.RoleId = new SelectList(db.Roles.Where(n => n.Id != 1), "Id", "RoleName");
            }

            return View();
        }
        //create customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateCustomer(User ObjUser)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int warehouse = Convert.ToInt32(wareid);
            try
            {
                if (ModelState.IsValid)
                {
                    ObjUser.WarehouseId = warehouse;
                    ObjUser.CanLogin = false;
                    db.Users.Add(ObjUser);
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
        public ActionResult Vendor()
        {
            return View();
        }

        public ActionResult GetGridVendor()
        {
            var tak = db.Users.Where(i => i.Role_RoleId.RoleName == "Supplier").ToArray();
            var userCustomers = db.Users.FirstOrDefault(n => n.UserName == userId).WarehouseId;
            if (userCustomers==1)
            {
                var result = from c in tak
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.UserName), 
            //Convert.ToString(c.Password), 
            Convert.ToString(c.FullName),
            Convert.ToString(c.Mobile),
            Convert.ToString(c.Address),
            Convert.ToString(c.About),
            Convert.ToString(c.Role_RoleId.RoleName),
            Convert.ToString(c.JoinDate),
            Convert.ToString(c.IsActive),
            Convert.ToString(c.CanLogin),
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = from c in tak.Where(i=>i.WarehouseId==userCustomers)
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.UserName), 
            //Convert.ToString(c.Password), 
            Convert.ToString(c.FullName),
            Convert.ToString(c.Mobile),
            Convert.ToString(c.Address),
            Convert.ToString(c.About),
            Convert.ToString(c.Role_RoleId.RoleName),
            Convert.ToString(c.JoinDate),
            Convert.ToString(c.IsActive),
            Convert.ToString(c.CanLogin),
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult GetGridCustomer()
        {
            var tak = db.Users.Where(i => i.Role_RoleId.RoleName == "Customer").ToArray();
            var userCustomers = db.Users.FirstOrDefault(n => n.UserName == userId).WarehouseId;
            if (userId == "Zimhope")
            {
                var result = from c in tak
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.UserName), 
            //Convert.ToString(c.Password), 
            Convert.ToString(c.FullName),
            Convert.ToString(c.Mobile),
            Convert.ToString(c.Address),
            Convert.ToString(c.About),
            Convert.ToString(c.Role_RoleId.RoleName),
            Convert.ToString(c.JoinDate),
            Convert.ToString(c.IsActive),
            Convert.ToString(c.CanLogin),
             Convert.ToString(c.credit),
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = from c in tak.Where(i=>i.WarehouseId==userCustomers)
                             select new string[] { c.Id.ToString(), Convert.ToString(c.Id),
            Convert.ToString(c.UserName), 
            //Convert.ToString(c.Password), 
            Convert.ToString(c.FullName),
            Convert.ToString(c.Mobile),
            Convert.ToString(c.Address),
            Convert.ToString(c.About),
            Convert.ToString(c.Role_RoleId.RoleName),
            Convert.ToString(c.JoinDate),
            Convert.ToString(c.IsActive),
            Convert.ToString(c.CanLogin),
             Convert.ToString(c.credit),
             };
                return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);

            }
        }


        // GET: /User/ModelBindIndex
        public ActionResult ModelBindIndex()
        {
            return View();
        }
        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User ObjUser = db.Users.Find(id);
            if (ObjUser == null)
            {
                return HttpNotFound();
            }
            return View(ObjUser);
        }
        // GET: /User/Create
        public ActionResult Create()
        {
            if (userId == "ZImhope")
            {
                ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName");
            }
            else
            {
                ViewBag.RoleId = new SelectList(db.Roles.Where(n => n.Id != 1), "Id", "RoleName");
            }


             return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(User ObjUser )
        {
            //var roleId = db.Roles.Where(i => i.Id == 1 || i.Id ==2 ).Count();
            var ngoni = db.Users.Where(n => n.RoleId == 1 || n.RoleId== 2 && n.WarehouseId == ObjUser.WarehouseId).Count();
            var userLimit = db.Warehouses.FirstOrDefault(o => o.Id == ObjUser.WarehouseId).NumberOfUsers;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(); 
            try
            {
                if ( userLimit == null ) { sb.Append("Please Contact Zimhope for User Limit"); return Content(sb.ToString()); }
                    if (ngoni > userLimit)
                    {
                        sb.Append("Please subscribe for more  users");
                        return Content(sb.ToString());
                    } 
                if (ModelState.IsValid)
                {
                    string password = ObjUser.Password;

                    string passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);

                    ObjUser.JoinDate = DateTime.Now;
                    ObjUser.Password = hashedPassword;

                    db.Users.Add(ObjUser);
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
        [HttpPost]
        public void AcountPayment(int Id, decimal amount)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try
            {
                //  ViewBag.ProductCategoryId = db.Products.Where(i => i.ProductCategoryId == category);
                string constring = System.Configuration.ConfigurationManager.ConnectionStrings["SIConnectionString"]
                   .ConnectionString;
                List<Product> products = new List<Product>();

                string qury = "UPDATE User SET credit=credit+'"+amount+"' WHERE Id='" + Id + "'";
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand(qury, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }


            }




            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                sb.Append("Error :" + ex.Message);
            }


            //  return Content(sb.ToString());

        }
        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User ObjUser = db.Users.Find(id);
            if (ObjUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", ObjUser.RoleId);

            return View(ObjUser);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(User ObjUser )
        { 
            System.Text.StringBuilder sb = new System.Text.StringBuilder(); 
            try
            {
                if (ModelState.IsValid)
                {
                    string password = ObjUser.Password;

                    string passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);

                    ObjUser.JoinDate = DateTime.Now;
                    ObjUser.Password = hashedPassword;


                    db.Entry(ObjUser).State = EntityState.Modified;
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
        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User ObjUser = db.Users.Find(id);
            if (ObjUser == null)
            {
                return HttpNotFound();
            }
            return View(ObjUser);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        { 
            System.Text.StringBuilder sb = new System.Text.StringBuilder(); 
            try
            {
                  
                    User ObjUser = db.Users.Find(id);
                    db.Users.Remove(ObjUser);
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
        // GET: /User/MultiViewIndex/5
        public ActionResult MultiViewIndex(int? id)
        { 
            User ObjUser = db.Users.Find(id);
            ViewBag.IsWorking = 0;
            if (id > 0)
            {
                ViewBag.IsWorking = id;
                ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", ObjUser.RoleId);

            }
            
            return View(ObjUser);
        }

        // POST: /User/MultiViewIndex/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MultiViewIndex(User ObjUser )
        {  
            System.Text.StringBuilder sb = new System.Text.StringBuilder(); 
            try
            {
                if (ModelState.IsValid)
                { 
                    

                    db.Entry(ObjUser).State = EntityState.Modified;
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
         public ActionResult MenuPermissionGetGrid(int id=0)
        {
            var tak = db.MenuPermissions.Where(i=>i.UserId==id).ToArray();
             
            var result = from c in tak select new string[] { Convert.ToString(c.Id),Convert.ToString(c.Id),
                Convert.ToString(c.Menu_MenuId.MenuText),Convert.ToString(c.Role_RoleId.RoleName),Convert.ToString(c.UserId),
                Convert.ToString(c.SortOrder),
                Convert.ToString(c.IsCreate),
                Convert.ToString(c.IsRead),
                Convert.ToString(c.IsUpdate),
                Convert.ToString(c.IsDelete),
                 };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
         public ActionResult SaleGetGrid(int id=0)
        {
            var tak = db.Sales.Where(i=>i.CustomerUserId==id).ToArray();
             
            var result = from c in tak select new string[] { Convert.ToString(c.Id),Convert.ToString(c.Id),
                Convert.ToString(c.CustomerUserId),
                Convert.ToString(c.Quantity),
                Convert.ToString(c.SalePrice),
                Convert.ToString(c.PaymentMode_PaymentModeId.Name),Convert.ToString(c.TotalAmount),
                Convert.ToString(c.PaidAmount),
                Convert.ToString(c.Product_ProductId.Name),Convert.ToString(c.DateAdded),
                Convert.ToString(c.ModifiedBy),
                Convert.ToString(c.DateModied),
                Convert.ToString(c.AddedBy),
                 };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
         public ActionResult PurchaseGetGrid(int id=0)
        {
            var tak = db.Purchases.Where(i=>i.VendorUserId==id).ToArray();
             
            var result = from c in tak select new string[] { Convert.ToString(c.Id),Convert.ToString(c.Id),
                Convert.ToString(c.VendorUserId),
                Convert.ToString(c.Product_ProductId.Name),Convert.ToString(c.Quantity),
                Convert.ToString(c.UnitPrice),
                Convert.ToString(c.TotalAmount),
                Convert.ToString(c.DateAdded),
                Convert.ToString(c.AddedBy),
                 };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
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

