using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class AddedCourseView
    {
        public Course course { get; set; }
        public ArrayList LecturesInSection { get; set; }
        public ArrayList sectionNames { get; set; }
        public string LectureDesc { get; set; }
        public string LectureVideoPath { get; set; }
        public string AddDocPath { get; set; }
        public int sectionNum { get; set; }
        public int lecNum { get; set; }
    }
}