using CMS.Data.Entities.Authen;
using CMS.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Configuration.Authen
{
    public class UserLogDetailConfiguration :IEntityTypeConfiguration<UserLogDetail>
    {
        public void Configure(EntityTypeBuilder<UserLogDetail> builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var dbType = configuration.GetConnectionString(ConstantHelper.DBTypeParamName)!.ToLower();

            builder.ToTable("UserLogDetails");
            builder.HasKey(x => x.UserLogDetailId);
            builder.Property(x => x.UserLogId).IsRequired(false);
            builder.Property(x => x.OriginData).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.NewData).IsRequired(false).IsUnicode().HasMaxLength(150);
            if (dbType.ToLower().Equals(ConstantHelper.SQLServer.ToLower()))
            {
                builder.Property(x => x.OriginData).IsRequired(false).IsUnicode().HasColumnType("nvarchar(max)");
                builder.Property(x => x.NewData).IsRequired(false).IsUnicode().HasColumnType("nvarchar(max)");
                builder.Property(x => x.ActionMessage).IsRequired(false).IsUnicode().HasColumnType("nvarchar(max)");
            }
            else if (dbType.ToLower().Equals(ConstantHelper.MySQL.ToLower()))
            {
                builder.Property(x => x.OriginData).IsRequired(false).IsUnicode().HasColumnType("text");
                builder.Property(x => x.NewData).IsRequired(false).IsUnicode().HasColumnType("text");
                builder.Property(x => x.ActionMessage).IsRequired(false).IsUnicode().HasColumnType("text");
            }
            builder.HasOne(x => x.UserLog)
              .WithMany(x => x.UserLogDetails)
              .HasForeignKey(x => x.UserLogId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
