using CMS.Data.Configuration.Authen;
using CMS.Data.Configuration.Supermarket;
using CMS.Data.Entities.Authen;
using CMS.Data.Entities.Supermarket;
using CMS.Data.Extensions;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.EF
{
    public class AICMSDBContext : IdentityDbContext<User, Role, int>
    {
        public AICMSDBContext() { }
        public AICMSDBContext(DbContextOptions<AICMSDBContext> options) : base(options) 
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                var dbType = configuration.GetConnectionString((ConstantHelper.DBTypeParamName).ToLower());

                if (dbType.ToLower().Equals(ConstantHelper.SQLServer.ToLower()))
                {
                    string connectionString = configuration.GetConnectionString((ConstantHelper.SQLServerParamName.ToLower()));
                    optionsBuilder.UseSqlServer(connectionString);
                }
                else if (dbType.ToLower().Equals(ConstantHelper.MySQL.ToLower()))
                {
                    string connectionString = configuration.GetConnectionString((ConstantHelper.MySQLParamName.ToLower()));
                    optionsBuilder.UseMySql(connectionString,
                             ServerVersion.AutoDetect(connectionString),
                             mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                                     maxRetryCount: 10,
                                     maxRetryDelay: TimeSpan.FromSeconds(30),
                                     errorNumbersToAdd: null
                             )
                         );
                }
            }
        }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API
            #region Authen
            modelBuilder.ApplyConfiguration(new UserStatusConfiguration());
            modelBuilder.ApplyConfiguration(new GenderConfiguration());
            modelBuilder.ApplyConfiguration(new UserLogConfiguration());
            modelBuilder.ApplyConfiguration(new UserLogDetailConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginConfiguration());
            modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokenConfiguration());

            modelBuilder.ApplyConfiguration(new FunctionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleFunctionConfiguration());
            modelBuilder.ApplyConfiguration(new UserFunctionConfiguration());
            modelBuilder.ApplyConfiguration(new IconConfiguration());
            #endregion

            #region Supermarket
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ReturnOrderConfiguration());
            modelBuilder.ApplyConfiguration(new StockImportConfiguration());
            modelBuilder.ApplyConfiguration(new StockImportDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductUnitConfiguration());
            
            #endregion
            //Data seeding
            modelBuilder.Seed();
        }
        #region Authen
        public DbSet<UserStatus> UserStatuses { set; get; }
        public DbSet<Gender> Genders { set; get; }
        public DbSet<UserLog> UserLogs { set; get; }
        public DbSet<UserLogDetail> UserLogDetails { set; get; }
        public DbSet<Function> Functions { set; get; }
        public DbSet<RoleFunction> RoleFunctions { set; get; }
        public DbSet<UserFunction> UserFunctions { set; get; }
        public DbSet<Icon> Icons { set; get; }
        #endregion
        #region Supermarket
        public DbSet<Product> Products { set; get; }
        public DbSet<Supplier> Suppliers { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderDetail> OrderDetails { set; get; }
        public DbSet<Customer> Customers { set; get; }
        public DbSet<StockImport> StockImports { set; get; }
        public DbSet<StockImportDetail> StockImportDetails { set; get; }
        public DbSet<ReturnOrder> ReturnOrders { set; get; }
        public DbSet<ProductUnit> ProductUnits { set; get; }

        #endregion

    }
}
