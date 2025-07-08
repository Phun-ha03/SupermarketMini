using CMS.Data.Entities.Supermarket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Configuration.Supermarket
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.ProductID);
           // builder.Property(x => x.ProductID).IsRequired(false);
            builder.Property(x => x.Name).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.Description).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.ProductCode).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.StockQuantity).IsRequired(true);
            builder.Property(x => x.ExpirationDate).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.Barcode).IsRequired(false).IsUnicode().HasMaxLength(50);
            builder.Property(x => x.SupplierID).IsRequired();
            builder.Property(x => x.CategoryID).IsRequired();
            builder.Property(x => x.Image).IsRequired(false).IsUnicode().HasMaxLength(100); ;
            builder.HasOne(x => x.Category)
               .WithMany(x => x.Products)
               .HasForeignKey(x => x.CategoryID);

            builder.HasOne(x => x.Supplier)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.SupplierID);
               
        }
    }
}
