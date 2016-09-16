using OnlineCoursePortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCoursePortal.Controllers
{
    public class HomeController : Controller
    {
        private OnlineCoursePortalContext db = new OnlineCoursePortalContext();

        public ActionResult Index()
        {
            //return View(db.Courses.ToList());
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ListCourses()
        {

            return View(db.Course.ToList());
        }
    }
}