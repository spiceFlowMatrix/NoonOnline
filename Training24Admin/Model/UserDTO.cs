using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training24Admin.Model
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<long> RoleId { get; set; }
        public List<string> RoleName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ProfilePicUrl { get; set; }
        public bool is_skippable { get; set; }
        public int timeout { get; set; }
        public int reminder { get; set; }
        public bool istimeouton { get; set; }
        public string phonenumber { get; set; }
        public bool isallowmap { get; set; }
    }
}
