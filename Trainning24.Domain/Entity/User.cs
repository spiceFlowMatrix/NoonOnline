using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class User : EntityBase, IUser
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ProfilePicUrl { get; set; }
        public bool IsBlocked { get; set; }
        public bool is_skippable { get; set; }
        public int timeout { get; set; }
        public int reminder { get; set; }
        public bool istimeouton { get; set; }
        public int intervals { get; set; }
        public string phonenumber { get; set; }
        public string AuthId { get; set; }
        public bool isfirstlogin { get; set; }
        public bool isforgot { get; set; }
        public string forgotkey { get; set; }
        public bool is_discussion_authorized { get; set; }
        public bool is_library_authorized { get; set; }
        public bool is_assignment_authorized { get; set; }
        public bool istrial { get; set; }
        public bool isallowmap { get; set; }
    }
}
