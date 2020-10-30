using Auth.Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Map
{
    public class AdminMap : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {

            var admin = new Admin { UserId =1, Id = 1L };

            builder.HasData(admin);
        }
    }
}
