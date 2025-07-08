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
    public class StockImportDetailConfiguration : IEntityTypeConfiguration<StockImportDetail>
    {
        public void Configure(EntityTypeBuilder<StockImportDetail> builder)
        {
            builder.ToTable("StockImportDetails");
            builder.HasKey(x => x.ImportDetailID);
            builder.Property(x => x.CostPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.ExpirationDate).HasDefaultValue(DateTime.UtcNow);
            builder.Property(x => x.UsedQuantity).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.StockImport)
      .WithMany(x => x.StockImportDetails)
      .HasForeignKey(x => x.StockImportID)
      .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.ProductUnit)
                .WithMany()
                .HasForeignKey(x => x.UnitID)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
