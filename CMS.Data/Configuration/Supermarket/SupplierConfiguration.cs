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
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");
            builder.HasKey(x => x.SupplierID);
            //builder.Property(x => x.SupplierID).ValueGeneratedOnAdd();
            builder.Property(x => x.SupplierName).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.ContactPerson).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.Phone).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.Email).IsRequired(true).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.Address).IsRequired(false).IsUnicode().HasMaxLength(250);


        }
    }
}
