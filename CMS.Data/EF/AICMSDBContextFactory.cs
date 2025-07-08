using CMS.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.EF
{
    public class AICMSDBContextFactory:IDesignTimeDbContextFactory<AICMSDBContext>
    {
        public AICMSDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AICMSDBContext>();
            var dbType = configuration.GetConnectionString(ConstantHelper.DBTypeParamName).ToLower();
            if (dbType.Equals(ConstantHelper.SQLServer.ToLower()))
            {
                var connectionString = configuration.GetConnectionString(ConstantHelper.SQLServerParamName);
                optionsBuilder.UseSqlServer(connectionString);
            }
            else if (dbType.Equals(ConstantHelper.MySQL.ToLower()))
            {
                var connectionString = configuration.GetConnectionString(ConstantHelper.MySQLParamName);
                optionsBuilder.UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null
                    )
                );
            }


            return new AICMSDBContext(optionsBuilder.Options);
        }
    }
}
