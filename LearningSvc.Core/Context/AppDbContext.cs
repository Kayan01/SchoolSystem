using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using LearningSvc.Core.Models;
using Shared.DataAccess.EfCore.Context;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LearningSvc.Core.Models.TimeTable;

namespace LearningSvc.Core.Context
{
    public class AppDbContext : ApplicationDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.UseOpenIddict();//Comment in other services other than Auth
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Ignore(typeof(User));
            modelBuilder.Ignore(typeof(Role));
            modelBuilder.Ignore(typeof(UserClaim));
            modelBuilder.Ignore(typeof(UserRole));
            modelBuilder.Ignore(typeof(UserLogin));
            modelBuilder.Ignore(typeof(RoleClaim));
            modelBuilder.Ignore(typeof(UserToken));

            //configure one to one between learningFile and FileUpload
            modelBuilder.Entity<LearningFile>().HasOne(f => f.File)
                .WithOne().HasForeignKey<LearningFile>(m=>m.FileUploadId);

            //configure one to one between Assignment and FileUpload
            modelBuilder.Entity<Assignment>().HasOne(a=>a.Attachment)
                .WithOne().HasForeignKey<Assignment>(m => m.FileUploadId);

            //configure one to one between AssignmentAnswer and FileUpload
            modelBuilder.Entity<AssignmentAnswer>().HasOne(a => a.Attachment)
                .WithOne().HasForeignKey<AssignmentAnswer>(m => m.FileUploadId);

        }

        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<LearningFile> LearningFiles { get; set; }
        public DbSet<Notice> Notices { get; set; }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentAnswer> AssignmentAnswers { get; set; }
        //public DbSet<Attendance> Attendances { get; set; }
        //public DbSet<VirtualClassSession> VirtualClassSessions { get; set; }
        //public DbSet<Exercise> Exercises { get; set; }
        //public DbSet<ExerciseAnswer> ExerciseAnswers { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<TimeTableCell> TimeTableCells { get; set; }
    }
}
