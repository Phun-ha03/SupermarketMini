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
    public class FunctionConfiguration: IEntityTypeConfiguration<Function>
    {
        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.ToTable("Functions");
            builder.HasKey(x => x.FunctionId);
            builder.Property(x => x.Name).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.Controler).IsRequired(false).IsUnicode().HasMaxLength(100);
            builder.Property(x => x.Action).IsRequired(false).IsUnicode().HasMaxLength(100);
            builder.Property(x => x.Icon).IsRequired(false).IsUnicode().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.CrDateTime).HasDefaultValue(DateTime.Now);
        }
    }
}
