using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;

namespace OnlineCoursePortal.Models
{
    public class Student : Models.ApplicationUser
    {
        public int NoOfCoursesEnrolled { get; set; }
        public string InterestedCategory { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}