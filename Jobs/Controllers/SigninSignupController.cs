using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobs.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.Optimization;
using System.Web.Security;
using System.Data.Common;

namespace Jobs.Controllers
{
    public class SigninSignupController : Controller
    {
        dbFastJobsDataContext db = new dbFastJobsDataContext();
        // GET: SigninSignup
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SignIn(FormCollection collection)
        {
            var sUsername = collection["Username"];
            var sPass = collection["Pass"];
            var sID = collection["ID"];
            if (String.IsNullOrEmpty(sUsername))
            {
                ViewData["Err1"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sPass))
            {
                ViewData["Err2"] = "Bạn phải nhập mật khẩu";
            }
            else
            {
                User user = db.Users.SingleOrDefault(n => n.UserName == sUsername && n.Password == sPass);
                if (user != null)
                {
                    
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    
                    Session["Account"] = user;
                    //Session["IDUser"] = User.Identity.GetUserId();
                    return RedirectToAction("Index", "Jobs");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("Account");
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult LogoutEmployer()
        {
            Session.Remove("AccountEmployer");
            FormsAuthentication.SignOut();
            return RedirectToAction("IndexEmployer", "Employer");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(FormCollection collection, User user)
        {
            var sName = collection["Name"];
            var sUsername = collection["Username"];
            var sPass = collection["Pass"];
            var sPassRepeat = collection["PassRepeat"];
            var sEmail = collection["Email"];

            if (String.IsNullOrEmpty(sUsername))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sUsername))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }     
            else if (String.IsNullOrEmpty(sPass))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sPassRepeat))
            {
                ViewData["err4"] = "Mật khẩu nhập lại không đúng";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            } 
            else if (db.Users.SingleOrDefault(n => n.UserName == sUsername) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.Users.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                user.Name = sName;
                user.UserName = sUsername;
                user.Password = sPass;
                user.Email = sEmail;
                user.CreatedDate = DateTime.Now;
                db.Users.InsertOnSubmit(user);
                db.SubmitChanges();
                return RedirectToAction("SignIn");
            }
            return this.SignUp();
        }

        [HttpGet]
        public ActionResult SignInEmployer()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SignInEmployer(FormCollection collection)
        {
            var sUsername = collection["Username"];
            var sPass = collection["Pass"];
            var sID = collection["ID"];
            if (String.IsNullOrEmpty(sUsername))
            {
                ViewData["Err1"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sPass))
            {
                ViewData["Err2"] = "Bạn phải nhập mật khẩu";
            }
            else
            {
                Employer emp = db.Employers.SingleOrDefault(n => n.UserName == sUsername && n.Password == sPass);
                if (emp != null)
                {

                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";

                    Session["AccountEmployer"] = emp;
                    //Session["IDUser"] = User.Identity.GetUserId();
                    return RedirectToAction("IndexEmployer", "Employer");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult SignUpEmployer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpEmployer(FormCollection collection, Employer emp)
        {
            var sName = collection["Name"];
            var sUsername = collection["Username"];
            var sPass = collection["Pass"];
            var sPassRepeat = collection["PassRepeat"];
            var sEmail = collection["Email"];

            if (String.IsNullOrEmpty(sUsername))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sUsername))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sPass))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sPassRepeat))
            {
                ViewData["err4"] = "Mật khẩu nhập lại không đúng";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if (db.Employers.SingleOrDefault(n => n.UserName == sUsername) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.Employers.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                emp.Name = sName;
                emp.UserName = sUsername;
                emp.Password = sPass;
                emp.Email = sEmail;
                emp.CreatedDate = DateTime.Now;
                db.Employers.InsertOnSubmit(emp);
                db.SubmitChanges();
                return RedirectToAction("SignInEmployer");
            }
            return this.SignUp();
        }

    }
}