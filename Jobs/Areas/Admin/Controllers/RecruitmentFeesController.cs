using Jobs.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace Jobs.Areas.Admin.Controllers
{
    public class RecruitmentFeesController : Controller
    {
        // GET: Admin/RecruitmentFees
        dbFastJobsDataContext db = new dbFastJobsDataContext();

        public ActionResult Index(int? page)
        {
            var result = from employ in db.Employers
                         join company in db.Companies on employ.IDCompany equals company.ID
                         select new ListJobEmploy
                         {
                             id = employ.ID,
                             name = employ.Name,
                             idCom = company.ID,
                             nameCom = company.Name,
                         };

            int iPageNum = (page ?? 1);
            int iPageSize = 5;
            
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(result.ToPagedList(iPageNum, iPageSize)); }
        }

        public ActionResult Fees(int? page)
        {
            var result = from job in db.Jobs
                         join company in db.Companies on job.CompanyID equals company.ID
                         select new ListEmployerCreated
                         {
                             id = company.ID,
                             idJob = job.ID,
                             money = (int)company.ViewCount,
                             name = company.Name,
                             nameJob = job.Name,
                             date = (DateTime)job.CreatedDate
                         };

            int iPageNum = (page ?? 1);
            int iPageSize = 5;

            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else { return View(result.ToPagedList(iPageNum, iPageSize)); }
        }

    }
}