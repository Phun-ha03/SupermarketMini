using CMS.Data.Enums;
using CMS.Data.Enums.Authen;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.WebApp.Helper
{
    public static class EnumHelper
    {
        public static List<SelectListItem> GenderEnumToSelectList(byte value)
        {
            return Enum.GetValues(typeof(GenderEnum))
                .Cast<GenderEnum>()
                .Select(v => new SelectListItem
                {
                    Text = v.GetDisplayName(),
                    Value = ((byte)v).ToString(),
                    Selected = value == (byte)v
                })
                .ToList();
        }
    }
}
