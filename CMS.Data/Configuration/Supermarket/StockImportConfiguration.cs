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
    public class StockImportConfiguration : IEntityTypeConfiguration<StockImport>
    {
        public void Configure(EntityTypeBuilder<StockImport> builder)
        {
            builder.ToTable("StockImports");
            builder.HasKey(x => x.StockImportID);
            builder.Property(x => x.TotalCost).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ImportDate).HasDefaultValue(DateTime.UtcNow);
            builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)"); ;
            builder.HasOne(x => x.Supplier)
                .WithMany()
                .HasForeignKey(x => x.SupplierID);

        }
    }
}
