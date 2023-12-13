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
    public class JobsController : Controller
    {
        dbFastJobsDataContext data = new dbFastJobsDataContext();
        

        private List<Company> GetCompanies(int count)
        {
            return data.Companies.OrderByDescending(c => c.Employees).Take(count).ToList();
        }

        // GET: Jobs
        public ActionResult Index()
        {
            var kq1 = from u in data.Users select u;
            ViewBag.user = kq1.Count();

            var kq3 = from c in data.Companies select c;
            ViewBag.company = kq3.Count();

            var kq4 = from j in data.Jobs select j;
            ViewBag.job = kq4.Count();

            return View();
        }
        [HttpGet]
        public ActionResult CareerPartial()
        {
            ViewBag.CategoryID = new SelectList(data.JobCategories.ToList().OrderBy(n => n.TypeJob), "ID", "TypeJob");
            ViewBag.CareerID = new SelectList(data.Careers.ToList().OrderBy(n => n.Name), "ID", "Name");
            return PartialView();
        }

        [HttpGet]
        public ActionResult Search(String searchKey)
        {
            var jobs = from Job in data.Jobs
                         join Company in data.Companies on Job.CompanyID equals Company.ID
                         join JobCategory in data.JobCategories on Job.CategoryID equals JobCategory.ID
                         select new ListJobUser
                         {
                             id = Job.ID,
                             avatar = Company.Avatar,
                             name = Company.Name,
                             nameJob = Job.Name,
                             location = Job.WorkLocation,
                             salaryMin = Job.SalaryMin,
                             salaryMaX = Job.SalaryMax,
                             typeJob = JobCategory.TypeJob,

                         };
            if (!string.IsNullOrEmpty(searchKey))
            {
                jobs = jobs.Where(j => j.nameJob.Contains(searchKey));
            }
            return View(jobs);
        }

        public ActionResult ListSuggest(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 2;
            User user = (User)Session["Account"];
            var jobs = from Job in data.Jobs
                       join Company in data.Companies on Job.CompanyID equals Company.ID
                       join JobCategory in data.JobCategories on Job.CategoryID equals JobCategory.ID
                       select new ListJobUser
                       {
                           id = Job.ID,
                           avatar = Company.Avatar,
                           name = Company.Name,
                           nameJob = Job.Name,
                           location = Job.WorkLocation,
                           salaryMin = Job.SalaryMin,
                           salaryMaX = Job.SalaryMax,
                           typeJob = JobCategory.TypeJob,

                       };
            //var userCV = data.CVs.SingleOrDefault(n => n.IDUser == user.ID);
            //CV userCV = data.CVs.SingleOrDefault(n => n.IDUser == user.ID);
            //if (userCV != null)
            //{
            //    jobs = jobs.Where(j => j.nameJob.Contains(userCV.EducationName2));
            //}
            if(user != null)
            {
                bool hasCV = data.CVs.Any(n => n.IDUser == user.ID);

                if (hasCV)
                {
                    CV userCV = data.CVs.SingleOrDefault(n => n.IDUser == user.ID);
                    jobs = jobs.Where(j => j.nameJob.Contains(userCV.EducationName2));
                }
                else
                {
                    jobs = from Job in data.Jobs
                           join Company in data.Companies on Job.CompanyID equals Company.ID
                           join JobCategory in data.JobCategories on Job.CategoryID equals JobCategory.ID
                           select new ListJobUser
                           {
                               id = Job.ID,
                               avatar = Company.Avatar,
                               name = Company.Name,
                               nameJob = Job.Name,
                               location = Job.WorkLocation,
                               salaryMin = Job.SalaryMin,
                               salaryMaX = Job.SalaryMax,
                               typeJob = JobCategory.TypeJob,

                           };
                }
            }
            else
            {
                jobs = from Job in data.Jobs
                           join Company in data.Companies on Job.CompanyID equals Company.ID
                           join JobCategory in data.JobCategories on Job.CategoryID equals JobCategory.ID
                           select new ListJobUser
                           {
                               id = Job.ID,
                               avatar = Company.Avatar,
                               name = Company.Name,
                               nameJob = Job.Name,
                               location = Job.WorkLocation,
                               salaryMin = Job.SalaryMin,
                               salaryMaX = Job.SalaryMax,
                               typeJob = JobCategory.TypeJob,

                           };
            }

            return PartialView(jobs.ToPagedList(iPageNum, iPageSize));
        }

        public ActionResult Company()
        {
            // Lấy 5 cty nhiều nv nhất
            var listCompanies = GetCompanies(5);
            return PartialView(listCompanies);
        }

        public ActionResult JobsUser()
        {
            
            var result = from Job in data.Jobs
                         join Company in data.Companies on Job.CompanyID equals Company.ID
                         join JobCategory in data.JobCategories on Job.CategoryID equals JobCategory.ID
                         select new ListJobUser
                         {
                             id = Job.ID,
                             avatar = Company.Avatar,
                             name = Company.Name,
                             nameJob = Job.Name,
                             location = Job.WorkLocation,
                             salaryMin = Job.SalaryMin,
                             salaryMaX = Job.SalaryMax,
                             typeJob = JobCategory.TypeJob,
                             
                         };
            return PartialView(result);
        }

        public ActionResult ListCareerPartial()
        {
            var listCareer = from cd in data.Careers select cd;
            return PartialView(listCareer);
        }

        public ActionResult Career(int ID)
        {
            ViewBag.id = ID;
            //var career = from c in data.Careers where c.ID == ID select c;
            var career = from Job in data.Jobs
                         join Company in data.Companies on Job.CompanyID equals Company.ID
                         join JobCategory in data.JobCategories on Job.CategoryID equals JobCategory.ID
                         where Job.CareerID == ID
                         select new ListJobUser
                         {
                             id = Job.ID,
                             avatar = Company.Avatar,
                             name = Company.Name,
                             nameJob = Job.Name,
                             location = Job.WorkLocation,
                             salaryMin = Job.SalaryMin,
                             salaryMaX = Job.SalaryMax,
                             typeJob = JobCategory.TypeJob,

                         };
            if(career == null)
            {
                //ViewBag.ThongBao = "DANH SÁCH CÔNG VIỆC RỖNG";
                //return View();
                Response.StatusCode = 404;
                return null;
            }
            return View(career);
        }

        public ActionResult LoginLogoutPartial()
        {
            return PartialView();
            //return Redirect(url);
        }

        public ActionResult NavPartial()
        {
            return PartialView();
        }

        public ActionResult FooterPartial()
        {
            return PartialView();
        }

        public ActionResult SliderPartial()
        {
            return PartialView();
        }
    }

    
}