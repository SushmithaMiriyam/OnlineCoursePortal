using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class DbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<OnlineCoursePortalContext>
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected override void Seed(OnlineCoursePortalContext context)
        {

            db.Roles.Add(new IdentityRole()
            {
                Name = "Student"
            });
            db.Roles.Add(new IdentityRole()
            {
                Name = "Instructor"
            });
            db.Roles.Add(new IdentityRole()
            {
                Name = "Admin"
            });

            //var Students = new List<Student> { };
            //var Instructors = new List<Instructor> { };

        }
    }
}