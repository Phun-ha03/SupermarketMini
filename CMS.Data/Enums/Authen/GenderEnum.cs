
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CMS.Data.Enums.Authen
{
    public enum GenderEnum
    {
        [Display(Name = "Nam", Prompt = "")]
        Male = 1,
        [Display(Name = "Nữ", Prompt = "")]
        Female = 2
    }
}
