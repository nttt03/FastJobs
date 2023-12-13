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
    public class AccountController : Controller
    {
        dbFastJobsDataContext db = new dbFastJobsDataContext();


        
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountManager()
        {
            return View();
        }
        [HttpGet]
        public ActionResult update()
        {
            User user = (User)Session["Account"];
            var userToUpdate = db.Users.SingleOrDefault(n => n.ID == user.ID);
            if (userToUpdate == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(userToUpdate);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult update(User userToUpdate, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            User user = (User)Session["Account"];
            userToUpdate = db.Users.SingleOrDefault(n => n.ID == user.ID);

            if (ModelState.IsValid)
            {
                if (fFileUpload != null) //Kiểm tra để xác nhận cho thay đổi ảnh bìa
                {
                    //Lấy tên file (Khai báo thư viện: System.I0)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    // Kiểm tra file đã tồn tại chưa
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);

                    }
                    userToUpdate.Avatar = sFileName;
                }
                
                userToUpdate.Name = f["sName"];
                userToUpdate.Phone = f["sPhone"];
                userToUpdate.Email = f["sEmail"];
                userToUpdate.Address = f["sAddress"];
                userToUpdate.ModifiedDate = DateTime.Now;
                //db.Users.InsertOnSubmit(userToUpdate);
                db.SubmitChanges();
                TempData["message"] = "Cập nhật thành công!";            
            }
            return View(userToUpdate);
        }

        public ActionResult JobsRecument()
        {
            User user = (User)Session["Account"];
            var listRecument = from Recument in db.Recuments
                           join Job in db.Jobs on Recument.JobID equals Job.ID
                           join Company in db.Companies on Job.CompanyID equals Company.ID
                           where Recument.UserID == user.ID
                           select new JobUser
                           {
                               jobdata = Job,
                               companydata = Company,
                               recumentdata = Recument,
                           };

            if (listRecument == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(listRecument);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            Employer emp = (Employer)Session["AccountEmployer"];
            var empToEdit = db.Employers.SingleOrDefault(n => n.ID == emp.ID);
            if (empToEdit == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(empToEdit);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            Employer emp = (Employer)Session["AccountEmployer"];
            var empToEdit = db.Employers.SingleOrDefault(n => n.ID == emp.ID);

            if (ModelState.IsValid)
            {
                if (fFileUpload != null) //Kiểm tra để xác nhận cho thay đổi ảnh bìa
                {
                    //Lấy tên file (Khai báo thư viện: System.I0)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    // Kiểm tra file đã tồn tại chưa
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);

                    }
                    empToEdit.Avatar = sFileName;
                }

                empToEdit.Name = f["sName"];
                empToEdit.Phone = f["sPhone"];
                empToEdit.Email = f["sEmail"];
                empToEdit.Address = f["sAddress"];
                empToEdit.ModifiedDate = DateTime.Now;
                db.SubmitChanges();
                TempData["message"] = "Cập nhật thành công!";
                //Về lại trang
                //return RedirectToAction("Index");
            }
            return View(empToEdit);
        }

    }
}
