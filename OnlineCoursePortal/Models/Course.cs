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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseID { get; set; }

        [Required]
        public string CourseName { get; set; }
        
        [ForeignKey("Instructor")]
        public string InstructorID { get; set; }
        
        public string CoursePath { get; set; }

        [Required]
        public string Category { get; set; }

        public DateTime UploadedDate { get; set; }

        [Required]
        public int Totalpoints { get; set; }

        [Required]
        public string CourseSummary { get; set; }

        [Required]
        [Range(0,5)]
        public int TotalSections { get; set; }

        public virtual Instructor Instructor { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}