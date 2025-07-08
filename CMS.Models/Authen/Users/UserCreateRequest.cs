using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class UserCreateRequest
    {
        [Display(Name = "Tài khoản")]
        public string? UserName { set; get; }
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string? Password { set; get; }
        [Display(Name = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { set; get; }
        [Display(Name = "Họ và Tên")]
        public string? FullName { get; set; }
        [Display(Name = "Giới tính")]
        public int? GenderId { set; get; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { set; get; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }
        [Display(Name = "Ghi chú")]
        public string? Comments { get; set; }
        [Display(Name = "Mầu tương phản")]
        public string? Color { get; set; }
        [Display(Name = "Mầu đại diện")]
        public string? Background { get; set; }
        [Display(Name = "Cửa hàng")]
        public int? ShopId { get; set; }
        [Display(Name = "Trạng thái")]
        public byte? UserStatusId { set; get; }
    }
}
