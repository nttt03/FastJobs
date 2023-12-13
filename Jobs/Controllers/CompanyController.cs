using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobs.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace Jobs.Controllers
{
    public class CompanyController : Controller
    {
        dbFastJobsDataContext data = new dbFastJobsDataContext();
        private List<Company> GetAllCompanies()
        {
            return data.Companies.OrderBy(c => c.ID).ToList();
        }

        // GET: Company
        public ActionResult Index(int ? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 8;
            var listAllCompanies = GetAllCompanies();
            return View(listAllCompanies.ToPagedList(iPageNum, iPageSize));
        }

        public ActionResult Details(int id)
        {

            var company = data.Companies.SingleOrDefault(c => c.ID == id);
            if (company == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(company);

        }

        public ActionResult JobsCompany(int id)
        {
            var jcompany = from Company in data.Companies
                          join Job in data.Jobs on Company.ID equals Job.ID
                          join JobCategory in data.JobCategories on Job.CategoryID equals JobCategory.ID
                          where Company.ID == id
                          select new JobUser
                          {
                              jobdata = Job,
                              companydata = Company,
                              jobcategorydata = JobCategory,
                          };

            if (jcompany == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return PartialView(jcompany);
        }

    }
}