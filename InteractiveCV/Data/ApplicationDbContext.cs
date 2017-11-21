using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InteractiveCV.Models;

namespace InteractiveCV.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //setting the DB's
        public DbSet<Skills> Skills { get; set; }
        public DbSet<SkillLink> SkillLinks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUpdate> ProjectUpdates { get; set; }
        public DbSet<ProjectGoal> ProjectGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Project>().ToTable("Project");
            builder.Entity<ProjectUpdate>().ToTable("ProjectUpdate");
            builder.Entity<ProjectGoal>().ToTable("ProjectGoal");
            builder.Entity<SkillLink>().ToTable("SkillLink");

        }

        
    }
}
