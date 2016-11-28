using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class EnrollmentStatus
    {
        public string courseName { get; set; }
        public List<string> studentEmail { get; set; }
        public List<string> status { get; set; }
    }
}