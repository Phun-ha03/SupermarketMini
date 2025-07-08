using CMS.Data.Entities.Authen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Configuration.Authen
{
    public class RoleFunctionConfiguration : IEntityTypeConfiguration<RoleFunction>
    {
        public void Configure(EntityTypeBuilder<RoleFunction> builder)
        {
            builder.ToTable("RoleFunctions");
            builder.HasKey(x => new { x.RoleId, x.FunctionId });

            builder.HasOne(x => x.Role)
                .WithMany(x => x.RoleFunctions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Function)
                .WithMany(x => x.RoleFunctions)
                .HasForeignKey(x => x.FunctionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
