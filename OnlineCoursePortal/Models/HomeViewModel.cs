using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class SearchStringViewModel
    {
        [Required]
        public string searchstring { get; set; }
    }
}