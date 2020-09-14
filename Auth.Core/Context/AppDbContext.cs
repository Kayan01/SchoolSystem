using Microsoft.EntityFrameworkCore;
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

namespace Auth.Core.Context
{
    public partial class AppDbContext : ApplicationDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<TeachingStaff> TeachingStaffs { get; set; }
        public DbSet<SchoolClass> Classes { get; set; }
        public DbSet<ClassArm> ClassGroups { get; set; }
        public DbSet<SchoolSection> SchoolSections { get; set; }
    }
}