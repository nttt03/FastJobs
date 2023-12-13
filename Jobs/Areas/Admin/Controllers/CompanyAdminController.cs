using Jobs.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Web.UI.WebControls;

namespace Jobs.Areas.Admin.Controllers
{
    public class CompanyAdminController : Controller
    {
        // GET: Admin/CompanyAdmin
        dbFastJobsDataContext db = new dbFastJobsDataContext();

        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 5;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMess = TempData["result"];
            }

            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(db.Companies.ToList().OrderBy(n => n.ID).ToPagedList(iPageNum, iPageSize)); }
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(); }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Company company, FormCollection f, HttpPostedFileBase avatar, HttpPostedFileBase background)
        {
            ViewBag.Name = f["Name"];
            ViewBag.LinkPage = f["LinkPage"];
            ViewBag.Description = f["Description"];
            ViewBag.Employees = f["Employees"];
            ViewBag.Location = f["Location"];
            ViewBag.CreatedDate = f["CreatedDate"];
            ViewBag.ModifiedDate = f["ModifiedDate"];

            if (avatar == null || background == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh đại diện/ ảnh bìa!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileNameAva = Path.GetFileName(avatar.FileName);
                    var sFileNameBa = Path.GetFileName(background.FileName);

                    var pathAva = Path.Combine(Server.MapPath("~/Images"), sFileNameAva);
                    var pathBa = Path.Combine(Server.MapPath("~/Images"), sFileNameBa);

                    if (!System.IO.File.Exists(pathAva))
                    {
                        avatar.SaveAs(pathAva);
                    }

                    if (!System.IO.File.Exists(pathBa))
                    {
                       background.SaveAs(pathBa);
                    }

                    company.Name = f["Name"];
                    company.LinkPage = f["LinkPage"];
                    company.Description = f["Description"];
                    company.Employees = int.Parse(f["Employees"]);
                    company.Location = f["location"];
                    company.CreatedDate = Convert.ToDateTime(f["CreatedDate"]);
                    company.ModifiedDate = Convert.ToDateTime(f["ModifiedDate"]);
                    company.Avatar = sFileNameAva;
                    company.Background = sFileNameBa;
                    db.Companies.InsertOnSubmit(company);
                    db.SubmitChanges();
                    TempData["result"] = "Thêm công ty thành công!";
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var company = db.Companies.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(company); }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection f, HttpPostedFileBase avatar, HttpPostedFileBase background)
        {
            var company = db.Companies.SingleOrDefault(n => n.ID == id);

            if (ModelState.IsValid)
            {
                if (avatar != null || background != null)
                {
                    var sFileNameAva = Path.GetFileName(avatar.FileName);
                    var sFileNameBa = Path.GetFileName(background.FileName);

                    var pathAva = Path.Combine(Server.MapPath("~/Images"), sFileNameAva);
                    var pathBa = Path.Combine(Server.MapPath("~/Images"), sFileNameBa);

                    if (!System.IO.File.Exists(pathAva))
                    {
                        avatar.SaveAs(pathAva);
                    }

                    if (!System.IO.File.Exists(pathBa))
                    {
                        background.SaveAs(pathBa);
                    }
                    company.Avatar = sFileNameAva;
                    company.Background = sFileNameBa;
                }
                company.Name = f["Name"];
                company.LinkPage = f["LinkPage"];
                company.Description = f["Description"];
                company.Employees = int.Parse(f["Employees"]);
                company.Location = f["location"];
                company.CreatedDate = Convert.ToDateTime(f["CreatedDate"]);
                company.ModifiedDate = Convert.ToDateTime(f["ModifiedDate"]);
                db.SubmitChanges();
                TempData["result"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }
            return View(company);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var company = db.Companies.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(company); }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            //Xóa trong các bảng có liên quan
            var job = db.Jobs.Where(r => r.CompanyID == id).ToList();
            if (job != null)
            {
                db.Jobs.DeleteAllOnSubmit(job);
                db.SubmitChanges();
            }

            var crecom = db.EmployerCreatedCompanies.Where(r => r.CompanyID == id).ToList();
            if (crecom != null)
            {
                db.EmployerCreatedCompanies.DeleteAllOnSubmit(crecom);
                db.SubmitChanges();
            }

            var crecom2 = db.EmployerCreatedCompanies.Where(r => r.EmployerID == r.Employer.ID).ToList();
            if (crecom2 != null)
            {
                db.EmployerCreatedCompanies.DeleteAllOnSubmit(crecom2);
                db.SubmitChanges();
            }

            var employ = db.Employers.Where(r => r.IDCompany == id).ToList();
            if (employ != null)
            {
                db.Employers.DeleteAllOnSubmit(employ);
                db.SubmitChanges();
            }

            //Xóa 
            var company = db.Companies.SingleOrDefault(n => n.ID == id);
            db.Companies.DeleteOnSubmit(company);
            db.SubmitChanges();
            TempData["result"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }
    }
}