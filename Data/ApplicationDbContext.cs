using System.Collections.Generic;
using System.Reflection.Emit;
using Mero_Doctor_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mero_Doctor_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<RatingReview> RatingReviews { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorWeeklyAvailability> DoctorWeeklyAvailabilities { get; set; }
        public DbSet<DoctorWeeklyTimeRange> DoctorWeeklyTimeRanges { get; set; }
        public DbSet<XRayRecord> XRayRecords { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ApplicationUser Relationships
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Doctors)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if they are a doctor

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Patients)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if they are a patient

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.BlogComments)
                .WithOne(bc => bc.User)
                .HasForeignKey(bc => bc.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Set UserId to null if user is deleted (for guest comments)

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Likes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Set UserId to null if user is deleted (for guest likes)

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.RatingReviews)
                .WithOne(rr => rr.User)
                .HasForeignKey(rr => rr.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if they have reviews
          
            modelBuilder.Entity<ApplicationUser>()
              .HasMany(p => p.XRayRecords)
              .WithOne(x => x.User)
              .HasForeignKey(x => x.PatientId)
              .OnDelete(DeleteBehavior.Cascade); // Delete X-ray records if patient is deleted

            modelBuilder.Entity<ApplicationUser>()
               .HasMany(u => u.Notifications)
               .WithOne(n => n.User)
               .HasForeignKey(n => n.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            // Doctor Relationships
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting specialization if doctors exist

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Ratings)
                .WithOne(rr => rr.Doctor)
                .HasForeignKey(rr => rr.DoctorId)
                .OnDelete(DeleteBehavior.Cascade); // Delete ratings if doctor is deleted

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.WeeklyAvailabilities)
                .WithOne(wa => wa.Doctor)
                .HasForeignKey(wa => wa.DoctorId)
                .OnDelete(DeleteBehavior.Cascade); // Delete availabilities if doctor is deleted

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Blogs)
                .WithOne(b => b.Doctor)
                .HasForeignKey(b => b.DoctorId)
                .OnDelete(DeleteBehavior.Cascade); // Delete blogs if doctor is deleted

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting doctor if appointments exist

            // Patient Relationships
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting patient if appointments exist

          

            // Appointment Relationships
            // Already covered under Doctor and Patient (no additional configuration needed)

            // DoctorWeeklyAvailability Relationships
            modelBuilder.Entity<DoctorWeeklyAvailability>()
                .HasMany(wa => wa.TimeRanges)
                .WithOne(tr => tr.WeeklyAvailability)
                .HasForeignKey(tr => tr.DoctorWeeklyAvailabilityId)
                .OnDelete(DeleteBehavior.Cascade); // Delete time ranges if availability is deleted

            // Blog Relationships
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Blogs)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting category if blogs exist

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.BlogComments)
                .WithOne(bc => bc.Blog)
                .HasForeignKey(bc => bc.BlogId)
                .OnDelete(DeleteBehavior.Cascade); // Delete comments if blog is deleted

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Likes)
                .WithOne(l => l.Blog)
                .HasForeignKey(l => l.BlogId)
                .OnDelete(DeleteBehavior.Cascade); // Delete likes if blog is deleted
       
        }

    }

}
