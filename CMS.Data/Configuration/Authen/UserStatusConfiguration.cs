using CMS.Data.Entities.Authen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Configuration.Authen
{
    public class UserStatusConfiguration: IEntityTypeConfiguration<UserStatus>
    {
        public void Configure(EntityTypeBuilder<UserStatus> builder)
        {
            builder.ToTable("UserStatuses");
            builder.HasKey(x => x.UserStatusId);
            builder.Property(x => x.UserStatusId).ValueGeneratedNever();
            builder.Property(x => x.UserStatusName).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.UserStatusDesc).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.Color).IsRequired(false).IsUnicode().HasMaxLength(50);
            builder.Property(x => x.BackgroundColor).IsRequired(false).IsUnicode().HasMaxLength(50);
        }

    }
}
