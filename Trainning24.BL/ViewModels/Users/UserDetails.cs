using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Users
{
    public class UserDetails
    {
        public long id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string rolename { get; set; }
        public string email { get; set; }
        public long roleid { get; set; }
        public string bio { get; set; }
        public string profilepicurl { get; set; }
        public bool is_skippable { get; set; }
        public string phonenumber { get; set; }
    }


    public class DetailUser
    {
        public long assignmentStudentId { get; set; }
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<string> RoleName { get; set; }
        public string Email { get; set; }
        public List<long> Roles { get; set; }
        public string Bio { get; set; }
        public string profilepicurl { get; set; }
        public bool is_skippable { get; set; }
        public int timeout { get; set; }
        public int reminder { get; set; }
        public bool istimeouton { get; set; }
    }

    public class FeedBackUser
    {        
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<string> RoleName { get; set; }
        public string Email { get; set; }
        public List<long> Roles { get; set; }
        public string Bio { get; set; }
        public string profilepicurl { get; set; }
        //public bool is_skippable { get; set; }
        //public int timeout { get; set; }
        //public int reminder { get; set; }
        //public bool istimeouton { get; set; }
    }


    public class AllFeedBackUser
    {
        public long Id { get; set; }
        public string Username { get; set; }
    }
}
