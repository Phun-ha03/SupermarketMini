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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(x => x.CustomerID);
           // builder.Property(x => x.CustomerID).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.Phone).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.Email).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.Address).IsRequired(false).IsUnicode().HasMaxLength(150);
            builder.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);

            builder.HasMany(x => x.Orders) // Một Customer có nhiều Orders
              .WithOne(x => x.Customer) // Mỗi Order có một Customer
              .HasForeignKey(x => x.CustomerID) // Khóa ngoại trong Order trỏ về CustomerID
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
