using CMS.Data.Entities.Authen;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Configuration.Authen
{
    public class RoleClaimConfiguration:IEntityTypeConfiguration<IdentityRoleClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<int>> builder)
        {
            builder.ToTable("RoleClaims");
            builder.Property(x => x.ClaimType).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.ClaimValue).IsUnicode().HasMaxLength(250);
        }
    }
}
