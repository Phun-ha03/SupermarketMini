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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.OrderID);
            builder.Property(x => x.CustomerID).IsRequired(false);
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Discount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Status).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerID);

            builder
        .HasOne(o => o.CreatedUser)
        .WithMany()
        .HasForeignKey(o => o.CreatedBy)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
