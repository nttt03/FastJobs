using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobs.Models;
using System.IO;

namespace Jobs.Controllers
{
    public class CVController : Controller
    {
        dbFastJobsDataContext db = new dbFastJobsDataContext();
        
        // GET: CV
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(CV cv, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            //User user = User.Identity;
            //long id = user.Identity.GetUserId();
            User user = (User)Session["Account"];
            if(user == null)
            {
                return RedirectToAction("SignIn", "SigninSignup");
            }
            else
            {
                if (fFileUpload == null)
                {
                    ViewBag.ThongBao = "Hãy chọn Avatar";
                    // Lưu thông tin để khi load lại trang do yêu cầu chọn ảnh bìa sẽ hiển thị các thông tin này lên trang
                    ViewBag.Name = f["sName"];
                    ViewBag.Phone = f["sPhone"];
                    ViewBag.Gmail = f["sGmail"];
                    ViewBag.YourLocation = f["sYourLocation"];
                    ViewBag.Skill1 = f["sSkill1"];
                    ViewBag.Skill2 = f["sSkill2"];
                    ViewBag.AboutMe = f["sAboutMe"];
                    ViewBag.EducationTime = f["sEducationTime"];
                    ViewBag.EducationTime2 = f["sEducationTime2"];
                    ViewBag.EducationName = f["sEducationName"];
                    ViewBag.EducationName2 = f["sJobApp"];
                    return View();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Lấy tên file (Khai báo thư viện: System.IO)
                        var sFileName = Path.GetFileName(fFileUpload.FileName);
                        //Lấy đường dẫn lưu file
                        var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                        //Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                        if (!System.IO.File.Exists(path))
                        {
                            fFileUpload.SaveAs(path);
                        }
                        //Lưu Sạch vào CSDL
                        //int id = 2;
                        cv.IDUser = user.ID;
                        cv.Name = f["sName"];
                        cv.Phone = f["sPhone"];
                        cv.Avatar = sFileName;
                        cv.Gmail = f["sGmail"];
                        cv.YourLocation = f["sYourLocation"];
                        cv.Skill1 = f["sSkill1"];
                        cv.Skill2 = f["sSkill2"];
                        cv.AboutMe = f["sAboutMe"];
                        cv.EducationTime = f["sEducationTime"];
                        cv.EducationTime2 = f["sEducationTime2"];
                        cv.EducationName = f["sEducationName"];
                        cv.EducationName2 = f["sJobApp"];

                        db.CVs.InsertOnSubmit(cv);
                        db.SubmitChanges();
                        CV userCV = db.CVs.SingleOrDefault(n => n.IDUser == user.ID);
                        Session["UserCV"] = userCV;
                        // Về lại trang          
                        return RedirectToAction("Index");
                    }
                    return View();

                }
            }
            

        }





    }
}