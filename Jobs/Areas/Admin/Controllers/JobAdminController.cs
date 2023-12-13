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
    public class JobAdminController : Controller
    {
        // GET: Admin/JobAdmin
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
            else { return View(db.Jobs.ToList().OrderBy(n => n.ID).ToPagedList(iPageNum, iPageSize)); }
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
        public ActionResult Create(Job job, FormCollection f, HttpPostedFileBase avatar)
        {
            ViewBag.Name = f["Name"];
            ViewBag.Description = f["Description"];
            ViewBag.RequestCandidate = f["RequestCandidate"];
            ViewBag.SalaryMin = f["SalaryMin"];
            ViewBag.SalaryMax = f["SalaryMax"];
            ViewBag.Rank = f["Rank"];
            ViewBag.Gender = f["Gender"];
            ViewBag.WorkLocation = f["WorkLocation"];
            ViewBag.CreatedDate = f["CreatedDate"];
            ViewBag.ModifiedDate = f["ModifiedDate"];

            if (avatar == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh đại diện!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileNameAva = Path.GetFileName(avatar.FileName);

                    var pathAva = Path.Combine(Server.MapPath("~/Images"), sFileNameAva);

                    if (!System.IO.File.Exists(pathAva))
                    {
                        avatar.SaveAs(pathAva);
                    }

                    job.Name = f["Name"];
                    job.Description = f["Description"];
                    job.RequestCandidate = f["RequestCandidate"];
                    job.SalaryMin = decimal.Parse(f["SalaryMin"]);
                    job.SalaryMax = decimal.Parse(f["SalaryMax"]);
                    job.Deadline = Convert.ToDateTime(f["Deadline"]);
                    job.Rank = f["Rank"];
                    job.Gender = f["Gender"];
                    job.WorkLocation = f["WorkLocation"];
                    job.CreatedDate = Convert.ToDateTime(f["CreatedDate"]);
                    job.ModifiedDate = Convert.ToDateTime(f["ModifiedDate"]);
                    job.Image = sFileNameAva;
                    db.Jobs.InsertOnSubmit(job);
                    db.SubmitChanges();
                    TempData["result"] = "Thêm công việc thành công!";
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var job = db.Jobs.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(job); }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var job = db.Jobs.SingleOrDefault(n => n.ID == id);

            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);

                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    job.Image = sFileName;
                }
                job.Name = f["Name"];
                job.Description = f["Description"];
                job.RequestCandidate = f["RequestCandidate"];
                job.SalaryMin = decimal.Parse(f["SalaryMin"]);
                job.SalaryMax = decimal.Parse(f["SalaryMax"]);
                job.Deadline = Convert.ToDateTime(f["Deadline"]);
                job.Rank = f["Rank"];
                job.Gender = f["Gender"];
                job.WorkLocation = f["WorkLocation"];
                job.CreatedDate = Convert.ToDateTime(f["CreatedDate"]);
                job.ModifiedDate = Convert.ToDateTime(f["ModifiedDate"]);
                db.SubmitChanges();
                TempData["result"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }
            return View(job);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var job = db.Jobs.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(job); }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            //Xóa trong các bảng có liên quan
            var re = db.Recuments.Where(r => r.JobID == id).ToList();
            if (re != null)
            {
                db.Recuments.DeleteAllOnSubmit(re);
                db.SubmitChanges();
            }

            var crejob = db.EmployerCreatedJobs.Where(r => r.JobID == id).ToList();
            if (crejob != null)
            {
                db.EmployerCreatedJobs.DeleteAllOnSubmit(crejob);
                db.SubmitChanges();
            }

            //Xóa 
            var job = db.Jobs.SingleOrDefault(n => n.ID == id);
            db.Jobs.DeleteOnSubmit(job);
            db.SubmitChanges();
            TempData["result"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }
    }
}