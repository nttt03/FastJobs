using Jobs.Models;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Web.UI.WebControls;


namespace Jobs.Areas.Admin.Controllers
{
    public class CVAdminController : Controller
    {
        // GET: Admin/CVAdmin
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
            else { return View(db.CVs.ToList().OrderBy(n => n.ID).ToPagedList(iPageNum, iPageSize)); }
        }
        public ActionResult Details(int id)
        {
            var cv = from s in db.CVs where s.ID == id select s;
            return View(cv.Single());
        }
    }
}