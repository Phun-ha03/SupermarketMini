using CMS.Data.Entities.Authen;
using CMS.Data.Enums.Authen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class UserViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Tải khoản")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Họ và Tên")]
        public string FullName { get; set; }
        [Display(Name = "Tên")]
        public string FirstName { get; set; }
        [Display(Name = "Họ và Tên Đệm")]
        public string LastName { get; set; }
        [Display(Name = "Giới thiệu")]
        public string Intro { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }
        [Display(Name = "Ảnh nền")]
        public string CoverPhoto { get; set; }
        [Display(Name = "Giới tính")]
        public int? GenderId { set; get; }
        [Display(Name = "Tên giới tính")]
        public string GenderName { set; get; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { set; get; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ghi chú")]
        public string Comments { get; set; }
        [Display(Name = "OAuthId")]
        public string OAuthId { get; set; }
        [Display(Name = "OAuthName")]
        public string OAuthName { get; set; }
        [Display(Name = "Trạng thái")]
        public byte? UserStatusId { set; get; }
        [Display(Name = "Tên trạng thái")]
        public string UserStatusName { set; get; }
        [Display(Name = "Mầu tương phản")]
        public string Color { set; get; }
        [Display(Name = "Mầu đại diện")]
        public string Background { set; get; }
        [Display(Name = "Cửa hàng")]
        public int? ShopId { get; set; }

        public string Roles { set; get; }
        public bool Deleteable { set; get; }


        public UserViewModel() { }

        public UserViewModel(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            FullName = user.FullName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Intro = user.Intro;
            Avatar = user.Avatar;
            CoverPhoto = user.CoverPhoto;
            GenderId = user.GenderId;
            GenderName = GenderId == 1 ? "Nam" : "Nữ";
            DateOfBirth = user.DateOfBirth;
            Address = user.Address;
            Comments = user.Comments;
            OAuthId = user.OAuthId;
            OAuthName = user.OAuthName;
            UserStatusId = user.UserStatusId;
            Color = user.Color;
            Background = user.Background;
            ShopId = user.ShopId;
            if (UserStatusId == (byte)UserStatusEnum.InActive)
            {
                UserStatusName = "Chưa kích hoạt";
            }
            else if (UserStatusId == (byte)UserStatusEnum.Active)
            {
                UserStatusName = "Đang hoạt động";
            }
            else if (UserStatusId == (byte)UserStatusEnum.Suspend)
            {
                UserStatusName = "Tạm dừng";
            }
            else if (UserStatusId == (byte)UserStatusEnum.Locked)
            {
                UserStatusName = "Bị chặn";
            }
            Roles = string.Empty;
        }
    }
}
