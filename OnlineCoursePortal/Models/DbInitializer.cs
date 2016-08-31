using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class DbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<OnlineCoursePortalContext>
    {
        protected override void Seed(OnlineCoursePortalContext context)
        {
            //var Students = new List<Student> { };
            //var Instructors = new List<Instructor> { };

        }
    }
}