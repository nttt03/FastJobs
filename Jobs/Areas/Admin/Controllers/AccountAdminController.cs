using Jobs.Models;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Web.UI.WebControls;

namespace Jobs.Areas.Admin.Controllers
{
    public class AccountAdminController : Controller
    {
        // GET: Admin/AccountAdmin
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
            else { return View(db.ADMINs.ToList().OrderBy(n => n.ID).ToPagedList(iPageNum, iPageSize)); }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ADMIN ad = (ADMIN)Session["Admin"];
            
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (ad.Role != 1)
                {
                    TempData["result"] = "Không thể thực hiện thêm Quản trị viên!";
                    return RedirectToAction("Index");
                }
            return View();                   
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ADMIN admin, FormCollection f)
        {
            ViewBag.UserName = f["UserName"];
            ViewBag.Password = f["Password"];
            ViewBag.Name = f["Name"];
            ViewBag.Address = f["Address"];
            ViewBag.Email = f["Email"];
            ViewBag.Phone = f["Phone"];
            ViewBag.Role = f["Role"];

            if (db.ADMINs.SingleOrDefault(n => n.UserName == f["UserName"]) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại!";
            }
            else if (db.ADMINs.SingleOrDefault(n => n.Email == f["Address"]) != null)
            {
                ViewBag.ThongBao = "Email này đã được sử dụng!";
            }
            else
            {
                admin.UserName = f["UserName"];
                admin.Password = f["Password"];
                admin.Name = f["Name"];
                admin.Address = f["Address"];
                admin.Email = f["Email"];
                admin.Phone = f["Phone"];
                admin.Role = int.Parse(f["Role"]);
                db.ADMINs.InsertOnSubmit(admin);
                db.SubmitChanges();
                TempData["result"] = "Thêm Quản trị viên thành công!";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var admin = db.ADMINs.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (admin == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                else { return View(admin); }
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection f)
        {
            ADMIN ad = (ADMIN)Session["Admin"];
            var adlog = db.ADMINs.SingleOrDefault(n => n.ID == ad.ID);
            var admin = db.ADMINs.SingleOrDefault(n => n.ID == id);

            if (ModelState.IsValid)
            {
                if (adlog == admin)
                {
                    adlog.UserName = f["UserName"];
                    adlog.Password = f["Password"];
                    adlog.Name = f["Name"];
                    adlog.Address = f["Address"];
                    adlog.Email = f["Email"];
                    adlog.Phone = f["Phone"];
                    if (ad.Role == 1 && admin.Role == 1)
                    {
                        adlog.Role = int.Parse(f["Role"]);
                    }                        
                    db.SubmitChanges();
                    TempData["result"] = "Cập nhật thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ThongBao = "Bạn không có quyền chỉnh sửa tài khoản của Quản trị viên khác!";
                }
            }
            return View(admin);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var admin = db.ADMINs.SingleOrDefault(n => n.ID == id);
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (admin == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                else { return View(admin); }
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            ADMIN ad = (ADMIN)Session["Admin"];
            var adlog = db.ADMINs.SingleOrDefault(n => n.ID == ad.ID);
            var admin = db.ADMINs.SingleOrDefault(n => n.ID == id);

            if (adlog != admin)
            {
                ViewBag.ThongBao = "Bạn không có quyền xóa tài khoản của Quản trị viên khác!";
            }
            else if (adlog == admin)
            {
                db.ADMINs.DeleteOnSubmit(admin);
                db.SubmitChanges();

                return RedirectToAction("Login", "Home");
            }
            return View();
        }
    }
}