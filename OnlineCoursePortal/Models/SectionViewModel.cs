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
        [Display(Name ="Section Name")]
        public string sectionName { get; set; }

        [Required]
        [Display(Name ="1. Lecture Description")]
        public string lectureDescription1 { get; set; }

        [Required]
        [Display(Name ="1. Lecture Video File")]
        [AllowedFileExtension(".mp4")]
        public HttpPostedFileBase lecturefile1 { get; set; }

        [Display(Name ="Additional Document")]
        [AllowedFileExtension(".doc",".docx",".pdf",".txt")]
        public HttpPostedFileBase  additionalDoc1 { get; set; }

        [Display(Name = "2. Lecture Description")]
        public string lectureDescription2 { get; set; }
        [AllowedFileExtension(".mp4")]

        [Display(Name = "2. Lecture Video File")]
        public HttpPostedFileBase lecturefile2 { get; set; }

        [Display(Name = "Additional Document")]
        [AllowedFileExtension(".doc", ".docx", ".pdf", ".txt")]
        public HttpPostedFileBase additionalDoc2 { get; set; }

        [Display(Name = "3. Lecture Description")]
        public string lectureDescription3 { get; set; }

        [Display(Name = "3. Lecture Video File")]
        [AllowedFileExtension(".mp4")]
        public HttpPostedFileBase lecturefile3 { get; set; }

        [Display(Name = "Additional Document")]
        [AllowedFileExtension(".doc", ".docx", ".pdf", ".txt")]
        public HttpPostedFileBase additionalDoc3 { get; set; }

        [Display(Name = "4. Lecture Description")]
        public string lectureDescription4 { get; set; }

        [Display(Name = "4. Lecture Video File")]
        [AllowedFileExtension(".mp4")]
        public HttpPostedFileBase lecturefile4 { get; set; }

        [Display(Name = "Additional Document")]
        [AllowedFileExtension(".doc", ".docx", ".pdf", ".txt")]
        public HttpPostedFileBase additionalDoc4 { get; set; }

        [Display(Name = "5. Lecture Description")]
        public string lectureDescription5 { get; set; }

        [Display(Name = "5. Lecture Video File")]
        [AllowedFileExtension(".mp4")]
        public HttpPostedFileBase lecturefile5 { get; set; }

        [Display(Name = "Additional Document")]
        [AllowedFileExtension(".doc", ".docx", ".pdf", ".txt")]
        public HttpPostedFileBase additionalDoc5 { get; set; }

        public int sectionsTobeAdded { get; set; }
        public int totalSections { get; set; }

    }
    
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowedFileExtensionAttribute : ValidationAttribute
    {
        public string[] AllowedFileExtensions { get; private set; }
        public AllowedFileExtensionAttribute(params string[] allowedFileExtensions)
        {
            AllowedFileExtensions = allowedFileExtensions;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;
            if (file != null)
            {
                if (!AllowedFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase)))
                {
                    return new ValidationResult(string.Format("{1} allowed file extensions: {0} : {2}", string.Join(", ", AllowedFileExtensions), validationContext.DisplayName, this.ErrorMessage));
                }
            }
            return null;
        }
    }
}