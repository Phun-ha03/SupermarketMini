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
    public class UserTokenConfiguration:IEntityTypeConfiguration<IdentityUserToken<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<int>> builder)
        {
            builder.ToTable("UserTokens");
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.Name).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.Value).IsUnicode().HasMaxLength(250);
        }
    }
}
