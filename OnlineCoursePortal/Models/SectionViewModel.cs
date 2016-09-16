using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class SectionViewModel
    {
        [Required]
        public string sectionName { get; set; }
        [Required]
        public string lectureDescription1 { get; set; }
        [Required]
        public HttpPostedFileBase lecturefile1 { get; set; }
        public HttpPostedFileBase  additionalDoc1 { get; set; }
        public string lectureDescription2 { get; set; }
        public HttpPostedFileBase lecturefile2 { get; set; }
        public HttpPostedFileBase additionalDoc2 { get; set; }
        public string lectureDescription3 { get; set; }
        public HttpPostedFileBase lecturefile3 { get; set; }
        public HttpPostedFileBase additionalDoc3 { get; set; }
        public string lectureDescription4 { get; set; }
        public HttpPostedFileBase lecturefile4 { get; set; }
        public HttpPostedFileBase additionalDoc4 { get; set; }
        public string lectureDescription5 { get; set; }
        public HttpPostedFileBase lecturefile5 { get; set; }
        public HttpPostedFileBase additionalDoc5 { get; set; }
        public int sectionsTobeAdded { get; set; }

    }
}