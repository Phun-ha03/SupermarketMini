using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class UserEditRequest
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Họ và Tên")]
        public string? FullName { get; set; }
        [Display(Name = "Tên")]
        public string? FirstName { get; set; }
        [Display(Name = "Họ và Tên đệm")]
        public string? LastName { get; set; }
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
        [Display(Name = "Ghi chú")]
        public string? Comments { get; set; }
        [Display(Name = "Mầu tương phản")]
        public string? Color { get; set; }
        [Display(Name = "Mầu đại diện")]
        public string? Background { get; set; }
        [Display(Name = "Trạng thái")]
        public byte? UserStatusId { set; get; }
        public IFormFile? file { get; set; }
        public string? Avatar { get; set; }

        public UserEditRequest() { }

        public UserEditRequest(UserViewModel user)
        {
            Id = user.Id;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            FullName = user.FullName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Intro = user.Intro;
            GenderId = user.GenderId;
            DateOfBirth = user.DateOfBirth;
            Address = user.Address;
            Comments = user.Comments;
            Color = user.Color;
            Background = user.Background;
            UserStatusId = user.UserStatusId;
            Avatar = user.Avatar;
        }
    }
}
