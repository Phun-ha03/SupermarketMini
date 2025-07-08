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
    public class UserLogConfiguration :IEntityTypeConfiguration<UserLog>
    {
        public void Configure(EntityTypeBuilder<UserLog> builder)
        {
            builder.ToTable("UserLogs");
            builder.HasKey(x => x.UserLogId);
            builder.Property(x => x.UserId).IsRequired(false);
            builder.Property(x => x.IpAddress).IsRequired(false).IsUnicode(true).HasMaxLength(50);
            builder.Property(x => x.ActionId).IsRequired(false);
            builder.Property(x => x.TableName).IsRequired(false).HasMaxLength(150);
            builder.Property(x => x.TableRowId).IsRequired(false).HasColumnType("bigint");
            builder.Property(x => x.CrDateTime).IsRequired(false).HasColumnType("datetime");
            builder.Property(x => x.StatusId).IsRequired(false).HasColumnType("tinyint");

        }
    }
}
