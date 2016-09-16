using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineCoursePortal.Models
{
    [Table("Reminder")]
    public class Reminder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReminderID { get; set; }

        [Required]
        [ForeignKey("Enrollment")]
        public int EnrollmentID { get; set; }

        [Required]
        public DateTime ReminderDateTime { get; set; }

        [Required]
        public string ReminderNote { get; set; }

        public virtual Enrollment Enrollment { get; set; }

    }
}