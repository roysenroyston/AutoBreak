using ShopMate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebErrorLogging.Utilities;

namespace ShopMate.Controllers
{
    public class AccountController : Controller
    {
        //
        SIContext db = new SIContext();
        // GET: /Account/
        public ActionResult login()
        {
           
            if (Env.GetUserInfo("name").Length > 0)
                return Redirect("~/Home/Index");
            else
                return View();
           
        }

        [HttpPost]
        public ActionResult login(System.Web.Mvc.FormCollection frmCollection)
        {
            try { 
            string email = frmCollection["email"].ToString();
            string password = frmCollection["password"].ToString();
                //string api_key = frmCollection["api_key"].ToString();

                User login = db.Users.FirstOrDefault(i => i.UserName == email && i.CanLogin==true);

            login = BCrypt.Net.BCrypt.Verify(password, login.Password) ? login : null;

            try
            {
                    //if(api_key != null)
                    //{
                    //    if (login != null)
                    //    {
                    //        //true
                    //        Response.StatusCode = 200;
                    //    }
                    //    else
                    //    {
                    //        Response.StatusCode = 403;
                    //        Response.ContentType = "Application/Json";

                    //    }

                    //}
                    DateTime dateOfJoining = (DateTime)login.JoinDate; // Example
                    // Calculate time difference
                    TimeSpan timeDifference = DateTime.Now - dateOfJoining;

                    // Check if one year has passed
                    if (timeDifference.TotalDays >= 365)
                    {
                        ModelState.AddModelError(string.Empty, "You are not allowed to log in as one year has passed since your date of joining.");
                        ViewBag.Msg = "Your Account Expired, Contact 0783 284 440";
                    }
                    else
                    {
                        if (login != null)
                        {
                            var claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, login.UserName.ToString())); // store username of user
                            claims.Add(new Claim(ClaimTypes.Role, login.RoleId.Value.ToString()));
                            claims.Add(new Claim(ClaimTypes.Sid, login.Id.ToString())); // store id of user
                            if (login.WarehouseId != null)
                            {
                                claims.Add(new Claim(ClaimTypes.Actor, login.WarehouseId.ToString()));//WarehouseId
                            }
                            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                            var authenticationManager = Request.GetOwinContext().Authentication;
                            authenticationManager.SignIn(identity);

                            var claimsPrincipal = new ClaimsPrincipal(identity);
                            Thread.CurrentPrincipal = claimsPrincipal;


                            if (login.RoleId == 1 || login.RoleId == 6)
                            {
                                return Redirect("~/Home/Index");
                            }
                            else
                            {
                                return Redirect("~/pos/Index");
                            }
                        }
                        else
                        {
                            ViewBag.Msg = "!Invalid UserName and Password";
                        }
                    }
                    
            }
            catch (InvalidOperationException ex)
            {
                Helper.WriteError(ex, ex.Message);
                return View(ex.Message);
               

            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                return View("System encountered an unknown error.");
            }
            }
            catch (Exception ex)
            {
                Helper.WriteError(ex, ex.Message);
                ViewBag.Msg = "!Invalid UserName and Password";
            }
            return View();
        }

        public ActionResult register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult register(System.Web.Mvc.FormCollection frmCollection)
        {
            User user = new User();
            user.UserName = frmCollection["name"];
            user.Password = frmCollection["password2"];
            db.Users.Add(user);
             
            ViewBag.Msg = "Register Successfully";
            return View();
        }

        public ActionResult signout()
        {
            AuthenticationManager.SignOut(); 
            HttpCookie c = new HttpCookie(".AspNet.ApplicationCookie");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c); 

            HttpCookie d = new HttpCookie("__RequestVerificationToken");
            d.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(d);

            return RedirectToAction("login", "Account");
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
		
		public ActionResult unauthorized()
        {
            return View();
        }
    }
}

