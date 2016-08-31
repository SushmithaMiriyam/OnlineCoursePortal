using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineCoursePortal.Models
{
    [Table("Course")]
    public class Course
    {
        [Required]
        [Key]
        public string CourseID { get; set; }

        [Required]
        public string CourseName { get; set; }

        [Required]
        [ForeignKey("Instructor")]
        public string InstructorID { get; set; }

        [Required]
        public string CoursePath { get; set; }

        [Required]
        public string Category { get; set; }

        public DateTime UploadedDate { get; set; }

        [Required]
        public int Totalpoints { get; set; }

        public virtual Instructor Instructor { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}