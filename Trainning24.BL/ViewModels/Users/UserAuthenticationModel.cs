using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.UserRole;

namespace Trainning24.BL.ViewModels.Users
{
    public class UserAuthenticationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string AuthId { get; set; }
        public string DeviceType { get; set; }
        public string DeviceToken { get; set; }
        public string Version { get; set; }
    }

    public class AthSignUp
    {     
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public long RoleId { get; set; }
        public List<AddUserRoleModel> Roles { get; set; }
        public string Email { get; set; }
        public bool is_skippable { get; set; }
        public int timeout { get; set; }
        public int reminder { get; set; }
        public bool istimeouton { get; set; }
        public int intervals { get; set; }
        public string phonenumber { get; set; }
    }
}
