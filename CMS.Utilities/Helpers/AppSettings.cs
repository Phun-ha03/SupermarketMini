using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace CMS.Utilities.Helpers
{
    public class AppSettings
    {
        private static AppSettings _appSettings;
        public string AppSettingValue { get; set; }
        public static string AppSetting(string key)
        {
            _appSettings = GetCurrentSettings(key);
            return _appSettings.AppSettingValue;
        }

        public AppSettings(IConfiguration config, string key)
        {
            AppSettingValue = config.GetValue<string>(key);
        }

        public static AppSettings GetCurrentSettings(string key)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var settings = new AppSettings(configuration.GetSection("AppSettings"), key);

            return settings;
        }
        

        public static string StaticFileVersion = !string.Equals(AppSetting("StaticFileVersion"), null, StringComparison.Ordinal)
            ? AppSetting("StaticFileVersion") : "";
        public static string ProcessSuccess = !string.Equals(AppSetting("ProcessSuccess"), null, StringComparison.Ordinal)
            ? AppSetting("ProcessSuccess") : "";
        public static string ProcessFailed = !string.Equals(AppSetting("ProcessFailed"), null, StringComparison.Ordinal)
            ? AppSetting("ProcessFailed") : "";
        public static string DataInvalid = !string.Equals(AppSetting("DataInvalid"), null, StringComparison.Ordinal)
            ? AppSetting("DataInvalid") : "";
        public static string InsertSuccess = !string.Equals(AppSetting("InsertSuccess"), null, StringComparison.Ordinal)
            ? AppSetting("InsertSuccess") : "";
        public static string UpdateSuccess = !string.Equals(AppSetting("UpdateSuccess"), null, StringComparison.Ordinal)
            ? AppSetting("UpdateSuccess") : "";
        public static string DeleteSuccess = !string.Equals(AppSetting("DeleteSuccess"), null, StringComparison.Ordinal)
            ? AppSetting("DeleteSuccess") : "";
        public static string InsertFailed = !string.Equals(AppSetting("InsertFailed"), null, StringComparison.Ordinal)
            ? AppSetting("InsertFailed") : "";
        public static string UpdateFailed = !string.Equals(AppSetting("UpdateFailed"), null, StringComparison.Ordinal)
            ? AppSetting("UpdateFailed") : "";
        public static string DeleteFailed = !string.Equals(AppSetting("DeleteFailed"), null, StringComparison.Ordinal)
            ? AppSetting("DeleteFailed") : "";
        public static string UpdateDataNotFound = !string.Equals(AppSetting("UpdateDataNotFound"), null, StringComparison.Ordinal)
            ? AppSetting("UpdateDataNotFound") : "";
        public static string DeleteDataNotFound = !string.Equals(AppSetting("DeleteDataNotFound"), null, StringComparison.Ordinal)
            ? AppSetting("DeleteDataNotFound") : "";
        public static string MainDomain = !string.Equals(AppSetting("MainDomain"), null, StringComparison.Ordinal)
            ? AppSetting("MainDomain") : "";
        public static string SendMessageMail = !string.Equals(AppSetting("SendMessageMail"), null, StringComparison.Ordinal)
            ? AppSetting("SendMessageMail") : "";
        public static string SendMessageMailPass = !string.Equals(AppSetting("SendMessageMailPass"), null, StringComparison.Ordinal)
            ? AppSetting("SendMessageMailPass") : "goprxdfladrdjqme";

    }
}