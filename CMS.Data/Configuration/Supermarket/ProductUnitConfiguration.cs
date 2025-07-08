using CMS.Data.Entities.Supermarket;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Configuration.Supermarket
{
    public class ProductUnitConfiguration : IEntityTypeConfiguration<ProductUnit>
    {
        public void Configure(EntityTypeBuilder<ProductUnit> builder)
        {
            builder.ToTable("ProductUnits");
            builder.HasKey(x => x.UnitID);
            builder.Property(x => x.UnitName).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.ConversionRate).IsRequired();
            builder.Property(x => x.ProductID).IsRequired();
            builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.IsBaseUnit);
    
            builder.HasOne(x => x.Product)
              .WithMany(x => x.ProductUnits)
              .HasForeignKey(x => x.ProductID)
               .OnDelete(DeleteBehavior.Restrict); 

        }
    }
}
