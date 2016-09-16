using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineCoursePortal.Models
{
    [Table("Enrollment")]
    public class Enrollment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnrollmentID { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseID { get; set; }

        [Required]
        [ForeignKey("Student")]
        public string StudentID { get; set; } //Student ID-- ApplicationUser Id 

        public DateTime EnrollmentDate { get; set; }
        public float Progress { get; set; }
        public float pointsEarned { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }
        public virtual ICollection<Notes> Notes { get; set; }

    }
}