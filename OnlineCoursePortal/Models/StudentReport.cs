using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class StudentReport
    {
        public List<Student> studentList { get; set; }
        public Student selectedstudent { get; set; }
        public int NumOfCourses { get; set; }
        public List<Enrollment> enrollments { get; set; }
    }
}