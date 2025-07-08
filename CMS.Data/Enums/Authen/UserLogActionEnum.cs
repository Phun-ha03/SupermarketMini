using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Enums.Authen
{
    public enum UserLogActionEnum
    {

        [Display(Name = "Tạo mới", Prompt = "bg-info")]
        Create = 0,
        [Display(Name = "Cập nhật", Prompt = "bg-warning")]
        Edit = 1,
        [Display(Name = "Xử lý", Prompt = "bg-warning")]
        Process = 2,
        [Display(Name = "Xóa", Prompt = "bg-danger")]
        Delete = 3,
        [Display(Name = "Lỗi", Prompt = "bg-danger")]
        Error = 4
    }
}
