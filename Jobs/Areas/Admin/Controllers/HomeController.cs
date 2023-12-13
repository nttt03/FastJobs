using Jobs.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;


namespace Jobs.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        dbFastJobsDataContext db = new dbFastJobsDataContext();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var sUserName = f["username"];
            var sPassword = f["password"];

            //Gán giá trị cho đối tượng được tạo mới (ad)
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UserName == sUserName && n.Password == sPassword);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("Admin");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}