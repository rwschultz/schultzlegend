using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SchultzLegendWebSite.Models;
using SchultzLegendWebSite.Utilities;
using SchultzLegendWebSite.Filters;

namespace SchultzLegendWebSite.Controllers
{
    [HttpsFilter]
    public class AccountController : BaseController
    {
        // GET: Account
        [Authorize]
        public ActionResult Index()
        {
            AccountViewModel model = new AccountViewModel();

            return View(model);
        }

        [Route("ChangePassword")]
        [Authorize]
        public ActionResult ChangePassword()
        {
            AccountViewModel model = new AccountViewModel();

            return View(model);
        }

        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            AccountViewModel model = new AccountViewModel();

            return View(model);
        }

        [Route("Login")]
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            AccountViewModel model = new AccountViewModel();

            return View(model);
        }
        
        [Route("Register")]
        public ActionResult Register()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            AccountViewModel model = new AccountViewModel();

            return View(model);
        }

        [Route("ConfirmEmail")]
        public RedirectResult ConfirmEmail(string token = null)
        {
            AccountViewModel model = new AccountViewModel();
            if(token == null)
            {
                return Redirect("/Login");
            }

            UserLogin user = DBHelper.DBGetUserLoginFromToken(token);
            if(user != null && user.Id == token && !user.EmailConfirmed)
            {
                DBHelper.DBConfirmUserEmail(user.Email);
                return Redirect("/Login");
            }
            else
            {
                return Redirect("/Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public JsonResult Register(string email, string password, string confpassword)
        {
            
            if(!email.Contains("@"))
            {
                return Json(new { Error = "email:invalid" }, JsonRequestBehavior.AllowGet);
            }
            else if(email.Length > 70)
            {
                return Json(new { Error = "email:long" }, JsonRequestBehavior.AllowGet);
            }
            if(password.Length < 8)
            {
                return Json(new { Error = "password:short" }, JsonRequestBehavior.AllowGet);
            }
            else if(!password.Any(char.IsDigit))
            {
                return Json(new { Error = "password:number" }, JsonRequestBehavior.AllowGet);
            }
            else if (!password.Any(char.IsUpper))
            {
                return Json(new { Error = "password:upper" }, JsonRequestBehavior.AllowGet);
            }
            else if (!password.Any(char.IsLower))
            {
                return Json(new { Error = "password:lower" }, JsonRequestBehavior.AllowGet);
            }
            else if (password != confpassword)
            {
                return Json(new { Error = "password:mismatch" }, JsonRequestBehavior.AllowGet);
            }

            string phash = LoginHelper.CreateHash(password);
            DBHelper.DBCreateUserLogin(email, phash);
            UserLogin user = DBHelper.DBGetUserLogin(email);
            UserLogin.SendConfirmEmail(user);

            return Json(new {Error = "none"}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public JsonResult Login(string email, string password, bool remember)
        {
            UserLogin user = DBHelper.DBGetUserLogin(email);
            if (user != null &&
                user.EmailConfirmed &&
                LoginHelper.ValidatePassword(password, user.PasswordHash))
            {
                bool persistant = remember;

                HttpCookie cookie = FormsAuthentication.GetAuthCookie(email, persistant);
                cookie.Secure = true;
                cookie.HttpOnly = true;
                HttpContext.Response.Cookies.Add(cookie);
                return Json(new { Error = "none" }, JsonRequestBehavior.AllowGet);
            }
            else if(user != null && !user.EmailConfirmed)
            {
                return Json(new { Error = "confirm" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Error = "invalid" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ChangePassword")]
        public JsonResult ChangePassword(string curpassword, string newpassword, string confpassword)
        {
            UserLogin user = DBHelper.DBGetUserLogin(HttpContext.User.Identity.Name);
            if (!LoginHelper.ValidatePassword(curpassword, user.PasswordHash))
            {
                return Json(new { Error = "password:invalid" }, JsonRequestBehavior.AllowGet);
            }
            if (newpassword.Length < 8)
            {
                return Json(new { Error = "password:short" }, JsonRequestBehavior.AllowGet);
            }
            else if (!newpassword.Any(char.IsDigit))
            {
                return Json(new { Error = "password:number" }, JsonRequestBehavior.AllowGet);
            }
            else if (!newpassword.Any(char.IsUpper))
            {
                return Json(new { Error = "password:upper" }, JsonRequestBehavior.AllowGet);
            }
            else if (!newpassword.Any(char.IsLower))
            {
                return Json(new { Error = "password:lower" }, JsonRequestBehavior.AllowGet);
            }
            else if (newpassword != confpassword)
            {
                return Json(new { Error = "password:mismatch" }, JsonRequestBehavior.AllowGet);
            }

            string phash = LoginHelper.CreateHash(newpassword);
            DBHelper.DBCChangeUserPassword(user.Email, phash);

            return Json(new { Error = "none" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public void ForgotPassword(string email = null)
        {
            UserLogin.SendResetPasswordEmail(email);
        }

        [Route("SignOut")]
        public RedirectResult SignOut(string from = null)
        {
            FormsAuthentication.SignOut();

            return Redirect(from ?? "/");
        }
    }
}