using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class UpdateProfileRequest
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

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

        [Display(Name = "Giới thiệu")]
        public string? Intro { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public string? Avatar { get; set; }
        [Display(Name = "Hình nền")]
        public string? CoverPhoto { get; set; }
        [Display(Name = "Mầu tương phản")]
        public string? Color { get; set; }
        [Display(Name = "Mầu đại diện")]
        public string? Background { get; set; }

        public UpdateProfileRequest() { }

        public UpdateProfileRequest(UserViewModel user)
        {
            Id = user.Id;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            FullName = user.FullName;
            GenderId = user.GenderId;
            DateOfBirth = user.DateOfBirth;
            Address = user.Address;
            Intro = user.Intro;
            Avatar = user.Avatar;
            CoverPhoto = user.CoverPhoto;
            Color = user.Color;
            Background = user.Background;
        }
    }
}
