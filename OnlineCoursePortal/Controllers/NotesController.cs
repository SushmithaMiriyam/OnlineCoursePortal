using OnlineCoursePortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCoursePortal.Controllers
{
    public class NotesController : Controller
    {

        // GET: Notes/Create
        public ActionResult Add(string notes, int EnrollmentID, int courseID, int secNum, int LecNum, string lecpath)
        {
            OnlineCoursePortalContext db = new OnlineCoursePortalContext();
            Notes note = new Notes
            {
                AddedNotes = notes,
                EnrollmentID = EnrollmentID,
                LastEditDate = DateTime.Today,
                NotesAddedDate = DateTime.Today,
                LecturePath = lecpath
            };
            db.Notes.Add(note);
            db.SaveChanges();
            return RedirectToAction("Details","Enrollments",new { Cid = courseID, sectionNum = secNum, LectureNum = LecNum });
        }

        
    }
}
