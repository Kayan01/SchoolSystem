﻿using AssessmentSvc.Core.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.Context;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Context
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
        }

        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SchoolClassSubject> SchoolClassSubjects { get; set; }
        public DbSet<AssessmentSetup> AssessmentSetups { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<SessionSetup> SessionSetups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<GradeSetup> GradeSetups { get; set; }
        public DbSet<ApprovedResult> ApprovedResults { get; set; }
        public DbSet<BehaviourResult> BehaviourResults { get; set; }
        public DbSet<StudentIncidence> StudentIncidence { get; set; }
        public DbSet<SchoolPromotionLog> SchoolPromotionLogs { get; set; }
        public DbSet<PromotionSetup> PromotionSetups { get; set; }
        public DbSet<ResultSummary> ResultSummaries { get; set; }
        public DbSet<AttendanceSubject> SubjectAttendance { get; set; }
        public DbSet<AttendanceClass> ClassAttendance { get; set; }

        public async Task AddSampleData()
        {
            TestModels.Add(new TestModel { TenantId = 1, Name = "Frank" });
            TestModels.Add(new TestModel { TenantId = 1, Name = "John" });
            TestModels.Add(new TestModel { TenantId = 2, Name = "Sani" });
            TestModels.Add(new TestModel { TenantId = 2, Name = "Chisom" });

            await SaveChangesAsync();
        }
    }
}
