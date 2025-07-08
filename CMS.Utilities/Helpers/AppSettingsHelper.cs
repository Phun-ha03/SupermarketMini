using Microsoft.Extensions.Configuration;
using System.IO;

namespace CMS.Utilities.Helpers
{
    public static partial class AppSettingsHelper
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        public static string GetCurrentSettings(string Property, string? ChildProperty)
        {
            string key = Property;
            key += (!string.IsNullOrWhiteSpace(ChildProperty) ? $":{ChildProperty}" : "");

            var section = _configuration!.GetSection(key);
            return section.Exists() ? section.Value : null;
        }

    }
}
