﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Auth.Core.Models;
using Shared.DataAccess.EfCore.Context;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Auth.Core.Models.Users;
using Auth.Core.Models.Setup;
using Auth.Core.Models.Alumni;

namespace Auth.Core.Context
{
    public partial class AppDbContext : ApplicationDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<SchoolProperty> SchoolProperties { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolGroup> SchoolGroups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<TeachingStaff> TeachingStaffs { get; set; }
        public DbSet<SchoolClass> Classes { get; set; }
        public DbSet<ClassArm> ClassGroups { get; set; }
        public DbSet<SchoolSection> SchoolSections { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SchoolTrackRole> SchoolTrackRoles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Alumni> Alumnis{ get; set; }
        public DbSet<PromotionLog> PromotionLogs{ get; set; }
        public DbSet<AlumniEvent> AlumniEvents { get; set; }
        public DbSet<SchoolSubscription> SchoolSubscriptions { get; set; }
        public DbSet<SubscriptionInvoice> SubscriptionInvoices { get; set; }
        public DbSet<PastAlumni> PastAlumnis { get; set; }
    }
}