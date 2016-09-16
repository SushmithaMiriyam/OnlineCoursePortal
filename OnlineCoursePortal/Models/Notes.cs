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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotesID { get; set; }

        [Required]
        [ForeignKey("Enrollment")]
        public int EnrollmentID { get; set; }

        [Required]
        public string AddedNotes { get; set; }
        public string LecturePath { get; set; }
        public DateTime NotesAddedDate { get; set; }
        public DateTime LastEditDate { get; set; }

        public virtual Enrollment Enrollment { get; set; }
    }
}