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
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        dbFastJobsDataContext db = new dbFastJobsDataContext();
        public ActionResult Index()
        {
            var kq1 = from u in db.Users select u;
            ViewBag.user = kq1.Count();

            var kq2 = from u in db.Employers select u;
            ViewBag.employer = kq2.Count();

            var kq3 = from u in db.Companies select u;
            ViewBag.company = kq3.Count();

            var kq4 = from u in db.Jobs select u;
            ViewBag.job = kq4.Count();

            var kq5 = from u in db.CVs select u;
            ViewBag.cv= kq5.Count();

            var kq6 = from u in db.Jobs
                      join company in db.Companies on u.CompanyID equals company.ID
                      select new ListEmployerCreated
                      {
                          id = company.ID,
                          idJob = u.ID,
                      };
             ViewBag.fee = kq6.Count();

            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(); }
        }

        public ActionResult Profiles()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(); }
        }

    }
}