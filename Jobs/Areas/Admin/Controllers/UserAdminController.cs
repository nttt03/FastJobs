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
    public class UserAdminController : Controller
    {
        // GET: Admin/UserAdmin
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
            else { return View(db.Users.ToList().OrderBy(n => n.ID).ToPagedList(iPageNum, iPageSize)); }
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
        public ActionResult Create(User user, FormCollection f, HttpPostedFileBase fFileUpload)
        {       
            ViewBag.UserName = f["UserName"];
            ViewBag.Password = f["Password"];
            ViewBag.Name = f["Name"];
            ViewBag.Address = f["Address"];
            ViewBag.Email = f["Email"];
            ViewBag.Phone = f["Phone"];

            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh đại diện!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);

                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);

                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }

                    user.UserName = f["UserName"];
                    user.Password = f["Password"];
                    user.Name = f["Name"];
                    user.Address = f["Address"];
                    user.Email = f["Email"];
                    user.Phone = f["Phone"];
                    user.Avatar = sFileName;
                    user.CreatedDate = Convert.ToDateTime(f["CreatedDate"]);
                    user.ModifiedDate = Convert.ToDateTime(f["ModifiedDate"]);
                    db.Users.InsertOnSubmit(user);
                    db.SubmitChanges();
                    TempData["result"] = "Thêm người dùng thành công!";
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = db.Users.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(user); }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var user = db.Users.SingleOrDefault(n => n.ID == id);

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
                    user.Avatar = sFileName;
                }
                user.UserName = f["UserName"];
                user.Password = f["Password"];
                user.Name = f["Name"];
                user.Address = f["Address"];
                user.Email = f["Email"];
                user.Phone = f["Phone"];
                user.CreatedDate = Convert.ToDateTime(f["CreatedDate"]);
                user.ModifiedDate = Convert.ToDateTime(f["ModifiedDate"]);
                db.SubmitChanges();
                TempData["result"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = db.Users.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(user); }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            //Xóa trong các bảng có liên quan
            var re = db.Recuments.Where(r => r.UserID == id).ToList();
            if (re != null)
            {
                db.Recuments.DeleteAllOnSubmit(re);
                db.SubmitChanges();
            }

            var crejob = db.EmployerCreatedJobs.Where(r => r.JobID == r.Job.ID).ToList();
            if (crejob != null)
            {
                db.EmployerCreatedJobs.DeleteAllOnSubmit(crejob);
                db.SubmitChanges();
            }

            var job = db.Jobs.Where(r => r.UserID == id).ToList();
            if (job != null)
            {
                db.Jobs.DeleteAllOnSubmit(job);
                db.SubmitChanges();
            }

            var cv = db.CVs.Where(r => r.IDUser == id).ToList();
            if (cv != null)
            {
                db.CVs.DeleteAllOnSubmit(cv);
                db.SubmitChanges();
            }

            var exper = db.Experiences.Where(r => r.UserID == id).ToList();
            if (exper != null)
            {
                db.Experiences.DeleteAllOnSubmit(exper);
                db.SubmitChanges();
            }

            var ski = db.Skills.Where(r => r.UserID == id).ToList();
            if (ski != null)
            {
                db.Skills.DeleteAllOnSubmit(ski);
                db.SubmitChanges();
            }

            //Xóa 
            var user = db.Users.SingleOrDefault(n => n.ID == id);
            db.Users.DeleteOnSubmit(user);
            db.SubmitChanges();
            TempData["result"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }
    }
}