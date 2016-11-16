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
using System.Collections;

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
                         orderby c.UploadedDate descending
                         select c).ToList();

            return View(course);
        }

        // GET: Courses/Details/5
        public ActionResult Details(int Cid, int sectionNum, int LectureNum)
        {
            var userId = User.Identity.GetUserId();
            Course course = db.Course.Find(Cid);
            Instructor instructor = db.Instructors.Find(course.InstructorID);
            string userName = instructor.Email;
            //get number of lectures in each section and section names
            ArrayList LecCount = new ArrayList();
            ArrayList sectionNames = new ArrayList();
            for (int i = 1; i <= course.TotalSections; i++)
            {
                string sPath = course.CoursePath + "/section" + i;
                DirectoryInfo dInfo = new DirectoryInfo(Server.MapPath(sPath));
                var str = dInfo.GetDirectories();
                int totLectures = str.Count();
                //get count of lectures in each section
                LecCount.Add(totLectures);
                var secName = System.IO.File.ReadAllText(Server.MapPath(sPath) + @"/SectionName.txt");
                sectionNames.Add((string)secName);
            }
            string path = course.CoursePath;
            string secPath = path + "/section" + sectionNum;
            //get section Name from "secPath"+/SectionName.txt

            string LecPath = secPath + "/Lecture" + LectureNum;
            
            //get list of filenames in the LecturePath
            //check for video file withname "Lecture"+LectureNum
            string LecVideo = "";
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath(LecPath));
            foreach (FileInfo l in dirInfo.GetFiles("Lecture" + LectureNum + ".*"))
            {

                LecVideo = LecPath + "/" + l.Name;
            }
            //check for additional document with name "AddDoc"+LectureNum
            //temp
            string AddDoc = "";
            try
            {
                foreach (FileInfo l in dirInfo.GetFiles("AddDoc" + LectureNum + ".*"))
                {
                    ViewBag.AddDoc = "true";
                    AddDoc = LecPath + "/" + l.Name;
                }
            }
            catch (DirectoryNotFoundException e)
            {
                ViewBag.AddDoc = "false";
            }


            ArrayList quizSecs = new ArrayList((from q in db.Quiz
                                                where q.CourseID == Cid
                                                orderby q.sectionNum
                                                select q.sectionNum).Distinct().ToList());
            //get lecture description from file "LectureDesc"+ LectureNum+".txt"

            var lecDesc = System.IO.File.ReadAllText(Server.MapPath(LecPath) + @"/LectureDesc" + LectureNum + ".txt");
            AddedCourseView courseView = new AddedCourseView
            {
                course = course,
                QuizSecNums = quizSecs,
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

        // GET: Courses/Details/5
        public ActionResult QuizDetails(int Cid, int sectionNum)
        {
            List<Quiz> quiz = new List<Quiz>((from q in db.Quiz
                                              where q.CourseID == Cid && q.sectionNum == sectionNum
                                              select q).ToList());
            Course course = db.Course.Find(Cid);

            ArrayList LecCount = new ArrayList();
            ArrayList sectionNames = new ArrayList();
            for (int i = 1; i <= course.TotalSections; i++)
            {
                string sPath = course.CoursePath + "/section" + i;
                DirectoryInfo dInfo = new DirectoryInfo(Server.MapPath(sPath));
                var str = dInfo.GetDirectories();
                int totLectures = str.Count();
                //get count of lectures in each section
                LecCount.Add(totLectures);
                var secName = System.IO.File.ReadAllText(Server.MapPath(sPath) + @"/SectionName.txt");
                sectionNames.Add((string)secName);
            }
            ArrayList quizSecs = new ArrayList((from q in db.Quiz
                                                where q.CourseID == Cid
                                                orderby q.sectionNum
                                                select q.sectionNum).Distinct().ToList());

            AddedQuizView quizView = new AddedQuizView
            {
                course = course,
                QuizSecNums = quizSecs,
                quiz = quiz,
                LecturesInSection = LecCount,
                sectionNames = sectionNames
            };
            return View(quizView);
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
        public ActionResult AddSections(SectionViewModel section, string submitBtn)
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
            if (submitBtn.CompareTo("Add Next Section") == 0)
            {
                if (section.sectionsTobeAdded == 0)
                {
                    return RedirectToAction("CourseUploadSucess");
                }
                return RedirectToAction("CreateSections");
            }
            else if (submitBtn.CompareTo("Add Quiz") == 0)
            {
                return RedirectToAction("CreateQuiz");
            }
            else if (submitBtn.CompareTo("") == 0)
            {
                return RedirectToAction("CourseUploadSucess");
            }
            if (section.sectionsTobeAdded == 0)
            {
                return RedirectToAction("CourseUploadSucess");
            }
            return RedirectToAction("CreateSections");
        }

        [HttpGet]
        [Authorize(Roles = "Instructor")]
        public ActionResult CreateQuiz()
        {
            ViewBag.sectionsTobeAdded = TempData["sectionsTobeAdded"];
            ViewBag.totSec = TempData["totSec"];
            TempData["totSec"] = ViewBag.totSec;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuiz(FormCollection form, string NumOfQuestions, string sectionsTobeAdded, string totalSections)
        {
            Course course = TempData["course"] as Course;
            TempData["totSec"] = Convert.ToInt32(totalSections);
            TempData["sectionsTobeAdded"] = Convert.ToInt32(sectionsTobeAdded);

            for (int i = 1; i <= Convert.ToInt32(NumOfQuestions); i++)
            {
                string quesID = "question" + i;
                string option1ID = "option1" + i;
                string option2ID = "option2" + i;
                string option3ID = "option3" + i;
                string option4ID = "option4" + i;
                string answerID = "answer" + i;
                Quiz quiz = new Quiz
                {
                    CourseID = course.CourseID,
                    sectionNum = Convert.ToInt32(totalSections) - Convert.ToInt32(sectionsTobeAdded),
                    question = form[quesID],
                    option1 = form[option1ID],
                    option2 = form[option2ID],
                    option3 = form[option3ID],
                    option4 = form[option4ID],
                    answer = Convert.ToInt32(form[answerID])

                };
                db.Quiz.Add(quiz);
                db.SaveChanges();

            }
            if (form["submitBtn"].CompareTo("Add Next Section") == 0)
            {
                if (sectionsTobeAdded.CompareTo("0") == 0)
                {
                    return RedirectToAction("CourseUploadSucess");
                }
                return RedirectToAction("CreateSections");
            }
            else if (form["submitBtn"].CompareTo("") == 0)
            {
                return RedirectToAction("CourseUploadSucess");
            }
            if (sectionsTobeAdded.CompareTo("0") == 0)
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
