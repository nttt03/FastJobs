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
    public class EmployerAdminController : Controller
    {
        // GET: Admin/EmployerAdmin
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
            else { return View(db.Employers.ToList().OrderBy(n => n.ID).ToPagedList(iPageNum, iPageSize)); }
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
        public ActionResult Create(Employer employ, FormCollection f, HttpPostedFileBase fFileUpload)
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

                    employ.UserName = f["UserName"];
                    employ.Password = f["Password"];
                    employ.Name = f["Name"];
                    employ.Address = f["Address"];
                    employ.Email = f["Email"];
                    employ.Phone = f["Phone"];
                    employ.Avatar = sFileName;
                    employ.CreatedDate = Convert.ToDateTime(f["CreatedDate"]);
                    employ.ModifiedDate = Convert.ToDateTime(f["ModifiedDate"]);
                    db.Employers.InsertOnSubmit(employ);
                    db.SubmitChanges();
                    TempData["result"] = "Thêm nhà tuyển dụng thành công!";
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var employ = db.Employers.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(employ); }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var employ = db.Employers.SingleOrDefault(n => n.ID == id);

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
                    employ.Avatar = sFileName;
                }
                employ.UserName = f["UserName"];
                employ.Password = f["Password"];
                employ.Name = f["Name"];
                employ.Address = f["Address"];
                employ.Email = f["Email"];
                employ.Phone = f["Phone"];
                employ.CreatedDate = Convert.ToDateTime(f["CreatedDate"]);
                employ.ModifiedDate = Convert.ToDateTime(f["ModifiedDate"]);
                db.SubmitChanges();
                TempData["result"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }
            return View(employ);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var employ = db.Employers.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(employ); }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var employer = db.Employers.SingleOrDefault(n => n.ID == id);
            db.Employers.DeleteOnSubmit(employer);
            db.SubmitChanges();
            TempData["result"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }
    }
}