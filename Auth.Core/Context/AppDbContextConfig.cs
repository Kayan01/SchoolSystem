using Auth.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Auth.Core.Context
{
    public partial class AppDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseOpenIddict();//Comment in other services other than Auth
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Uncomment in other service other than auth
            /*modelBuilder.Ignore(typeof(User));
            modelBuilder.Ignore(typeof(Role));
            modelBuilder.Ignore(typeof(UserClaim));
            modelBuilder.Ignore(typeof(UserRole));
            modelBuilder.Ignore(typeof(UserLogin));
            modelBuilder.Ignore(typeof(RoleClaim));
            modelBuilder.Ignore(typeof(UserToken));*/

            SeedData(modelBuilder);
        }


        private void SeedData(ModelBuilder builder)
        {
            var schools = new List<School>
            {
                new School{ Id= 1, Name = "Johnson International"},
                 new School{Id= 2,  Name = "Bariga International"},
                 new School{Id= 3,  Name = "Ikeja International"},
            };

            builder.Entity<School>().HasData(schools);

            var secs = new List<SchoolSection>() { new SchoolSection { TenantId = 1, Id = 1, Name = "A" }, new SchoolSection { TenantId = 1, Id = 2, Name = "B" } };

            var cls = new List<SchoolClass>() { new SchoolClass { Id = 1L, Name = "Jss1A", SchoolSectionId = 1 }, new SchoolClass { Id = 2L, Name = "Jss2A", SchoolSectionId = 2} };


            var studts = new List<Student>
            {
                new Student{ Id = 1, TenantId = 1, ClassId = 1L},
                new Student{ Id = 2, TenantId = 1, ClassId = 1L },
                new Student{ Id = 3, TenantId = 2, ClassId = 1L },
                new Student{ Id = 4, TenantId = 2, ClassId = 2L },
                new Student{ Id = 5, TenantId = 3, ClassId = 2L},
                new Student{ Id = 6, TenantId = 3, ClassId = 2L}

            };

            builder.Entity<SchoolSection>().HasData(secs);
            builder.Entity<SchoolClass>().HasData(cls);
            builder.Entity<Student>().HasData(studts);


        }
    }
}
