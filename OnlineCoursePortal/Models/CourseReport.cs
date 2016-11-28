using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class CourseReport
    {
        public List<Course> courseList { get; set; }
        public Course selectedCourse { get; set; }
        public int NumOfStudents { get; set; }
        public List<Enrollment> enrollments { get; set; }
    }
}