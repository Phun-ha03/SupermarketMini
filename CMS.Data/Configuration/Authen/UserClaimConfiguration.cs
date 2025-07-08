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
    public class UserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<int>> builder)
        {
            builder.ToTable("UserClaims");
            builder.Property(x => x.ClaimType).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.ClaimValue).IsUnicode().HasMaxLength(250);
        }
    }
}
