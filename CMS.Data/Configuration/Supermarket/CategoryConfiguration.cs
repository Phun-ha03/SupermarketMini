using CMS.Data.Entities.Authen;
using CMS.Data.Entities.Supermarket;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Configuration.Supermarket
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories ");
            builder.HasKey(x => x.CategoryID);
           // builder.Property(x => x.CategoryID).IsRequired(false);
            builder.Property(x => x.Description).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.CategoryName).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.UpdateAt).HasDefaultValue(DateTime.Now);
        }
    }
}
