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
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(x => x.OrderDetailID);
            //builder.Property(x => x.OrderDetailID).ValueGeneratedNever();
            builder.Property(x => x.Quantity).IsRequired().HasColumnType("DECIMAL(18,2)");
            builder.Property(x => x.UnitPrice).HasColumnType("DECIMAL(18,2)");
            builder.Property(x => x.OrderID).IsRequired();
            builder.Property(x => x.ProductID).IsRequired();
            builder.Property(x => x.UnitID).IsRequired();

            builder.HasOne(x => x.Product)
               .WithMany()
               .HasForeignKey(x => x.ProductID)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Order)
       .WithMany(o => o.OrderDetails) // nếu Order có ICollection<OrderDetail>
       .HasForeignKey(x => x.OrderID)
       .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProductUnit)
              .WithMany()
              .HasForeignKey(x => x.UnitID);
        }
    }
}
