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
    public class UserFunctionConfiguration : IEntityTypeConfiguration<UserFunction>
    {
        public void Configure(EntityTypeBuilder<UserFunction> builder)
        {
            builder.ToTable("UserFunctions");
            builder.HasKey(x => new { x.UserId, x.FunctionId });

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserFunctions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Function)
                .WithMany(x => x.UserFunctions)
                .HasForeignKey(x => x.FunctionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
