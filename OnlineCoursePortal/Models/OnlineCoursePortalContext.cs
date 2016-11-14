using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace OnlineCoursePortal.Models
{
    public class OnlineCoursePortalContext : DbContext
    {
        public OnlineCoursePortalContext() : base("OnlineCoursePortalContext")
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Quiz> Quiz { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}