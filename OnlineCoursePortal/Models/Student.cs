using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCoursePortal.Models
{
    [Table("Student")]
    public class Student 
    {
        [Required]
        [Key]
        public string StudentID { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }
        public int NoOfCoursesEnrolled { get; set; }
        public string InterestedCategory { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}