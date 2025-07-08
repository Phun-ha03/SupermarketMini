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
    public class ReturnOrderConfiguration : IEntityTypeConfiguration<ReturnOrder>
    {
        public void Configure(EntityTypeBuilder<ReturnOrder> builder)
        {
            builder.ToTable("ReturnOrder");
            builder.HasKey(x => x.ReturnID);
            builder.Property(x => x.Status).IsRequired(false).IsUnicode().HasMaxLength(250);
            builder.Property(x => x.ReturnDate).HasDefaultValue(DateTime.Now);
            
        }
    }
}
