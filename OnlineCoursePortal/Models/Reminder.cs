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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Key]
        public string ReminderID { get; set; }

        [Required]
        [ForeignKey("Enrollment")]
        public string EnrollmentID { get; set; }

        [Required]
        public DateTime ReminderDateTime { get; set; }

        [Required]
        public string ReminderNote { get; set; }

        public virtual Enrollment Enrollment { get; set; }

    }
}