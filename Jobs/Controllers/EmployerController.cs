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
    public class EmployerController : Controller
    {
        dbFastJobsDataContext db = new dbFastJobsDataContext();
        // GET: NhaTuyenDung
        public ActionResult IndexEmployer()
        {
            return View();
        }

        public ActionResult NavPartial()
        {
            return PartialView();
        }

        public ActionResult SliderPartial()
        {
            return PartialView();
        }

        public ActionResult LoginLogout()
        {
            return PartialView();
            //return Redirect(url);
        }

        [HttpGet]
        public ActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateCompany(Company company, FormCollection f, HttpPostedFileBase fFileLogo, HttpPostedFileBase fFileBackground)
        {
            Employer emp = (Employer)Session["AccountEmployer"];

            if (fFileLogo == null)
            {
                ViewBag.ThongBao = "Hãy chọn Logo của công ty.";
                // Lưu thông tin để khi load lại trang do yêu cầu chọn ảnh bìa sẽ hiển thị các thông tin này lên trang
                ViewBag.Name = f["sName"];
                ViewBag.LinkPage = f["sLinkPage"];
                ViewBag.Description = f["sDescription"];
                ViewBag.Employees = int.Parse(f["sEmployees"]);
                ViewBag.Location = f["sLocation"];
                ViewBag.Avatar = fFileLogo;
                ViewBag.Background = fFileBackground;
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileLogo = Path.GetFileName(fFileLogo.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileLogo);
                    //Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                    if (!System.IO.File.Exists(path))
                    {
                        fFileLogo.SaveAs(path);
                    }
                    
                    if (fFileBackground != null)
                    {
                        var sFileBgr = Path.GetFileName(fFileBackground.FileName);
                        var pathbgr = Path.Combine(Server.MapPath("~/Images"), sFileBgr);
                        if (!System.IO.File.Exists(pathbgr))
                        {
                            fFileBackground.SaveAs(pathbgr);
                        }
                        company.Background = sFileBgr;
                    }
                    company.Name = f["sName"];
                    company.LinkPage = f["sLinkPage"];
                    company.Description = f["sDescription"];
                    company.Employees = int.Parse(f["sEmployees"]);
                    company.Location = f["sLocation"];
                    company.Avatar = sFileLogo;
                    company.CreatedDate = DateTime.Now;
                    //company.Background = sFileBgr;

                    db.Companies.InsertOnSubmit(company);
                    db.SubmitChanges();

                    Employer empl = new Employer();
                    empl.IDCompany = company.ID;
                    db.Employers.InsertOnSubmit(empl);
                    db.SubmitChanges();

                    EmployerCreatedCompany createCompany = new EmployerCreatedCompany();
                    createCompany.EmployerID = emp.ID;
                    createCompany.CompanyID = company.ID;
                    db.EmployerCreatedCompanies.InsertOnSubmit(createCompany);
                    db.SubmitChanges();

                    TempData["message"] = "Tạo công ty thành công!";
                    ViewBag.ThongBao = "Tạo công ty thành công!";
                    //return RedirectToAction("Index");
                }
                return View();

            }

        }

        [HttpGet]
        public ActionResult updateCompany()
        {
            Employer emp = (Employer)Session["AccountEmployer"];
            var CompanyToUpdate = db.Companies.SingleOrDefault(n => n.ID == emp.IDCompany);
            if (CompanyToUpdate == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(CompanyToUpdate);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateCompany(FormCollection f, HttpPostedFileBase fFileLogo, HttpPostedFileBase fFileBackground)
        {
            Employer emp = (Employer)Session["AccountEmployer"];
            var CompanyToUpdate = db.Companies.SingleOrDefault(n => n.ID == emp.IDCompany);

            if (ModelState.IsValid)
            {
                if (fFileLogo != null) //Kiểm tra để xác nhận cho thay đổi ảnh bìa
                {
                    //Lấy tên file (Khai báo thư viện: System.I0)
                    var sFileName = Path.GetFileName(fFileLogo.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    // Kiểm tra file đã tồn tại chưa
                    if (!System.IO.File.Exists(path))
                    {
                        fFileLogo.SaveAs(path);

                    }
                    CompanyToUpdate.Avatar = sFileName;
                }
                if (fFileBackground != null) //Kiểm tra để xác nhận cho thay đổi ảnh bìa
                {
                    //Lấy tên file (Khai báo thư viện: System.I0)
                    var sFileName = Path.GetFileName(fFileBackground.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    // Kiểm tra file đã tồn tại chưa
                    if (!System.IO.File.Exists(path))
                    {
                        fFileBackground.SaveAs(path);

                    }
                    CompanyToUpdate.Background = sFileName;
                }
                

                CompanyToUpdate.Name = f["sName"];
                CompanyToUpdate.LinkPage = f["sLinkPage"];
                CompanyToUpdate.Description = f["sDescription"];
                CompanyToUpdate.Employees = int.Parse(f["sEmployees"]);
                CompanyToUpdate.Location = f["sLocation"];
                CompanyToUpdate.ModifiedDate = DateTime.Now;

                db.SubmitChanges();
                TempData["message"] = "Cập nhật thành công!";
                //Về lại trang
                //return RedirectToAction("Index");
            }
            return View(CompanyToUpdate);
        }


        [HttpGet]
        public ActionResult CreateJobs()
        {
            
            Employer emp = (Employer)Session["AccountEmployer"];
            
            ViewBag.CategoryID = new SelectList(db.JobCategories.ToList().OrderBy(n => n.TypeJob), "ID", "TypeJob");
            ViewBag.CareerID = new SelectList(db.Careers.ToList().OrderBy(n => n.Name), "ID", "Name");
            
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateJobs(Job job, FormCollection f)
        {
            Employer emp = (Employer)Session["AccountEmployer"];
            ViewBag.CategoryID = new SelectList(db.JobCategories.ToList().OrderBy(n => n.TypeJob), "ID", "TypeJob");
            ViewBag.CareerID = new SelectList(db.Careers.ToList().OrderBy(n => n.Name), "ID", "Name");
            //var listCompany = db.EmployerCreatedCompanies.Where(c => c.EmployerID == emp.ID).ToList();
            //foreach (var item in listCompany)
            //{
            //    ViewBag.CompanyID = new SelectList(db.Companies.Where(l => l.ID == item.CompanyID), "ID", "Name").ToList();
            //}


            if (ModelState.IsValid)
            {
                
                job.Name = f["sName"];
                job.Description = f["sDescription"];
                job.RequestCandidate = f["sRequestCandidate"];
                job.SalaryMin = decimal.Parse(f["dSalaryMin"]);
                job.SalaryMax = decimal.Parse(f["dSalaryMax"]);
                job.Details = f["sDetails"];
                job.CategoryID = int.Parse(f["CategoryID"]);
                job.CareerID = int.Parse(f["CareerID"]);
                job.Experience = f["sExperience"];
                job.CompanyID = emp.IDCompany;
                job.Gender = job.Gender;
                job.WorkLocation = f["sWorkLocation"];
                job.CreatedDate = DateTime.Now;
                job.UserID = emp.ID;
                db.Jobs.InsertOnSubmit(job);
                db.SubmitChanges();

                EmployerCreatedJob createJobs = new EmployerCreatedJob();
                createJobs.EmployerID = emp.ID;
                createJobs.JobID = job.ID;
                db.EmployerCreatedJobs.InsertOnSubmit(createJobs);
                db.SubmitChanges();

                TempData["message"] = "Đăng tuyển thành công!";
                ViewBag.Success = "Đăng tuyển thành công!";

                return RedirectToAction("CreateJobs");
            }
            return View();

        }

        [HttpGet]
        public ActionResult CreateJo()
        {
            return View();
        }
    }
}