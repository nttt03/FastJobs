using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobs.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.Optimization;
using System.Web.Security;
using System.Data.Common;

namespace Jobs.Controllers
{
    public class ManagePostsController : Controller
    {
        dbFastJobsDataContext data = new dbFastJobsDataContext();
        //GET: ManagePosts
        public ActionResult PostsIndex()
        {
            Employer emp = (Employer)Session["AccountEmployer"];
            ViewBag.counts = from EmployerCreatedJob in data.EmployerCreatedJobs
                            join Recument in data.Recuments on EmployerCreatedJob.JobID equals Recument.JobID
                            join Job in data.Jobs on Recument.JobID equals Job.ID
                            where EmployerCreatedJob.EmployerID == emp.ID
                            group Recument by new { Job.ID, Job.Name } into g
                            select new ListPosts
                            {
                                count = g.Count(),
                                id = g.Key.ID,


                            }; 
            var posts = from EmployerCreatedJob in data.EmployerCreatedJobs      
                            join Job in data.Jobs on EmployerCreatedJob.JobID equals Job.ID
                            where EmployerCreatedJob.EmployerID == emp.ID
                            select new ListPosts
                            {
                                name = Job.Name,
                                id = Job.ID,

                            };
            return View(posts);
        }
        [HttpGet]
        public ActionResult CVDetail(int id, int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 1;
            var result = from Job in data.Jobs
                     join Recument in data.Recuments on Job.ID equals Recument.JobID
                     join CV in data.CVs on Recument.CVID equals CV.ID
                     where Recument.JobID == id
                     select new ListResult
                     {
                         Recumentdata = Recument,
                         Jobdata = Job,
                         CVdata = CV,
                     };
            return View(result.ToPagedList(iPageNum, iPageSize));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CVDetail(FormCollection f, int id, int? page, int sID)
        {

            int iPageNum = (page ?? 1);
            int iPageSize = 1;
            var result = from Job in data.Jobs
                         join Recument in data.Recuments on Job.ID equals Recument.JobID
                         join CV in data.CVs on Recument.CVID equals CV.ID
                         where Recument.JobID == id
                         select new ListResult
                         {
                             Recumentdata = Recument,
                             Jobdata = Job,
                             CVdata = CV,
                         };
            foreach (var item in result)
            {
                if (item.Recumentdata.ID == sID)
                {
                    item.Recumentdata.Status = int.Parse(f["iStatus"]);
                }
            }
            data.SubmitChanges();
            ViewBag.ThongBao = "Phê duyệt thành công";
            TempData["message2"] = "Phê duyệt thành công!";
            return View(result.ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult LettersDetail(int id, int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 1;
            var result = from Recument in data.Recuments
                         join Job in data.Jobs on Recument.JobID equals Job.ID
                         where Recument.JobID == id && Recument.LetterInfo != null
                         select new ListResult
                         {
                             Recumentdata = Recument,
                             Jobdata = Job,
                         };
            return View(result.ToPagedList(iPageNum, iPageSize));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LettersDetail(Recument StatusToUpdate, FormCollection f, int id, int? page, int sID)
        {

            int iPageNum = (page ?? 1);
            int iPageSize = 1;
            var result = from Recument in data.Recuments
                         join Job in data.Jobs on Recument.JobID equals Job.ID
                         where Recument.JobID == id && Recument.LetterInfo != null
                         select new ListResult
                         {
                             Recumentdata = Recument,
                             Jobdata = Job,
                         };   
            foreach (var item in result){
                if(item.Recumentdata.ID == sID)
                {
                    item.Recumentdata.Status = int.Parse(f["iStatus"]);
                }
            }           
            data.SubmitChanges();
            ViewBag.ThongBao = "Phê duyệt thành công";
            TempData["message2"] = "Phê duyệt thành công!";
            return View(result.ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult findCV(String sAddressKey, String sJobKey)
        {
            //var cvs = from cv in data.CVs select cv;
            //if (!string.IsNullOrEmpty(sAddressKey))
            //{
            //    if(sJobKey != null)
            //    {
            //        cvs = cvs.Where(c => c.YourLocation.Contains(sAddressKey) && c.EducationName2.Contains(sJobKey));
            //    }
            //    else
            //    {
            //        cvs = cvs.Where(c => c.YourLocation.Contains(sAddressKey));
            //    }
            //}
            //ViewBag.count = cvs.Count();
            //return View(cvs);
            //ViewBag.flag = 0;
            if (!string.IsNullOrEmpty(sAddressKey))
            {
                if (sJobKey != null)
                {
                   var cvs = data.CVs.Where(c => c.YourLocation.Contains(sAddressKey) && c.EducationName2.Contains(sJobKey));
                    ViewBag.count = cvs.Count();
                    ViewBag.flag = "1";
                    return View(cvs);
                }
                else
                {
                   var cvs = data.CVs.Where(c => c.YourLocation.Contains(sAddressKey));
                    ViewBag.count = cvs.Count();
                    ViewBag.flag = "1";
                    return View(cvs);
                }
            }
            return View();
        }

        public ActionResult findCVDetail(int idcv)
        {
            var cv = data.CVs.SingleOrDefault(c => c.ID == idcv);
            if (cv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cv);
        }
    }
}