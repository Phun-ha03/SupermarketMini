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
    public class UserLoginConfiguration:IEntityTypeConfiguration<IdentityUserLogin<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<int>> builder)
        {
            builder.ToTable("UserLogins");
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.LoginProvider).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.ProviderKey).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.ProviderDisplayName).IsUnicode().HasMaxLength(250);
        }
    }
}
