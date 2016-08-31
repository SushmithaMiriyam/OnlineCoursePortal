using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineCoursePortal.Models
{
    [Table("Notes")]
    public class Notes
    {
        [Required]
        [Key]
        public string NotesID { get; set; }

        [Required]
        [ForeignKey("Enrollment")]
        public string EnrollmentID { get; set; }

        [Required]
        public string AddedNotes { get; set; }
        public string LecturePath { get; set; }
        public DateTime NotesAddedDate { get; set; }
        public DateTime LastEditDate { get; set; }

        public virtual Enrollment Enrollment { get; set; }
    }
}