
using CMS.Data.Entities.Authen;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using UserStatus = CMS.Data.Entities.Authen.UserStatus;

namespace CMS.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var filePath = string.Empty;

            #region UserStatus sedding
            var userStatuses = new List<UserStatus>();
            filePath = Directory.GetCurrentDirectory() + @"\Extensions\SeedingData\UserStatuses.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) userStatuses = JsonConvert.DeserializeObject<List<UserStatus>>(json);
                }
                modelBuilder.Entity<UserStatus>().HasData(userStatuses);
            }
            #endregion

            #region Gender sedding
            var genders = new List<Gender>();
            filePath = Directory.GetCurrentDirectory() + @"\Extensions\SeedingData\Genders.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) genders = JsonConvert.DeserializeObject<List<Gender>>(json);
                }
                modelBuilder.Entity<Gender>().HasData(genders);
            }
            #endregion

            #region Role sedding
            var roles = new List<Role>();
            filePath = Directory.GetCurrentDirectory() + @"\Extensions\SeedingData\Roles.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) roles = JsonConvert.DeserializeObject<List<Role>>(json);
                }
                modelBuilder.Entity<Role>().HasData(roles);
            }
            #endregion

            #region User sedding
            var users = new List<User>();
            filePath = Directory.GetCurrentDirectory() + @"\Extensions\SeedingData\Users.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) users = JsonConvert.DeserializeObject<List<User>>(json);
                }
                modelBuilder.Entity<User>().HasData(users);
            }
            #endregion

            #region Function sedding
            var functions = new List<Function>();
            filePath = Directory.GetCurrentDirectory() + @"\Extensions\SeedingData\Functions.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) functions = JsonConvert.DeserializeObject<List<Function>>(json);
                }
                modelBuilder.Entity<Function>().HasData(functions);
            }
            #endregion

            #region RoleFunction sedding
            var roleFunctions = new List<RoleFunction>();
            filePath = Directory.GetCurrentDirectory() + @"\Extensions\SeedingData\RoleFunctions.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) roleFunctions = JsonConvert.DeserializeObject<List<RoleFunction>>(json);
                }
                modelBuilder.Entity<RoleFunction>().HasData(roleFunctions);
            }
            #endregion

            #region UserFunction sedding
            var userFunctions = new List<UserFunction>();
            filePath = Directory.GetCurrentDirectory() + @"\Extensions\SeedingData\UserFunctions.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) userFunctions = (JsonConvert.DeserializeObject<List<UserFunction>>(json)).Where(m=>m.UserId == 1 || m.UserId==2).ToList();
                }
                modelBuilder.Entity<UserFunction>().HasData(userFunctions);
            }
            #endregion

            #region IdentityUserRole sedding
            var userRoles = new List<IdentityUserRole<int>>();
            filePath = Directory.GetCurrentDirectory() + @"\Extensions\SeedingData\UserRoles.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) userRoles = JsonConvert.DeserializeObject<List<IdentityUserRole<int>>>(json);
                }
                modelBuilder.Entity<IdentityUserRole<int>>().HasData(userRoles);
            }
            #endregion

            #region Icon sedding
            var icons = new List<Icon>();
            filePath = Directory.GetCurrentDirectory() + @"\Extensions\SeedingData\Icons.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) icons = JsonConvert.DeserializeObject<List<Icon>>(json);
                }
                modelBuilder.Entity<Icon>().HasData(icons);
            }
            #endregion

        }
    }
}
