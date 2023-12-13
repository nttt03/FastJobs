using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobs.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace Jobs.Controllers
{
    public class JobsUserController : Controller
    {
        dbFastJobsDataContext data = new dbFastJobsDataContext();
        // GET: JobsUser
        [HttpGet]
        public ActionResult Index(int id)
        {
            
            var job = from Job in data.Jobs
                         join Company in data.Companies on Job.CompanyID equals Company.ID
                         join JobCategory in data.JobCategories on Job.CategoryID equals JobCategory.ID
                         where Job.ID == id
                         select new JobUser
                         {
                             jobdata = Job,
                             companydata = Company,
                             jobcategorydata = JobCategory,


                         };
            
            if (job == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //ewBag.idCompany = id;
            return View(job);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Apply(Recument recument, FormCollection f, int id)
        {
            User user = (User)Session["Account"];

            if (user == null)
            {
                return RedirectToAction("SignIn", "SigninSignup");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (f["sLetter"] != null)
                    {
                        recument.LetterInfo = f["sLetter"];
                        recument.Name = user.Name;
                        recument.Phone = user.Phone;
                        recument.Email = user.Email;
                        recument.Address = user.Address;
                        recument.UserID = user.ID;
                        recument.JobID = id;
                        recument.Status = 0;
                        recument.CreateDate = DateTime.Now;
                        
                    }
                    else
                    {
                        // userCV = (CV)Session["UserCV"];
                        var userCV = data.CVs.SingleOrDefault(n => n.IDUser == user.ID);
                        if (userCV == null)
                        {
                            ViewBag.Thongbao = "Bạn chưa tạo CV!";
                            return RedirectToAction("Index", "JobsUser", new { id = id });
                        }
                        else
                        {
                            recument.Name = user.Name;
                            recument.Phone = user.Phone;
                            recument.Email = user.Email;
                            recument.Address = user.Address;
                            recument.UserID = user.ID;
                            recument.JobID = id;
                            recument.Status = 0;
                            recument.CreateDate = DateTime.Now;
                            recument.CVID = userCV.ID;
                            
                        }
                    }
                    data.Recuments.InsertOnSubmit(recument);
                    data.SubmitChanges();
                    TempData["message2"] = "Ứng tuyển thành công!";
                }
            }
            return RedirectToAction("Index", "JobsUser", new { id = id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ApplyPartial(Recument recument, FormCollection f)
        {
            User user = (User)Session["Account"];
           

            if (user == null)
            {
                return RedirectToAction("SignIn", "SigninSignup");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (f["sLetter"] != null)
                    {
                        recument.LetterInfo = f["sLetter"];
                        recument.Name = user.Name;
                        recument.Phone = user.Phone;
                        recument.Email = user.Email;
                        recument.Address = user.Address;
                        recument.UserID = user.ID;
                        recument.JobID = ViewBag.idCompany;
                        recument.Status = 0;
                        recument.CreateDate = DateTime.Now;
                    }
                    else
                    {
                        
                        CV userCV = (CV)Session["UserCV"];
                        if (userCV == null)
                        {
                            ViewBag.Thongbao = "Bạn chưa tạo CV!";
                        }
                        else
                        {
                            recument.Name = user.Name;
                            recument.Phone = user.Phone;
                            recument.Email = user.Email;
                            recument.Address = user.Address;
                            recument.UserID = user.ID;
                            recument.JobID = ViewBag.idCompany;
                            recument.Status = 0;
                            recument.CreateDate = DateTime.Now;
                            recument.CVID = userCV.ID;
                        }
                    }
                    data.Recuments.InsertOnSubmit(recument);
                    data.SubmitChanges();
                    TempData["message2"] = "Ứng tuyển thành công!";
                }
            }
            return PartialView();
        }
        
    }
}