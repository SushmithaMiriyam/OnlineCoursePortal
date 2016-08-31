using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;

namespace OnlineCoursePortal.Models
{
    public class Instructor : Models.ApplicationUser
    {
        public int TotalCoursesUploaded { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}