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
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable("Genders");
            builder.HasKey(x => x.GenderId);
            builder.Property(x => x.GenderId).ValueGeneratedNever();
            builder.Property(x => x.GenderName).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.GenderDesc).IsRequired(false).IsUnicode().HasMaxLength(150);
        }
    }
}
