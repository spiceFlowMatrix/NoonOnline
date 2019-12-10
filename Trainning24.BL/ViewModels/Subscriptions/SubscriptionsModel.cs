using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Subscriptions
{
    public class SubscriptionsModel
    {
        public long id { get; set; }
        public List<UserDetails> userids { get; set; }
        public long subscriptionmetadataid { get; set; }
        //public string status { get; set; }
    }

    public class NewSubscriptionsModel
    {
        public long id { get; set; }
        public List<int> userids { get; set; }
        public long subscriptionmetadataid { get; set; }
        public string status { get; set; }
    }

    public class UserDetails
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<string> RoleName { get; set; }
        public string Email { get; set; }
        public List<long> Roles { get; set; }
        public string Bio { get; set; }
        public string profilepicurl { get; set; }
        public List<ParentUser> parents { get; set; }
    }

    public class ParentUser
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<string> RoleName { get; set; }
        public string Email { get; set; }
        public List<long> Roles { get; set; }
        public string Bio { get; set; }
        public string profilepicurl { get; set; }
    }

    public class StudParentDTO
    {
        public long studentids { get; set; }
        public List<long> parentsids { get; set; }
    }

    public class UserDetailsDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<string> RoleName { get; set; }
        public string Email { get; set; }
        public List<long> Roles { get; set; }
        public string Bio { get; set; }
        public string profilepicurl { get; set; }
    }
}
