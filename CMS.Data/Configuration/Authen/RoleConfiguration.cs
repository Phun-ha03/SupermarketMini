using CMS.Data.Entities.Authen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Configuration.Authen
{
    public class RoleConfiguration :IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.Property(x => x.Name).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.NormalizedName).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.ConcurrencyStamp).IsRequired(false).IsUnicode().HasMaxLength(250);

            builder.Property(x => x.Controler).IsRequired(false).IsUnicode().HasMaxLength(100);
            builder.Property(x => x.Action).IsRequired(false).IsUnicode().HasMaxLength(100);
            builder.Property(x => x.Icon).IsRequired(false).IsUnicode().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired(false).IsUnicode().HasMaxLength(250);
        }
    }
}
