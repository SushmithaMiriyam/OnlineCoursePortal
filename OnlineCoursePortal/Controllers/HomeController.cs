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
            var course = (from c in db.Course
                          where c.TotalSections > 0
                          orderby c.UploadedDate descending
                          select c).ToList();
            return View(course);
        }
        [HttpGet]
        public ActionResult Search()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Search(string searchString)
        {
            ViewBag.searchString = searchString;
            var course = (from c in db.Course
                          where c.CourseName.Contains(searchString) || c.CourseSummary.Contains(searchString)
                          orderby c.UploadedDate descending
                          select c).ToList();
            return View(course);
        }
    }
}