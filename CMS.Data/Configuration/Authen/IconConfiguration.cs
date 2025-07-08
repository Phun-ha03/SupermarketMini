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
    public class IconConfiguration : IEntityTypeConfiguration<Icon>
    {
        public void Configure(EntityTypeBuilder<Icon> builder)
        {
            builder.ToTable("Icons");
            builder.HasKey(x => x.IconId);

            builder.Property(x => x.IconCode).IsRequired(false).IsUnicode().HasMaxLength(50);
            builder.Property(x => x.IconTypeCode).IsRequired(false).IsUnicode().HasMaxLength(50);
        }
    }
}
