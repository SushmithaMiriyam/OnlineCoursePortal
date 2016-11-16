using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class EnrolledQuizView
    {
        public int enrollmentID { get; set; }
        public Course course { get; set; }
        public ArrayList QuizSecNums { get; set; }
        public ArrayList LecturesInSection { get; set; }
        public ArrayList sectionNames { get; set; }
        public List<Quiz> quiz { get; set; }
    }
}