using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Authen
{
    public class User :IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Avatar {  get; set; }
        public string CoverPhoto { get; set; }
        public int? GenderId { set; get; }
        public DateTime? DateOfBirth { set; get; }
        public string Address { get; set; }
        public string Comments { get; set; }
        public string OAuthId { get; set; }
        public string OAuthName { get; set; }
        public DateTime CrDateTime { set; get; }
        public DateTime? ActiveDateTime { set; get; }
        public byte? UserStatusId { set; get; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Intro { get; set; }
        public string Color { get; set; }
        public string Background { get; set; }
        public int? ShopId { get; set; }

        public UserStatus UserStatus { set; get; }
        public Gender Gender { set; get; }
        public List<UserFunction> UserFunctions { get; set; }
    }
}
