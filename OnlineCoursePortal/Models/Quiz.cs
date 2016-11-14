using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    [Table("Quiz")]
    public class Quiz
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuizID { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        public int sectionNum { get; set; }
        public string question { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string option4 { get; set; }
        public int answer { get; set; }


        public virtual Course Course { get; set; }



    }
}