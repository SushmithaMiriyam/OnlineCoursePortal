using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineCoursePortal.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace OnlineCoursePortal.Controllers
{
    public class CoursesController : Controller
    {
        private OnlineCoursePortalContext db = new OnlineCoursePortalContext();

        // GET: Courses
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            // var courses = from c in db.Course
            //              where c.InstructorID == user.Id
            //              select c;
            var course = (from c in db.Course.Include(c => c.Instructor)
                         where c.InstructorID == userId
                         orderby c.UploadedDate 
                         select c).ToList();

            return View(course);
        }

        // GET: Courses/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Instructor")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles ="Instructor")]
        public ActionResult CreateSections()
        {
            ViewBag.sectionsTobeAdded = TempData["sectionsTobeAdded"];
            ViewBag.totSec = TempData["totSec"];
            TempData["totSec"] = ViewBag.totSec;
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles ="Instructor")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSections([Bind(Include = "CourseName,Category,Totalpoints,CourseSummary,TotalSections")] Course course)
        {    //string CName, string CCategory, string totPoints,string CSummary,int TotalSec)
        
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //ApplicationUser user = new ApplicationUser();
            string uname = user.UserName;
            string coursePath = "~/Content/CourseContents/" + uname.Replace('@', '-');
            Directory.CreateDirectory(Server.MapPath(coursePath));
            ViewBag.sectionsTobeAdded = course.TotalSections;
            ViewBag.totSec = course.TotalSections;
            TempData["totSec"] = (int) course.TotalSections;
            course.TotalSections = 0;
            //ViewBag.sectionsTobeAdded = TotalSec;
            //TempData["totSec"] = TotalSec;
            course.InstructorID = user.Id;
            course.UploadedDate = DateTime.Today;
            course.CoursePath = coursePath;
            //Course course = new Course()
            //{
            //    CourseName = CName,
            //    InstructorID = user.Id,
            //    CoursePath = coursePath,
            //    Category = CCategory,
            //    UploadedDate = DateTime.Today,
            //    Totalpoints = totPoints,
            //    CourseSummary = CSummary,
            //    TotalSections = TotalSec

            //};

            if (ModelState.IsValid)
            {
                db.Course.Add(course);
                db.SaveChanges();

                coursePath = coursePath + "/" + course.CourseID;
                Directory.CreateDirectory(Server.MapPath(coursePath));
                course.CoursePath = coursePath;
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
            }

            TempData["course"] = course;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public ActionResult AddSections(SectionViewModel section)
        {
            Course course = TempData["course"] as Course;
            int totsec = section.totalSections;
            TempData["totSec"] = totsec;
            TempData["sectionsTobeAdded"] = section.sectionsTobeAdded;
            int sectionNumber = totsec - section.sectionsTobeAdded;
            string secPath = course.CoursePath + "/section" + sectionNumber;
            Directory.CreateDirectory(Server.MapPath(secPath));
            using (StreamWriter outputFile = new StreamWriter(Server.MapPath(secPath) + @"/SectionName.txt"))
            {
                    outputFile.WriteLine(section.sectionName);
            }
            int i = 1;
            if (section.lecturefile1 != null && section.lecturefile1.ContentLength > 0)
            {

                string lecPath = secPath + "/Lecture" + i;
                Directory.CreateDirectory(Server.MapPath(lecPath));
                var fileName = "Lecture" + i + Path.GetExtension(section.lecturefile1.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath(lecPath), fileName);
                section.lecturefile1.SaveAs(path);
                if (section.additionalDoc1 != null && section.additionalDoc1.ContentLength > 0)
                {
                    fileName = "AddDoc" + i + Path.GetExtension(section.additionalDoc1.FileName);
                    path = Path.Combine(Server.MapPath(lecPath), fileName);
                    section.additionalDoc1.SaveAs(path);
                }


                using (StreamWriter outputFile = new StreamWriter(Server.MapPath(lecPath) + @"/LectureDesc"+i+".txt"))
                {
                    outputFile.WriteLine(section.lectureDescription1);
                }
                course.TotalSections++;
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                i++;
            }
            else
            {
                TempData["sectionsTobeAdded"] = section.sectionsTobeAdded+1;
                Directory.Delete(Server.MapPath(secPath));
                return RedirectToAction("CreateSections");

            }

            if (section.lecturefile2 != null && section.lecturefile2.ContentLength > 0)
            {
                string lecPath = secPath + "/Lecture" + i;
                Directory.CreateDirectory(Server.MapPath(lecPath));
                var fileName = "Lecture" + i + Path.GetExtension(section.lecturefile1.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath(lecPath), fileName);
                section.lecturefile2.SaveAs(path);
                if (section.additionalDoc2 != null && section.additionalDoc2.ContentLength > 0)
                {
                    fileName = "AddDoc" + i + Path.GetExtension(section.additionalDoc2.FileName);
                    path = Path.Combine(Server.MapPath(lecPath), fileName);
                    section.additionalDoc2.SaveAs(path);
                }
                using (StreamWriter outputFile = new StreamWriter(Server.MapPath(lecPath) + @"/LectureDesc" + i + ".txt"))
                {
                    outputFile.WriteLine(section.lectureDescription2);
                }
                i++;
            }

            if (section.lecturefile3 != null && section.lecturefile3.ContentLength > 0)
            {
                string lecPath = secPath + "/Lecture" + i;
                Directory.CreateDirectory(Server.MapPath(lecPath));
                var fileName = "Lecture" + i + Path.GetExtension(section.lecturefile1.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath(lecPath), fileName);
                section.lecturefile3.SaveAs(path);
                if (section.additionalDoc3 != null && section.additionalDoc3.ContentLength > 0)
                {
                    fileName = "AddDoc" + i + Path.GetExtension(section.additionalDoc3.FileName);
                    path = Path.Combine(Server.MapPath(lecPath), fileName);
                    section.additionalDoc3.SaveAs(path);
                }
                using (StreamWriter outputFile = new StreamWriter(Server.MapPath(lecPath) + @"/LectureDesc" + i + ".txt"))
                {
                    outputFile.WriteLine(section.lectureDescription3);
                }
                i++;
            }

            if (section.lecturefile4 != null && section.lecturefile4.ContentLength > 0)
            {
                string lecPath = secPath + "/Lecture" + i;
                Directory.CreateDirectory(Server.MapPath(lecPath));
                var fileName = "Lecture" + i + Path.GetExtension(section.lecturefile1.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath(lecPath), fileName);
                section.lecturefile4.SaveAs(path);
                if (section.additionalDoc4 != null && section.additionalDoc4.ContentLength > 0)
                {
                    fileName = "AddDoc" + i + Path.GetExtension(section.additionalDoc4.FileName);
                    path = Path.Combine(Server.MapPath(lecPath), fileName);
                    section.additionalDoc4.SaveAs(path);
                }
                using (StreamWriter outputFile = new StreamWriter(Server.MapPath(lecPath) + @"/LectureDesc" + i + ".txt"))
                {
                    outputFile.WriteLine(section.lectureDescription4);
                }
                i++;
            }

            if (section.lecturefile5 != null && section.lecturefile5.ContentLength > 0)
            {
                string lecPath = secPath + "/Lecture" + i;
                Directory.CreateDirectory(Server.MapPath(lecPath));
                var fileName = "Lecture" + i + Path.GetExtension(section.lecturefile1.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath(lecPath), fileName);
                section.lecturefile5.SaveAs(path);
                if (section.additionalDoc5 != null && section.additionalDoc5.ContentLength > 0)
                {
                    fileName = "AddDoc" + i + Path.GetExtension(section.additionalDoc5.FileName);
                    path = Path.Combine(Server.MapPath(lecPath), fileName);
                    section.additionalDoc5.SaveAs(path);
                }
                using (StreamWriter outputFile = new StreamWriter(Server.MapPath(lecPath) + @"/LectureDesc" + i + ".txt"))
                {
                    outputFile.WriteLine(section.lectureDescription5);
                }
                i++;
            }
            if (section.sectionsTobeAdded == 0)
            {
                return RedirectToAction("CourseUploadSucess");
            }
            return RedirectToAction("CreateSections");
        }

        public ActionResult CourseUploadSucess()
        {

            return View();
        }
        // GET: Courses/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FirstName", course.InstructorID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,CourseName,InstructorID,CoursePath,Category,UploadedDate,Totalpoints")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FirstName", course.InstructorID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Course course = db.Course.Find(id);
            db.Course.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
