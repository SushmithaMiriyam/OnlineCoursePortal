using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineCoursePortal.Models;
using Microsoft.AspNet.Identity;
using System.Collections;
using System.IO;

namespace OnlineCoursePortal.Controllers
{
    [Authorize(Roles ="Student")]
    public class EnrollmentsController : Controller
    {
        private OnlineCoursePortalContext db = new OnlineCoursePortalContext();

        // GET: Enrollments
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var course = (from c in db.Course
                         join e in db.Enrollments on c.CourseID equals e.CourseID
                         where e.StudentID == userId
                         select c).ToList();


            return View(course);
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int Cid, int sectionNum, int LectureNum)
        {
            string fromEnroll = (string)TempData["messageEnrollSucess"];
            if(fromEnroll == null)
            {
                ViewBag.message = "";
            }
            else if (fromEnroll.CompareTo("Sucess") ==0)
            {
                ViewBag.message = "Sucessfully Enrolled";
            }
            else if(fromEnroll.CompareTo("failure")==0)
            {
                ViewBag.message = "You have already Enrolled";

            }
            var userId = User.Identity.GetUserId();
            /*Enrollment enroll = (Enrollment) (from e in db.Enrollments
                         where e.CourseID == id && e.StudentID == userId
                         select new Enrollment { EnrollmentID=e.EnrollmentID,
                         CourseID=e.CourseID,
                         StudentID=e.StudentID,
                         EnrollmentDate=e.EnrollmentDate,
                         Progress=e.Progress,
                         pointsEarned=e.pointsEarned});*/
            Enrollment enroll = db.Enrollments
                .Where(i => i.StudentID == userId)
                .Where(i=>i.CourseID == Cid).Single();
            var reminders = (from r in db.Reminders
                            where r.EnrollmentID == enroll.EnrollmentID
                            select r).ToList();
            Course course = db.Course.Find(Cid);
            Instructor instructor = db.Instructors.Find(course.InstructorID);
            string userName = instructor.Email;
            //get number of lectures in each section and section names
            ArrayList LecCount = new ArrayList();
            ArrayList sectionNames = new ArrayList();
            for(int i= 1; i <= course.TotalSections; i++) {
                string sPath = course.CoursePath+"/section"+i;
                DirectoryInfo dInfo = new DirectoryInfo(Server.MapPath(sPath));
                var str= dInfo.GetDirectories();
                int totLectures=str.Count();
                //get count of lectures in each section
                LecCount.Add(totLectures);
                var secName = System.IO.File.ReadAllText(Server.MapPath(sPath)+@"/SectionName.txt");
                sectionNames.Add((string)secName);
            }
            string path = course.CoursePath;
            string secPath = path + "/section" + sectionNum;
            //get section Name from "secPath"+/SectionName.txt

            string LecPath = secPath + "/Lecture" + LectureNum;
            var notes = (from n in db.Notes
                        where n.EnrollmentID == enroll.EnrollmentID && n.LecturePath == LecPath
                        select n).ToList();
            //get list of filenames in the LecturePath
            //check for video file withname "Lecture"+LectureNum
            string LecVideo = "";
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath(LecPath));
            foreach (FileInfo l in dirInfo.GetFiles("Lecture"+LectureNum+".*"))
            {
                
                LecVideo = LecPath + "/"+l.Name;
            }
            //check for additional document with name "AddDoc"+LectureNum
            //temp
            string AddDoc = "";
            try { 
            foreach (FileInfo l in dirInfo.GetFiles("AddDoc" + LectureNum + ".*"))
            {
                    ViewBag.AddDoc = true;
                    AddDoc = LecPath + "/" + l.Name;
            }
            }
            catch(DirectoryNotFoundException e)
            {
                ViewBag.AddDoc = false;
            }


            //get lecture description from file "LectureDesc"+ LectureNum+".txt"

            var lecDesc = System.IO.File.ReadAllText(Server.MapPath(LecPath) + @"/LectureDesc"+LectureNum+".txt");
            EnrolledCourseView courseView = new EnrolledCourseView
            {
                enrollment = enroll,
                course = course,
                notes = notes,
                reminders = reminders,
                LectureVideoPath = LecVideo,
                AddDocPath = AddDoc,
                LectureDesc = (string)lecDesc,
                LecturesInSection = LecCount,
                sectionNames = sectionNames,
                sectionNum = sectionNum,
                lecNum = LectureNum
            };
            
            return View(courseView);
        }
        public FileResult downloadAddDoc(string Filepath)
        {
           
            var reg = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Path.GetExtension(Filepath).ToLower());
            string contentType = "application/unknown";

            if (reg != null)
            {
                string registryContentType = reg.GetValue("Content Type") as string;

                if (!String.IsNullOrWhiteSpace(registryContentType))
                {
                    contentType = reg.GetValue("Content Type") as string;
                }
            }

            return File(Filepath, contentType);
            /*string ext = Path.GetExtension(FilePath).ToLower();
            string contenType = "";
            if (ext.CompareTo(".doc")==0){
                contenType = "application / msword";
            }
            else if(ext.CompareTo(".docx") == 0){
                contenType = "application / vnd.openxmlformats - officedocument.wordprocessingml.document";
            }
            else if(ext.CompareTo(".docx") == 0){
                contenType = "application / pdf";
            }
            else if (ext.CompareTo(".txt") == 0)
            {
                contenType=""
            }
            return new FilePathResult(FilePath,)*/
        }

        // GET: Enrollments/Create
        public ActionResult EnrollToCourse()
        {
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName");
            return RedirectToAction("Index");
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public ActionResult EnrollToCourse(int CourseID)
        {
            //[Bind(Include = "EnrollmentID,CourseID,StudentID,EnrollmentDate,Progress,pointsEarned")] Enrollment enrollment)
            Enrollment courseEnrollment = new Enrollment()
            {
                EnrollmentDate = DateTime.Today,
                StudentID = User.Identity.GetUserId(),
                CourseID = CourseID,
                pointsEarned = 0,
                Progress = 0
            };
            var userid = User.Identity.GetUserId();
            var check = (from e in db.Enrollments
                         where e.CourseID == CourseID && e.StudentID == userid
                         select e).ToList();
            if (check.Count == 0)
            {
                db.Enrollments.Add(courseEnrollment);
                db.SaveChanges();
                TempData["messageEnrollSucess"] = "Sucess";
            }
            else
            {
                TempData["messageEnrollSucess"] = "failure";
            }
            return RedirectToAction("Details",new { Cid=CourseID, sectionNum =1, LectureNum =1});
        }

        // GET: Enrollments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,EnrollmentDate,Progress,pointsEarned")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
